using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using EasyModbus;
using System.Data.SQLite;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // =====================================================================
        //  一、常量区 —— 消灭魔法数字
        //      原来 10.0 / 100.0 / 0.9 / 1000 散落在各方法中，
        //      其中 0.9 这个阈值在 CheckAlarm 和 UpdateUI 里各写了一遍，改一处必漏一处。
        // =====================================================================
        private const string ModeCustom = "自定义协议";
        private const string ModeModbus = "Modbus RTU";

        private const double TempScale = 10.0;   // 寄存器原始值 ÷10  → 温度 ℃
        private const double PressureScale = 100.0;  // 寄存器原始值 ÷100 → 压力 MPa
        private const double PressureAlarmLimit = 0.9;    // 压力报警阈值 MPa

        private const int MaxHistoryCount = 1000;      // 内存历史记录上限
        private const int MaxAlarmLogCount = 100;       // 报警日志上限
        private const int MaxChartPoints = 50;        // 曲线最大点数
        private const int MaxFailCount = 3;         // 连续通信失败几次后自动断开
        private const int ReceiveBufferLimit = 4096;      // 接收缓冲区保护上限

        // =====================================================================
        //  二、字段
        // =====================================================================
        private bool isFault = false;            // 是否处于故障状态
        private bool isConnected = false;        // 是否已连接
        private bool isPressureAlarm = false;    // ★关键：上一周期的报警状态，用于边沿触发
        private int continuousFailCount = 0;     // 连续通信失败次数
        private string currentMode = ModeCustom; // 缓存通信模式，供串口后台线程安全读取

        private Random random = new Random();    // 仅 ReadDeviceData() 使用，可整块删除
        private double temperature;
        private double pressure;
        private int readCount;
        private int alarmCount;
        private int faultCount;
        private int sampleIndex = 0;             // 采样序号，作为曲线横轴

        private List<DeviceData> historyList = new List<DeviceData>();
        private string receiveBuffer = "";
        private EasyModbus.ModbusClient modbusClient = null;

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;

            dgvHistory.AutoGenerateColumns = false;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 先减后加：无论 Designer.cs 里是否已经绑定过，都保证只订阅一次。
            // （重复订阅会让同一帧数据被处理两次，报警计数凭空翻倍）
            serialPort1.DataReceived -= SerialPort1_DataReceived;
            serialPort1.DataReceived += SerialPort1_DataReceived;
            InitDatabase();
        }

        // =====================================================================
        //  三、界面数据清理
        // =====================================================================
        private void ClearData()
        {
            txtDeviceId.Clear();
            txtDeviceName.Clear();
            txtTemperature.Clear();
            txtPressure.Clear();
            txtTime.Clear();

            txtalarmCount.Text = "0";
            txtCount.Text = "0";
            txtFaultCount.Text = "0";
        }

        // =====================================================================
        //  四、连接 / 断开
        // =====================================================================
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // ★ 空值保护：机器上没插设备时 cmbPortName 是空的，
            //   原来的 .SelectedItem.ToString() 会直接抛 NullReferenceException
            if (cmbPortName.SelectedItem == null)
            {
                MessageBox.Show("未检测到可用串口，请确认设备已插好并被驱动识别。");
                return;
            }
            if (cmbBaudRate.SelectedItem == null || cmbCommMode.SelectedItem == null)
            {
                MessageBox.Show("请先选择波特率和通信模式。");
                return;
            }

            string portName = cmbPortName.SelectedItem.ToString();
            int baudRate = Convert.ToInt32(cmbBaudRate.SelectedItem);
            string mode = cmbCommMode.SelectedItem.ToString();

            try
            {
                if (mode == ModeModbus)
                {
                    modbusClient = new EasyModbus.ModbusClient(portName);

                    modbusClient.Baudrate = baudRate;
                    modbusClient.Parity = System.IO.Ports.Parity.None;
                    modbusClient.StopBits = System.IO.Ports.StopBits.One;
                    modbusClient.UnitIdentifier = 1;

                    // 限制超时，避免设备离线时同步读把 UI 线程卡死
                    // （如果你的 EasyModbus 版本没有这个属性，删掉这一行即可）
                    modbusClient.ConnectionTimeout = 1000;

                    modbusClient.Connect();
                }
                else
                {
                    if (serialPort1.IsOpen)
                        serialPort1.Close();

                    serialPort1.PortName = portName;
                    serialPort1.BaudRate = baudRate;
                    serialPort1.DataBits = 8;
                    serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    serialPort1.Parity = System.IO.Ports.Parity.None;
                    serialPort1.Open();
                }

                currentMode = mode;   // 供后台接收线程安全读取
                isConnected = true;
                isPressureAlarm = false;
                continuousFailCount = 0;
                receiveBuffer = "";

                txtStatus.Text = "设备已连接";
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnRead.Enabled = true;
                timerMonitor.Start();
            }
            catch (Exception ex)
            {
                // ★ 连接失败必须回滚，否则会残留一个"new 出来了但没连上"的 ModbusClient
                DisconnectDevice();
                MessageBox.Show("连接失败：" + ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            DisconnectDevice();
        }

        /// <summary>
        /// 统一的断开收尾逻辑：用户点"断开"和"通信连续失败自动断开"共用这一套，
        /// 避免两处收尾不一致（原来只写在按钮事件里）。
        /// </summary>
        private void DisconnectDevice()
        {
            timerMonitor.Stop();
            isConnected = false;

            string errMsg = null;
            try
            {
                if (modbusClient != null)
                {
                    if (modbusClient.Connected)
                        modbusClient.Disconnect();
                    modbusClient = null;
                }

                if (serialPort1.IsOpen)
                    serialPort1.Close();
            }
            catch (Exception ex)
            {
                // 断开过程中的异常不弹窗打断用户，写进状态栏即可
                errMsg = "关闭串口异常：" + ex.Message;
            }
            finally
            {
                isPressureAlarm = false;
                continuousFailCount = 0;
                readCount = 0;
                alarmCount = 0;
                faultCount = 0;

                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnRead.Enabled = false;

                ClearData();
                txtStatus.Text = errMsg ?? "设备已断开";
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            MessageBox.Show("当前为串口自动接收模式，无需手动读取。");
        }

        // =====================================================================
        //  五、定时轮询（原代码的"定时炸弹"就在这里）
        // =====================================================================
        private void timerMonitor_Tick(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                txtStatus.Text = "设备未连接";
                timerMonitor.Stop();
                return;
            }

            if (currentMode == ModeModbus && modbusClient != null)
            {
                try
                {
                    if (!modbusClient.Connected)
                        throw new InvalidOperationException("Modbus 链路已断开");

                    int[] registers = modbusClient.ReadHoldingRegisters(0, 2);
                    temperature = registers[0] / TempScale;
                    pressure = registers[1] / PressureScale;
                    readCount++;

                    continuousFailCount = 0;   // ★ 通信成功，失败计数归零
                    CheckAlarm();
                    UpdateUI();
                }
                catch (Exception ex)
                {
                    // ★ 原来只改状态栏文字、不停定时器 → 每秒报一次错，无限刷屏
                    HandleCommFailure("读取失败：" + ex.Message);
                }
            }
            else // 自定义协议：这里只负责发指令，数据由 DataReceived 事件接收
            {
                if (!serialPort1.IsOpen)
                {
                    txtStatus.Text = "串口未打开";
                    timerMonitor.Stop();
                    return;
                }

                try
                {
                    serialPort1.Write("DATA\n");
                    continuousFailCount = 0;
                }
                catch (Exception ex)
                {
                    HandleCommFailure("发送失败：" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 统一处理通信失败：累计次数，达到上限就自动断开并提示，
        /// 避免用户看到一片滚动的报错却不知道该干什么。
        /// </summary>
        private void HandleCommFailure(string message)
        {
            continuousFailCount++;
            txtStatus.Text = $"{message}（连续失败 {continuousFailCount}/{MaxFailCount} 次）";

            if (continuousFailCount >= MaxFailCount)
            {
                DisconnectDevice();
                MessageBox.Show($"通信连续失败 {MaxFailCount} 次，已自动断开。请检查设备供电与线路。",
                                "通信中断");
            }
        }

        // =====================================================================
        //  六、报警判定（★核心修复）
        // =====================================================================
        /// <summary>
        /// 报警日志统一入口，避免多处重复写 Insert + 上限裁剪。
        /// </summary>
        private void AddAlarmLog(string message)
        {
            lstAlarmLog.Items.Insert(0, $"{DateTime.Now:HH:mm:ss} {message}");
            if (lstAlarmLog.Items.Count > MaxAlarmLogCount)
                lstAlarmLog.Items.RemoveAt(lstAlarmLog.Items.Count - 1);
        }

        /// <summary>
        /// 报警判定。
        ///
        /// 原代码两个问题：
        ///   ① 同一个 if (pressure > 0.9) 写了两遍 → 计数 +2、蜂鸣器响两次、日志写两条；
        ///   ② 更严重的是没有"边沿触发"：本方法每秒被定时器调用一次，
        ///      只要压力持续超标，报警计数就会每秒钟涨一次（高 10 秒 = 报 10 次警）。
        ///
        /// 工业上的报警是一个"事件"——状态从正常变成超标的那个瞬间，而不是一个持续状态。
        /// 因此这里用 isPressureAlarm 记住上一周期的判断结果，只在 0→1 的上升沿计数。
        /// </summary>
        private void CheckAlarm()
        {
            // 故障优先级最高：故障期间不参与压力报警判定
            if (isFault)
            {
                txtStatus.Text = "设备故障";
                return;   // 注意：此时不更新 isPressureAlarm，故障恢复后不会误判为新报警
            }

            bool alarmNow = pressure > PressureAlarmLimit;   // 全程序只在这里判断一次

            if (alarmNow)
            {
                txtStatus.Text = $"压力报警（{pressure:F2} MPa）";

                // ★ 边沿触发：只有"上一周期正常 → 本周期报警"才计数、响铃、记日志
                if (!isPressureAlarm)
                {
                    alarmCount++;
                    System.Media.SystemSounds.Beep.Play();
                    AddAlarmLog($"压力报警 {pressure:F2} MPa");
                }
            }
            else
            {
                txtStatus.Text = "设备正常";

                // 报警解除：记一条恢复日志（工业软件的标准做法，不需要可删掉这 2 行）
                if (isPressureAlarm)
                    AddAlarmLog($"压力恢复正常 {pressure:F2} MPa");
            }

            isPressureAlarm = alarmNow;   // 保存本周期状态，供下一周期比较
        }

        private void btnFault_Click(object sender, EventArgs e)
        {
            if (isFault) return;          // 已故障不重复计数（原代码这里仍会调 UpdateUI）

            isFault = true;
            faultCount++;
            txtStatus.Text = "设备故障";
            AddAlarmLog("设备故障（模拟）");
            UpdateUI();
        }

        private void btnRecover_Click(object sender, EventArgs e)
        {
            isFault = false;
            CheckAlarm();                 // 重新评估；边沿触发保证不会重复计数
            AddAlarmLog("设备故障已恢复");
            UpdateUI();
        }

        // =====================================================================
        //  七、界面刷新
        // =====================================================================
        private void UpdateUI()
        {
            txtDeviceId.Text = "10001";
            txtDeviceName.Text = "温度采集器";
            txtTemperature.Text = temperature.ToString("0.0");
            txtPressure.Text = pressure.ToString("0.00");
            txtTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            txtCount.Text = readCount.ToString();
            txtalarmCount.Text = alarmCount.ToString();
            txtFaultCount.Text = faultCount.ToString();

            // 历史记录：报警判定复用同一个阈值常量，不会和 CheckAlarm 出现不一致
            bool isAlarmNow = pressure > PressureAlarmLimit;
            historyList.Add(new DeviceData(temperature, pressure, isAlarmNow, txtStatus.Text));
            SaveToDatabase(temperature, pressure);
            if (historyList.Count > MaxHistoryCount)
                historyList.RemoveAt(0);

            Series sTemp = chartMain.Series["温度"];
            Series sPress = chartMain.Series["压力"];
            if (sTemp == null || sPress == null) return;

            // ★ X 轴用"采样序号"：原来是"当天已过秒数"，一到 24:00 归零会让曲线回折
            sampleIndex++;
            sTemp.Points.AddXY(sampleIndex, temperature);
            sPress.Points.AddXY(sampleIndex, pressure);   // 压力挂在 AxisY2，无需再 ×100

            while (sTemp.Points.Count > MaxChartPoints) sTemp.Points.RemoveAt(0);
            while (sPress.Points.Count > MaxChartPoints) sPress.Points.RemoveAt(0);

            if (sTemp.Points.Count > 0)
            {
                chartMain.ChartAreas[0].AxisX.Minimum = sTemp.Points[0].XValue;
                chartMain.ChartAreas[0].AxisX.Maximum = sTemp.Points[sTemp.Points.Count - 1].XValue;
            }
        }
        // =====================================================================
        //  数据库初始化与保存方法
        // =====================================================================

        // 1. 建表方法（程序启动时执行一次）
        private void InitDatabase()
        {
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=HistoryData.db;Version=3;"))
            {
                conn.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS History (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                RecordTime TEXT,
                                Temperature REAL,
                                Pressure REAL
                               )";
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 2. 存数据方法（每次收到数据就存进去）
        private void SaveToDatabase(double temp, double press)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=HistoryData.db;Version=3;"))
                {
                    conn.Open();
                    string sql = "INSERT INTO History (RecordTime, Temperature, Pressure) VALUES (@time, @t, @p)";
                    using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@t", temp);
                        cmd.Parameters.AddWithValue("@p", press);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("写数据库失败：" + ex.Message);
            }
        }
        // =====================================================================
        //  八、串口接收与报文解析
        // =====================================================================
        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // 这个事件在后台线程上执行，因此这里不要访问任何控件属性，
            // 只读缓存字段 currentMode（原代码直接读 cmbCommMode.SelectedItem）
            if (currentMode == ModeModbus) return;

            if (!serialPort1.IsOpen) return;   // 串口可能正在关闭，先判断

            string newData;
            try
            {
                newData = serialPort1.ReadExisting();
            }
            catch (Exception)
            {
                // 关闭瞬间的读取失败属于正常现象，直接忽略
                // （原代码会把它弹到状态栏显示"接收异常"，误导使用者）
                return;
            }

            receiveBuffer += newData;

            // 防止下位机一直不发换行符，导致缓冲区无限增长
            if (receiveBuffer.Length > ReceiveBufferLimit)
                receiveBuffer = receiveBuffer.Substring(receiveBuffer.Length - ReceiveBufferLimit / 2);

            while (true)
            {
                int newLineIndex = receiveBuffer.IndexOf('\n');
                if (newLineIndex == -1) break;

                string line = receiveBuffer.Substring(0, newLineIndex).TrimEnd('\r');
                receiveBuffer = receiveBuffer.Substring(newLineIndex + 1);

                ProcessLine(line);
            }
        }

        private void ProcessLine(string line)
        {
            if (string.IsNullOrEmpty(line)) return;

            string[] parts = line.Split(',');
            if (parts.Length < 2) return;

            // 有第三列时做异或校验，校验失败直接丢弃这一帧
            if (parts.Length >= 3)
            {
                byte xor = 0;
                string payload = parts[0] + "," + parts[1];
                foreach (char c in payload) xor ^= (byte)c;

                if (!xor.ToString("X2").Equals(parts[2].Trim(), StringComparison.OrdinalIgnoreCase))
                    return;
            }

            // ★ InvariantCulture：无论系统区域怎么设置，都按 "." 解析小数
            //   （原代码的 Convert.ToDouble 在法语/德语等区域会把 23.5 解析成 235）
            // ★ TryParse：脏数据不会抛异常把程序打断
            if (!double.TryParse(parts[0].Trim(), NumberStyles.Float,
                                 CultureInfo.InvariantCulture, out double t)) return;
            if (!double.TryParse(parts[1].Trim(), NumberStyles.Float,
                                 CultureInfo.InvariantCulture, out double p)) return;

            SafeInvoke(() =>
            {
                temperature = t;
                pressure = p;
                readCount++;
                continuousFailCount = 0;
                CheckAlarm();
                UpdateUI();
            });
        }

        /// <summary>
        /// 安全更新界面。
        /// 程序正在关闭时，后台线程可能还在跑，此时控件已销毁 → Invoke 会抛
        /// ObjectDisposedException 直接把程序搞崩。这里先做存活判断再更新。
        /// </summary>
        private void SafeInvoke(Action action)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;

            try
            {
                if (this.InvokeRequired)
                    this.BeginInvoke(action);   // 异步投递，不阻塞串口接收线程
                else
                    action();
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        // =====================================================================
        //  九、手动发送 / Modbus 写寄存器
        // =====================================================================
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("串口未打开");
                return;
            }

            try
            {
                string cmd = txtSend.Text + "\n";   // 加换行，方便下位机用 ReadLine
                serialPort1.Write(cmd);
                txtSend.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送失败：" + ex.Message);
            }
        }

        private void btnWriteReg_Click(object sender, EventArgs e)
        {
            if (cmbCommMode.SelectedItem == null || cmbCommMode.SelectedItem.ToString() != ModeModbus
                || modbusClient == null)
            {
                MessageBox.Show("请在 Modbus RTU 模式下使用此功能。");
                return;
            }

            if (!modbusClient.Connected)
            {
                MessageBox.Show("Modbus 设备未连接。");
                return;
            }

            if (!int.TryParse(txtRegAddr.Text, out int address))
            {
                MessageBox.Show("寄存器地址必须是整数。");
                return;
            }

            if (!int.TryParse(txtRegValue.Text, out int value))
            {
                MessageBox.Show("寄存器值必须是整数。");
                return;
            }

            try
            {
                modbusClient.WriteSingleRegister(address, value);
                AddAlarmLog($"写入寄存器[{address}] = {value}");
                MessageBox.Show($"写入成功：寄存器[{address}] ← {value}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"写入失败：{ex.Message}");
            }
        }

        // =====================================================================
        //  十、历史记录查看与导出
        // =====================================================================
        private void btnShowHistory_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=HistoryData.db;Version=3;"))
                {
                    conn.Open();
                    string sql = "SELECT RecordTime AS 采集时间, Temperature AS 温度, Pressure AS 压力 FROM History ORDER BY Id DESC LIMIT 100";
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // ★ 核心修复：清空原本设计器里堵住的列，直接全盘接住数据库的列
                        dgvHistory.DataSource = null;
                        dgvHistory.Columns.Clear();
                        dgvHistory.AutoGenerateColumns = true;
                        dgvHistory.DataSource = dt;
                        dgvHistory.AllowUserToAddRows = false;
                        dgvHistory.AutoResizeColumns();

                        MessageBox.Show($"共 {dt.Rows.Count} 条记录");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询历史记录失败：" + ex.Message);
            }
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV文件|*.csv";
                saveDialog.FileName = $"History_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                // UTF8 带 BOM，中文用 Excel 打开不会乱码
                using (StreamWriter sw = new StreamWriter(saveDialog.FileName, false, new UTF8Encoding(true)))
                {
                    sw.WriteLine("采集时间,温度(℃),压力(MPa)"); // 去掉原来的"状态"列，因为数据库没存

                    // ★ 改为直接从数据库查询导出，不再依赖内存 historyList
                    using (SQLiteConnection conn = new SQLiteConnection("Data Source=HistoryData.db;Version=3;"))
                    {
                        conn.Open();
                        string sql = "SELECT RecordTime, Temperature, Pressure FROM History ORDER BY Id DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                string time = reader["RecordTime"].ToString();
                                string temp = Convert.ToDouble(reader["Temperature"]).ToString("F1");
                                string press = Convert.ToDouble(reader["Pressure"]).ToString("F2");

                                sw.WriteLine($"{time},{temp},{press}");
                                count++;
                            }
                            MessageBox.Show($"导出成功！共 {count} 条记录。", "提示");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败：" + ex.Message, "错误");
            }
        }

        // =====================================================================
        //  十一、窗体初始化
        // =====================================================================
        private void Form1_Load(object sender, EventArgs e)
        {
            // ---- 串口列表 ----
            cmbPortName.Items.Clear();
            foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
                cmbPortName.Items.Add(port);

            if (cmbPortName.Items.Count > 0)
            {
                int com1Index = -1;
                for (int i = 0; i < cmbPortName.Items.Count; i++)
                {
                    if (cmbPortName.Items[i].ToString() == "COM1")
                    {
                        com1Index = i;
                        break;
                    }
                }
                cmbPortName.SelectedIndex = com1Index >= 0 ? com1Index : 0;
            }

            // ---- 波特率 ----
            cmbBaudRate.Items.Clear();
            cmbBaudRate.Items.AddRange(new object[] { 9600, 19200, 38400, 57600, 115200 });
            cmbBaudRate.SelectedIndex = 0;

            // ---- 通信模式（用常量，避免字符串写错）----
            cmbCommMode.Items.Clear();
            cmbCommMode.Items.AddRange(new object[] { ModeCustom, ModeModbus });
            cmbCommMode.SelectedIndex = 0;
            currentMode = ModeCustom;

            // ---- 轮询周期 ----
            timerMonitor.Interval = 1000;

            // ---- 图表 ----
            chartMain.Series.Clear();
            chartMain.ChartAreas.Clear();

            ChartArea area = new ChartArea("Area1");
            area.AxisX.Title = "采样点";
            area.AxisX.MajorGrid.LineColor = Color.LightGray;

            // 主轴：温度 0~40 ℃
            area.AxisY.Title = "温度 (℃)";
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 40;
            area.AxisY.Interval = 10;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            // ★ 次轴：压力 0~1.5 MPa —— 原来靠"压力 ×100"挤在同一条轴上，
            //   读数要靠心算换算，工业界不这么干
            area.AxisY2.Title = "压力 (MPa)";
            area.AxisY2.Minimum = 0;
            area.AxisY2.Maximum = 1.5;
            area.AxisY2.Interval = 0.3;
            area.AxisY2.Enabled = AxisEnabled.True;
            area.AxisY2.MajorGrid.Enabled = false;
            // ★ 锁定图表布局：最大化（宽>高）时防止 X 轴刻度被挤出
            area.Position = new ElementPosition(2, 3, 96, 92);
            area.InnerPlotPosition = new ElementPosition(10, 3, 82, 80);

            chartMain.ChartAreas.Add(area);   // ← 原有行，不动

          

            // ---- 温度曲线（主轴）----
            Series seriesTemp = new Series("温度");
            seriesTemp.ChartType = SeriesChartType.Line;
            seriesTemp.BorderWidth = 2;
            seriesTemp.Color = Color.Red;
            seriesTemp.YAxisType = AxisType.Primary;
            chartMain.Series.Add(seriesTemp);

            // ---- 压力曲线（次轴）----
            Series seriesPress = new Series("压力");
            seriesPress.ChartType = SeriesChartType.Line;
            seriesPress.BorderWidth = 2;
            seriesPress.Color = Color.Blue;
            seriesPress.YAxisType = AxisType.Secondary;
            chartMain.Series.Add(seriesPress);
            this.Resize += (s, ev) => ApplyAdaptiveLayout();
            ApplyAdaptiveLayout();


        }

        // =====================================================================
        //  十二、清空按钮（修复"假清空"）
        // =====================================================================
        // ═══ 自适应分割：最大化时把上下分割线下移到 60% 处 ═══
        private void ApplyAdaptiveLayout()
        {
            int bottomEdge = ClientSize.Height - statusStrip1.Height - 6;  // 下块底边（状态栏上方）
            int splitY = Math.Max(446, (int)(bottomEdge * 0.60));          // 分割线：默认446，最大化移到60%

            // 上块三个框拉伸到分割线
            grpChart.Height = splitY - 14 - grpChart.Top;   // 图表：曲线区真正变大
            grpDevice.Height = splitY - 6 - grpDevice.Top;   // 设备状态框（内部下方留白）
            grpReg.Height = splitY - 6 - grpReg.Top;      // 寄存器框（内部下方留白）

            // 下块从分割线开始，到状态栏结束
            grpAlarm.Top = splitY - 8;
            grpAlarm.Height = bottomEdge - grpAlarm.Top;
            grpData.Top = splitY;
            grpData.Height = bottomEdge - grpData.Top;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();

            // ★ ClearData() 只清了 TextBox 上的文字，
            //   这几个计数器字段没归零的话，下一个采集周期又会把旧值写回界面，
            //   用户看起来就是"清空没生效"
            readCount = 0;
            alarmCount = 0;
            faultCount = 0;
            isFault = false;
            isPressureAlarm = false;

            // 曲线与报警日志一起复位
            sampleIndex = 0;
            receiveBuffer = "";
            lstAlarmLog.Items.Clear();
            foreach (Series s in chartMain.Series) s.Points.Clear();

            if (chartMain.ChartAreas.Count > 0)
            {
                chartMain.ChartAreas[0].AxisX.Minimum = 0;
                chartMain.ChartAreas[0].AxisX.Maximum = 10;
            }

            // 历史记录 historyList 刻意保留：统计清零 ≠ 历史数据消失，
            // 真实的生产系统里，班次统计可以清零，但历史数据必须可追溯。
            // ★ 状态栏只在未连接时才改写，保证界面与硬件真实状态一致
            //   （原代码无条件写成"设备未连接"，但串口其实还开着，用户会误判）
            txtStatus.Text = isConnected ? "数据已清空（设备仍在连接中）" : "设备未连接";
        }

        // =====================================================================
        //  十三、可选：随机数据模拟（原 ReadDeviceData，当前未被调用）
        //       需要"模拟采集"按钮时，在按钮里调用它即可；
        //       完全不需要的话，可以连同字段 random 一起删除。
        // =====================================================================
        private void ReadDeviceData()
        {
            readCount++;
            temperature = random.NextDouble() * 20 + 20;
            temperature = Math.Round(temperature, 1);
            pressure = random.NextDouble() * 0.5 + 0.5;
            pressure = Math.Round(pressure, 2);
        }

        // =====================================================================
        //  十四、保留的设计器占位事件
        //      这几个方法体是空的，但 Form1.Designer.cs 里已经绑定了它们，
        //      删掉会编译报错，别动。
        // =====================================================================
        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click_1(object sender, EventArgs e)
        {
        }

        private void labalarmCount_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void cmbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void txtSend_TextChanged(object sender, EventArgs e)
        {
        }

        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void grpDevice_Enter(object sender, EventArgs e)
        {

        }

        private void t_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click_2(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtDeviceId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

// 主窗体：串口（自定义协议 / Modbus RTU）+ SQLite 历史库
// 本文件只放 Form1 一个类 —— WinForms 设计器靠"第一个类型是窗体类"识别设计视图，
// 基础设施类已拆到 AppConfig.cs / Logger.cs / DbPath.cs / DbRecord.cs

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using EasyModbus;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private volatile bool isFault = false;
        private volatile bool isConnected = false;
        private volatile bool isPressureAlarm = false;  // 上一周期的报警状态，用于边沿触发
        private volatile bool isClosing = false;

        private int continuousFailCount = 0;
        private volatile string currentMode = AppConfig.ModeCustom;  // 后台线程要读，须 volatile

        private double temperature;
        private double pressure;
        private int readCount;
        private int alarmCount;
        private int faultCount;
        private int sampleIndex = 0;            // 曲线横轴用的采样序号
        private int discardedFrameCount = 0;    // 累计丢弃的非法帧

        private List<DeviceData> historyList = new List<DeviceData>();
        private EasyModbus.ModbusClient modbusClient = null;

        private readonly object modbusLock = new object();       // EasyModbus 非线程安全
        private readonly object serialWriteLock = new object();
        private readonly object rxLock = new object();           // 接收缓冲区

        private readonly StringBuilder rxBuffer = new StringBuilder();
        private int pollBusy = 0;                                // 采集重入哨兵，0 = 空闲

        // 模拟采集：没有下位机时用来跑通完整链路
        private volatile bool isSimulating = false;
        private readonly Random simulateRandom = new Random();

        // 数据库后台写入
        private BlockingCollection<DbRecord> dbQueue;
        private CancellationTokenSource dbCts;
        private Task dbWriterTask;
        private volatile bool dbAccepting = true;

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;

            dgvHistory.AutoGenerateColumns = false;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 先减后加：Designer.cs 里可能已绑定过，重复订阅会让一帧数据被处理两次
            serialPort1.DataReceived -= SerialPort1_DataReceived;
            serialPort1.DataReceived += SerialPort1_DataReceived;

            // 设计器会真的执行一遍构造函数，这里拦住建库/起线程，否则设计视图加载会卡或报错
            if (IsInDesignMode()) return;

            InitDatabase();
            StartDbWriter();
        }

        /// <summary>
        /// 构造函数阶段不能用 this.DesignMode —— 它依赖窗口句柄，此刻句柄未创建，恒为 false。
        /// </summary>
        private static bool IsInDesignMode()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return true;

            return string.Equals(System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                                 "devenv", StringComparison.OrdinalIgnoreCase);
        }

        // ---------- 连接 / 断开 ----------

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // 没插设备时 SelectedItem 为 null，直接 ToString 会抛 NullReferenceException
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
                if (mode == AppConfig.ModeModbus)
                {
                    EasyModbus.ModbusClient client = new EasyModbus.ModbusClient(portName);

                    client.Baudrate = baudRate;
                    client.Parity = System.IO.Ports.Parity.None;
                    client.StopBits = System.IO.Ports.StopBits.One;
                    client.UnitIdentifier = AppConfig.ModbusSlaveId;
                    client.ConnectionTimeout = AppConfig.ModbusConnectTimeoutMs;   // 设备离线时别把线程卡死

                    lock (modbusLock)   // 与采集线程互斥，避免半连接状态的实例被并发使用
                    {
                        client.Connect();
                        modbusClient = client;
                    }
                }
                else
                {
                    lock (serialWriteLock)
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
                }

                currentMode = mode;
                isConnected = true;
                isPressureAlarm = false;
                continuousFailCount = 0;
                lock (rxLock) { rxBuffer.Clear(); }

                // 连上真机就退出模拟
                if (isSimulating)
                {
                    isSimulating = false;
                    btnRead.Text = "读取数据";
                }

                txtStatus.Text = "设备已连接";
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnRead.Enabled = true;
                timerMonitor.Start();

                Logger.Info(string.Format("连接成功：{0} @ {1}bps，模式 {2}", portName, baudRate, mode));
            }
            catch (Exception ex)
            {
                // 失败必须回滚，否则残留一个 new 出来但没连上的 ModbusClient
                Logger.Error(ex, string.Format("连接 {0} 失败", portName));
                DisconnectDevice();
                MessageBox.Show("连接失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            DisconnectDevice();
        }

        /// <summary>
        /// 统一的断开收尾：手动断开和连续失败自动断开共用，避免两处收尾不一致。
        /// </summary>
        private void DisconnectDevice()
        {
            timerMonitor.Stop();
            isConnected = false;

            string errMsg = null;
            try
            {
                lock (modbusLock)
                {
                    if (modbusClient != null)
                    {
                        if (modbusClient.Connected)
                            modbusClient.Disconnect();
                        modbusClient = null;
                    }
                }

                lock (serialWriteLock)
                {
                    if (serialPort1.IsOpen)
                        serialPort1.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "关闭通信链路异常");
                errMsg = "关闭串口异常：" + ex.Message;   // 不弹窗打断用户，写状态栏
            }
            finally
            {
                isPressureAlarm = false;
                isFault = false;      // 不复位的话，下次连接一上来就是"设备故障"
                continuousFailCount = 0;

                if (isSimulating)
                {
                    isSimulating = false;
                    btnRead.Text = "读取数据";
                }

                readCount = 0;
                alarmCount = 0;
                faultCount = 0;

                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnRead.Enabled = true;   // 该按钮同时是模拟开关，未连接时也要能点

                ClearData();
                txtStatus.Text = errMsg ?? "设备已断开";
            }
        }

        /// <summary>
        /// "读取数据"按钮兼作模拟采集开关。
        /// 模拟数据走与真实采集相同的 ApplySample 通道，只是数据源换成随机数
        /// （压力区间会穿越报警线，便于演示报警与恢复）。
        /// </summary>
        private void btnRead_Click(object sender, EventArgs e)
        {
            if (!isConnected && !isSimulating)
            {
                // 没连设备也能开模拟，不碰串口
                isSimulating = true;
                timerMonitor.Start();   // 未连接时定时器是停的，这里拉起来
            }
            else if (isSimulating)
            {
                isSimulating = false;
                // 没连设备时顺手收掉定时器，状态栏就不会被下一轮 Tick 覆盖
                if (!isConnected) timerMonitor.Stop();
            }
            else
            {
                isSimulating = true;
            }

            btnRead.Text = isSimulating ? "停止模拟" : "读取数据";
            txtStatus.Text = isSimulating ? "模拟采集中…" : "模拟已停止";
            Logger.Info(isSimulating ? "开启模拟采集" : "停止模拟采集");
        }

        // ---------- 定时轮询（后台线程采集） ----------

        private async void timerMonitor_Tick(object sender, EventArgs e)
        {
            if (!isConnected && !isSimulating)
            {
                txtStatus.Text = "设备未连接";
                timerMonitor.Stop();
                return;
            }

            // 重入哨兵：上一轮还没返回就跳过，避免任务堆积
            if (Interlocked.CompareExchange(ref pollBusy, 1, 0) != 0)
            {
                Logger.Debug("上一轮采集尚未返回，跳过本轮");
                return;
            }

            try
            {
                await Task.Run(() => PollOnce());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "采集任务执行异常");
            }
            finally
            {
                Interlocked.Exchange(ref pollBusy, 0);
            }
        }

        /// <summary>后台线程执行的一次采集，不碰任何控件。</summary>
        private void PollOnce()
        {
            if (isSimulating)
            {
                double t = Math.Round(simulateRandom.NextDouble() * 20.0 + 20.0, 1);   // 20.0~40.0 ℃
                double p = Math.Round(simulateRandom.NextDouble() * 1.2 + 0.1, 2);    // 0.10~1.30 MPa

                SafeInvoke(() => ApplySample(t, p));
                return;
            }

            if (!isConnected) return;

            if (currentMode == AppConfig.ModeModbus)
                PollModbus();
            else
                PollCustomProtocol();
        }

        private void PollModbus()
        {
            try
            {
                int[] registers;

                // ModbusClient 非线程安全：采集线程在读、用户同时点"写寄存器"，
                // 两条报文会交错进同一条串口，读和写必须共用同一把锁
                lock (modbusLock)
                {
                    EasyModbus.ModbusClient client = modbusClient;
                    if (client == null || !client.Connected)
                        throw new InvalidOperationException("Modbus 链路已断开");

                    registers = client.ReadHoldingRegisters(AppConfig.ModbusReadStartAddress,
                                                            AppConfig.ModbusReadRegisterCount);
                }

                double t = registers[0] / AppConfig.TempScale;
                double p = registers[1] / AppConfig.PressureScale;

                SafeInvoke(() => ApplySample(t, p));
            }
            catch (Exception ex)
            {
                Logger.Warn("Modbus 读取失败：" + ex.Message);
                SafeInvoke(() => HandleCommFailure("读取失败：" + ex.Message));
            }
        }

        /// <summary>自定义协议：这里只发指令，数据由 DataReceived 事件接收。</summary>
        private void PollCustomProtocol()
        {
            try
            {
                lock (serialWriteLock)
                {
                    if (!isConnected) return;
                    if (!serialPort1.IsOpen)
                    {
                        SafeInvoke(() =>
                        {
                            if (!isConnected) return;
                            txtStatus.Text = "串口未打开";
                            timerMonitor.Stop();
                        });
                        return;
                    }

                    serialPort1.Write("DATA\n");
                }

                // 发送成功即清零失败计数；在 UI 线程改字段，避免与界面线程竞争
                SafeInvoke(() => { continuousFailCount = 0; });
            }
            catch (Exception ex)
            {
                Logger.Warn("指令发送失败：" + ex.Message);
                SafeInvoke(() => HandleCommFailure("发送失败：" + ex.Message));
            }
        }

        /// <summary>累计通信失败次数，达到上限自动断开。只能在 UI 线程调用。</summary>
        private void HandleCommFailure(string message)
        {
            if (isClosing) return;

            continuousFailCount++;
            txtStatus.Text = string.Format("{0}（连续失败 {1}/{2} 次）", message, continuousFailCount, AppConfig.MaxFailCount);

            if (continuousFailCount >= AppConfig.MaxFailCount)
            {
                Logger.Error(null, string.Format("通信连续失败 {0} 次，自动断开。最后一次错误：{1}", AppConfig.MaxFailCount, message));
                DisconnectDevice();
                MessageBox.Show(string.Format("通信连续失败 {0} 次，已自动断开。请检查设备供电与线路。", AppConfig.MaxFailCount),
                                "通信中断", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>一次采样的统一落点：Modbus 轮询和串口接收都走这里。只在 UI 线程执行。</summary>
        private void ApplySample(double t, double p)
        {
            if (isClosing) return;

            temperature = t;
            pressure = p;
            readCount++;
            continuousFailCount = 0;
            CheckAlarm();
            UpdateUI();
        }

        // ---------- 报警 ----------

        private void AddAlarmLog(string message)
        {
            lstAlarmLog.Items.Insert(0, string.Format("{0:HH:mm:ss} {1}", DateTime.Now, message));
            if (lstAlarmLog.Items.Count > AppConfig.MaxAlarmLogCount)
                lstAlarmLog.Items.RemoveAt(lstAlarmLog.Items.Count - 1);
        }

        /// <summary>
        /// 报警判定。用 isPressureAlarm 记录上一周期状态做边沿触发：
        /// 压力持续超标时只报一次警，而不是每个采集周期报一次。
        /// </summary>
        private void CheckAlarm()
        {
            if (isFault)
            {
                txtStatus.Text = "设备故障";
                return;   // 不更新 isPressureAlarm，故障恢复后不会被误判成新报警
            }

            bool alarmNow = pressure > AppConfig.PressureAlarmLimit;

            if (alarmNow)
            {
                txtStatus.Text = string.Format("压力报警（{0:F2} MPa）", pressure);

                if (!isPressureAlarm)   // 仅上升沿：计数、响铃、记日志
                {
                    alarmCount++;
                    System.Media.SystemSounds.Beep.Play();
                    AddAlarmLog(string.Format("压力报警 {0:F2} MPa", pressure));
                    Logger.Warn(string.Format("压力报警：{0:F2} MPa", pressure));
                }
            }
            else
            {
                txtStatus.Text = "设备正常";

                if (isPressureAlarm)    // 恢复也要留痕，便于追溯
                {
                    AddAlarmLog(string.Format("压力恢复正常 {0:F2} MPa", pressure));
                    Logger.Info(string.Format("压力恢复正常：{0:F2} MPa", pressure));
                }
            }

            isPressureAlarm = alarmNow;
        }

        private void btnFault_Click(object sender, EventArgs e)
        {
            if (isFault) return;   // 已故障不重复计数

            isFault = true;
            faultCount++;
            txtStatus.Text = "设备故障";
            AddAlarmLog("设备故障（模拟）");
            Logger.Warn("设备故障（模拟按钮触发）");
            UpdateUI();
        }

        private void btnRecover_Click(object sender, EventArgs e)
        {
            isFault = false;
            CheckAlarm();   // 边沿触发保证不会重复计数
            AddAlarmLog("设备故障已恢复");
            Logger.Info("设备故障已恢复");
            UpdateUI();
        }

        // ---------- 界面刷新（只在 UI 线程执行） ----------

        private void UpdateUI()
        {
            txtDeviceId.Text = AppConfig.DeviceId;
            txtDeviceName.Text = AppConfig.DeviceName;
            txtTemperature.Text = temperature.ToString("0.0");
            txtPressure.Text = pressure.ToString("0.00");
            txtTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            txtCount.Text = readCount.ToString();
            txtalarmCount.Text = alarmCount.ToString();
            txtFaultCount.Text = faultCount.ToString();

            bool isAlarmNow = pressure > AppConfig.PressureAlarmLimit;
            historyList.Add(new DeviceData(temperature, pressure, isAlarmNow, txtStatus.Text));
            if (historyList.Count > AppConfig.MaxHistoryCount)
                historyList.RemoveAt(0);

            // 落库只入队，磁盘 IO 交给后台写线程批量做
            EnqueueDbRecord(new DbRecord(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), temperature, pressure));

            Series sTemp = chartMain.Series.FindByName("温度");
            Series sPress = chartMain.Series.FindByName("压力");
            if (sTemp == null || sPress == null)
            {
                Logger.Warn("曲线系列未初始化（温度/压力），本次跳过绘图");
                return;
            }

            // X 轴用采样序号而非"当天秒数"，否则 24:00 归零会让曲线回折
            sampleIndex++;
            sTemp.Points.AddXY(sampleIndex, temperature);
            sPress.Points.AddXY(sampleIndex, pressure);   // 压力挂 AxisY2，无需 ×100

            while (sTemp.Points.Count > AppConfig.MaxChartPoints) sTemp.Points.RemoveAt(0);
            while (sPress.Points.Count > AppConfig.MaxChartPoints) sPress.Points.RemoveAt(0);

            if (chartMain.ChartAreas.Count > 0 && sTemp.Points.Count > 0)
            {
                chartMain.ChartAreas[0].AxisX.Minimum = sTemp.Points[0].XValue;
                chartMain.ChartAreas[0].AxisX.Maximum = sTemp.Points[sTemp.Points.Count - 1].XValue;
            }
        }

        // ---------- 数据库：初始化 + 后台写入线程 ----------

        private void InitDatabase()
        {
            try
            {
                const string sql = @"CREATE TABLE IF NOT EXISTS History (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        RecordTime TEXT,
                                        Temperature REAL,
                                        Pressure REAL
                                     );
                                     CREATE INDEX IF NOT EXISTS IX_History_RecordTime ON History(RecordTime DESC);";

                using (SQLiteConnection conn = DbPath.OpenConnection())
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                Logger.Info("数据库初始化完成：" + DbPath.File);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "数据库初始化失败");
                MessageBox.Show("数据库初始化失败，历史数据将无法保存：\n" + ex.Message,
                                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartDbWriter()
        {
            dbCts = new CancellationTokenSource();
            dbQueue = new BlockingCollection<DbRecord>(AppConfig.DbQueueCapacity);

            // LongRunning：长驻线程，不占用线程池的工作线程
            dbWriterTask = Task.Factory.StartNew(DbWriterLoop,
                                                 dbCts.Token,
                                                 TaskCreationOptions.LongRunning,
                                                 TaskScheduler.Default);
            Logger.Info("数据库写入线程已启动");
        }

        /// <summary>入队（生产者）。带超时，宁可丢一条采样也不阻塞界面。</summary>
        private void EnqueueDbRecord(DbRecord record)
        {
            if (!dbAccepting) return;

            try
            {
                if (!dbQueue.TryAdd(record, AppConfig.DbEnqueueTimeoutMs))
                {
                    Logger.Warn(string.Format("写库队列已满({0})，丢弃一条采样：{1}",
                                              AppConfig.DbQueueCapacity, record.RecordTime));
                }
            }
            catch (InvalidOperationException)
            {
                // 停机后再入队会走到这里，属正常收尾
                Logger.Debug("写库队列已关闭，丢弃一条采样");
            }
        }

        /// <summary>消费循环（消费者，后台线程）。停机时也要把队列排空。</summary>
        private void DbWriterLoop()
        {
            List<DbRecord> batch = new List<DbRecord>(AppConfig.DbBatchSize);

            try
            {
                // BlockingCollection<T>.IsEmpty 是显式接口实现，类实例访问不到，只能判 Count
                while (!dbCts.IsCancellationRequested || dbQueue.Count > 0)
                {
                    batch.Clear();

                    // 运行中最多等 500ms 凑批；停机中不等待，取空即退
                    int wait = dbCts.IsCancellationRequested ? 0 : AppConfig.DbFlushIntervalMs;

                    DbRecord first;
                    if (!dbQueue.TryTake(out first, wait))
                        continue;

                    batch.Add(first);

                    DbRecord more;
                    while (batch.Count < AppConfig.DbBatchSize && dbQueue.TryTake(out more, 0))
                        batch.Add(more);

                    FlushBatch(batch);
                }

                Logger.Info("数据库写入线程正常退出");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "数据库写入线程异常终止（后续采样将只留在内存队列外）");
            }
        }

        /// <summary>单批落库：一个连接、一个事务，参数对象复用。</summary>
        private void FlushBatch(List<DbRecord> batch)
        {
            if (batch == null || batch.Count == 0) return;

            try
            {
                using (SQLiteConnection conn = DbPath.OpenConnection())
                using (SQLiteTransaction tran = conn.BeginTransaction())
                {
                    const string sql = "INSERT INTO History (RecordTime, Temperature, Pressure) VALUES (@time, @t, @p)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn, tran))
                    {
                        SQLiteParameter pTime = cmd.Parameters.Add("@time", DbType.String);
                        SQLiteParameter pTemp = cmd.Parameters.Add("@t", DbType.Double);
                        SQLiteParameter pPress = cmd.Parameters.Add("@p", DbType.Double);

                        for (int i = 0; i < batch.Count; i++)
                        {
                            pTime.Value = batch[i].RecordTime;
                            pTemp.Value = batch[i].Temperature;
                            pPress.Value = batch[i].Pressure;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                // 写库失败意味着数据丢了，必须留痕并提示，不能空 catch 吞掉
                Logger.Error(ex, string.Format("批量写库失败，本批 {0} 条数据已丢弃", batch.Count));

                SafeInvoke(() =>
                {
                    if (!isClosing)
                        txtStatus.Text = "写数据库失败，请查看 logs 目录";
                });
            }
        }

        /// <summary>停机：停入队 → 唤醒消费者 → 等队列排空退出。</summary>
        private void ShutdownDbWriter()
        {
            dbAccepting = false;

            try
            {
                if (dbCts != null)
                    dbCts.Cancel();

                if (dbQueue != null && !dbQueue.IsAddingCompleted)
                    dbQueue.CompleteAdding();

                if (dbWriterTask != null && !dbWriterTask.Wait(AppConfig.DbShutdownTimeoutMs))
                    Logger.Warn("数据库写入线程未在超时时间内退出，可能仍有数据未落盘");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "关闭数据库写入线程异常");
            }
            finally
            {
                if (dbCts != null)
                {
                    dbCts.Dispose();
                    dbCts = null;
                }
            }
        }

        // ---------- 串口接收与报文解析（后台线程） ----------

        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // 本方法在后台线程执行，不要访问任何控件，只读 volatile 字段
            if (isClosing) return;
            if (currentMode == AppConfig.ModeModbus) return;

            string newData;
            try
            {
                if (!serialPort1.IsOpen) return;   // 串口可能正在关闭
                newData = serialPort1.ReadExisting();
            }
            catch (Exception ex)
            {
                // 关闭瞬间的读取失败属正常现象，但仍要留痕
                Logger.Debug("串口读取被忽略（可能是关闭瞬间）：" + ex.Message);
                return;
            }

            if (string.IsNullOrEmpty(newData)) return;

            // StringBuilder + 锁：避免高频拼接产生大量临时字符串，也与清空操作互斥
            List<string> lines = new List<string>(4);
            lock (rxLock)
            {
                rxBuffer.Append(newData);

                // 防下位机不发换行符导致缓冲区无限增长
                if (rxBuffer.Length > AppConfig.ReceiveBufferLimit)
                    rxBuffer.Remove(0, rxBuffer.Length - AppConfig.ReceiveBufferLimit / 2);

                while (true)
                {
                    int newLineIndex = IndexOfNewLine(rxBuffer);
                    if (newLineIndex < 0) break;

                    string line = rxBuffer.ToString(0, newLineIndex).TrimEnd('\r');
                    rxBuffer.Remove(0, newLineIndex + 1);
                    lines.Add(line);
                }
            }

            // 解析放到锁外面，别占着接收缓冲区
            for (int i = 0; i < lines.Count; i++)
                ProcessLine(lines[i]);
        }

        /// <summary>.NET Framework 的 StringBuilder 没有 IndexOf，只能手写。</summary>
        private static int IndexOfNewLine(StringBuilder sb)
        {
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == '\n') return i;
            }
            return -1;
        }

        private void ProcessLine(string line)
        {
            if (string.IsNullOrEmpty(line)) return;

            string[] parts = line.Split(',');
            if (parts.Length < 2) return;

            // 有第三列则做异或校验，不过就丢掉这一帧
            if (parts.Length >= 3)
            {
                byte xor = 0;
                string payload = parts[0] + "," + parts[1];
                foreach (char c in payload) xor ^= (byte)c;

                if (!xor.ToString("X2").Equals(parts[2].Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    // 每 100 帧报一次，避免刷屏
                    discardedFrameCount++;
                    if (discardedFrameCount % 100 == 0)
                        Logger.Warn(string.Format("校验失败，已累计丢弃 {0} 帧，最近一帧：{1}", discardedFrameCount, line));
                    return;
                }
            }

            // TryParse + InvariantCulture：脏数据不抛异常，小数点不受系统区域影响
            // （用 Convert.ToDouble 的话，德语/法语区域会把 23.5 读成 235）
            double t;
            double p;
            if (!double.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out t)) return;
            if (!double.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out p)) return;

            SafeInvoke(() => ApplySample(t, p));
        }

        /// <summary>
        /// 跨线程安全更新界面。关闭期间控件已销毁，直接 Invoke 会抛 ObjectDisposedException
        /// 把程序搞崩，所以先做存活判断，再用 BeginInvoke 异步投递（不阻塞串口接收线程）。
        /// </summary>
        private void SafeInvoke(Action action)
        {
            if (action == null) return;
            if (isClosing || this.IsDisposed || !this.IsHandleCreated) return;

            try
            {
                if (this.InvokeRequired)
                    this.BeginInvoke(action);
                else
                    action();
            }
            catch (ObjectDisposedException ex)
            {
                Logger.Debug("关闭期间丢弃一次界面更新（句柄已释放）：" + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Debug("关闭期间丢弃一次界面更新（句柄未创建/已失效）：" + ex.Message);
            }
        }

        // ---------- 手动发送 / Modbus 写寄存器 ----------

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
                lock (serialWriteLock)              // 与采集线程的发送互斥
                {
                    serialPort1.Write(cmd);
                }
                txtSend.Clear();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "手动发送失败");
                MessageBox.Show("发送失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnWriteReg_Click(object sender, EventArgs e)
        {
            if (cmbCommMode.SelectedItem == null || cmbCommMode.SelectedItem.ToString() != AppConfig.ModeModbus
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

            int address;
            int value;
            if (!int.TryParse(txtRegAddr.Text, out address))
            {
                MessageBox.Show("寄存器地址必须是整数。");
                return;
            }
            if (!int.TryParse(txtRegValue.Text, out value))
            {
                MessageBox.Show("寄存器值必须是整数。");
                return;
            }

            try
            {
                lock (modbusLock)   // 与采集线程的 ReadHoldingRegisters 互斥，必须加
                {
                    if (modbusClient == null || !modbusClient.Connected)
                        throw new InvalidOperationException("Modbus 链路已断开");

                    modbusClient.WriteSingleRegister(address, value);
                }

                AddAlarmLog(string.Format("写入寄存器[{0}] = {1}", address, value));
                Logger.Info(string.Format("写寄存器成功：[{0}] <- {1}", address, value));
                MessageBox.Show(string.Format("写入成功：寄存器[{0}] ← {1}", address, value));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("写寄存器失败：[{0}] <- {1}", address, value));
                MessageBox.Show("写入失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- 历史记录查看与导出 ----------

        private void btnShowHistory_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = DbPath.OpenConnection())
                {
                    const string sql = "SELECT RecordTime AS 采集时间, Temperature AS 温度, Pressure AS 压力 " +
                                       "FROM History ORDER BY Id DESC LIMIT 100";
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // 清掉 Designer 里预置的列，改用查询结果的列
                        dgvHistory.DataSource = null;
                        dgvHistory.Columns.Clear();
                        dgvHistory.AutoGenerateColumns = true;
                        dgvHistory.DataSource = dt;
                        dgvHistory.AllowUserToAddRows = false;
                        dgvHistory.AutoResizeColumns();

                        MessageBox.Show(string.Format("共 {0} 条记录", dt.Rows.Count));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "查询历史记录失败");
                MessageBox.Show("查询历史记录失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV文件|*.csv";
                saveDialog.FileName = string.Format("History_{0:yyyyMMdd_HHmmss}.csv", DateTime.Now);
                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                // UTF8 带 BOM，中文用 Excel 打开不会乱码
                using (StreamWriter sw = new StreamWriter(saveDialog.FileName, false, new UTF8Encoding(true)))
                {
                    sw.WriteLine("采集时间,温度(℃),压力(MPa)");

                    // 直接查库导出，不依赖内存列表
                    using (SQLiteConnection conn = DbPath.OpenConnection())
                    using (SQLiteCommand cmd = new SQLiteCommand(
                        "SELECT RecordTime, Temperature, Pressure FROM History ORDER BY Id DESC", conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        int count = 0;
                        while (reader.Read())
                        {
                            string time = reader["RecordTime"].ToString();
                            string temp = Convert.ToDouble(reader["Temperature"]).ToString("F1");
                            string press = Convert.ToDouble(reader["Pressure"]).ToString("F2");

                            sw.WriteLine(string.Format("{0},{1},{2}", time, temp, press));
                            count++;
                        }

                        Logger.Info(string.Format("导出 CSV 成功，共 {0} 条：{1}", count, saveDialog.FileName));
                        MessageBox.Show(string.Format("导出成功！共 {0} 条记录。", count), "提示");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "导出 CSV 失败");
                MessageBox.Show("导出失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- 窗体初始化 ----------

        private void Form1_Load(object sender, EventArgs e)
        {
            // 串口列表：优先选中 COM1
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

            cmbBaudRate.Items.Clear();
            cmbBaudRate.Items.AddRange(new object[] { 9600, 19200, 38400, 57600, 115200 });
            cmbBaudRate.SelectedIndex = 0;

            cmbCommMode.Items.Clear();
            cmbCommMode.Items.AddRange(new object[] { AppConfig.ModeCustom, AppConfig.ModeModbus });
            cmbCommMode.SelectedIndex = 0;
            currentMode = AppConfig.ModeCustom;

            timerMonitor.Interval = AppConfig.PollIntervalMs;

            // 图表
            chartMain.Series.Clear();
            chartMain.ChartAreas.Clear();

            ChartArea area = new ChartArea("Area1");
            area.AxisX.Title = "采样点";
            area.AxisX.MajorGrid.LineColor = Color.LightGray;

            // 主轴：温度
            area.AxisY.Title = "温度 (℃)";
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 40;
            area.AxisY.Interval = 10;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            // 次轴：压力单独成轴，不再 ×100 与温度挤在一起
            area.AxisY2.Title = "压力 (MPa)";
            area.AxisY2.Minimum = 0;
            area.AxisY2.Maximum = 1.5;
            area.AxisY2.Interval = 0.3;
            area.AxisY2.Enabled = AxisEnabled.True;
            area.AxisY2.MajorGrid.Enabled = false;
            // 固定绘图区比例，最大化时防止 X 轴刻度被挤掉
            area.Position = new ElementPosition(2, 3, 96, 92);
            area.InnerPlotPosition = new ElementPosition(10, 3, 82, 80);

            chartMain.ChartAreas.Add(area);

            Series seriesTemp = new Series("温度");
            seriesTemp.ChartType = SeriesChartType.Line;
            seriesTemp.BorderWidth = 2;
            seriesTemp.Color = Color.Red;
            seriesTemp.YAxisType = AxisType.Primary;
            chartMain.Series.Add(seriesTemp);

            Series seriesPress = new Series("压力");
            seriesPress.ChartType = SeriesChartType.Line;
            seriesPress.BorderWidth = 2;
            seriesPress.Color = Color.Blue;
            seriesPress.YAxisType = AxisType.Secondary;
            chartMain.Series.Add(seriesPress);

            this.Resize += (s, ev) => ApplyAdaptiveLayout();
            ApplyAdaptiveLayout();
        }

        // ---------- 自适应分割：最大化时把分割线移到 60% ----------

        private void ApplyAdaptiveLayout()
        {
            int bottomEdge = ClientSize.Height - statusStrip1.Height - 6;
            int splitY = Math.Max(446, (int)(bottomEdge * 0.60));

            grpChart.Height = splitY - 14 - grpChart.Top;
            grpDevice.Height = splitY - 6 - grpDevice.Top;
            grpReg.Height = splitY - 6 - grpReg.Top;

            grpAlarm.Top = splitY - 8;
            grpAlarm.Height = bottomEdge - grpAlarm.Top;
            grpData.Top = splitY;
            grpData.Height = bottomEdge - grpData.Top;
        }

        // ---------- 清空 ----------

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();

            // ClearData 只清界面文字，计数器字段也要归零，
            // 否则下一个采集周期又把旧值写回界面，看起来就是"清空没生效"
            readCount = 0;
            alarmCount = 0;
            faultCount = 0;
            isFault = false;
            isPressureAlarm = false;

            sampleIndex = 0;
            lock (rxLock) { rxBuffer.Clear(); }
            lstAlarmLog.Items.Clear();
            foreach (Series s in chartMain.Series) s.Points.Clear();

            if (chartMain.ChartAreas.Count > 0)
            {
                chartMain.ChartAreas[0].AxisX.Minimum = 0;
                chartMain.ChartAreas[0].AxisX.Maximum = 10;
            }

            // historyList 刻意保留：统计清零 ≠ 历史数据消失
            // 状态栏按真实连接状态改写，不然串口还开着却显示"设备未连接"
            txtStatus.Text = isConnected ? "数据已清空（设备仍在连接中）" : "设备未连接";
        }

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

        // ---------- 停机清理 ----------

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;

            try
            {
                timerMonitor.Stop();                                    // 1 停止新数据
                DisconnectDevice();                                     // 2 断链路（内部加锁，等于等采集返回）
                serialPort1.DataReceived -= SerialPort1_DataReceived;    // 3 摘事件订阅
                ShutdownDbWriter();                                     // 4 排空写库队列，保证最后几条落盘

                Logger.Info("程序关闭清理完成");
            }
            catch (Exception ex)
            {
                // 关闭阶段只记日志，不弹窗阻塞用户退出
                Logger.Error(ex, "程序关闭清理异常");
            }
        }
    }
}

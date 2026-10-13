namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDeviceId = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.txtTemperature = new System.Windows.Forms.TextBox();
            this.txtPressure = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.timerMonitor = new System.Windows.Forms.Timer(this.components);
            this.lblTime = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.labCount = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.labalarmCount = new System.Windows.Forms.Label();
            this.txtalarmCount = new System.Windows.Forms.TextBox();
            this.t = new System.Windows.Forms.Label();
            this.txtFaultCount = new System.Windows.Forms.TextBox();
            this.btnFault = new System.Windows.Forms.Button();
            this.btnRecover = new System.Windows.Forms.Button();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.cmbMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTemp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnShowHistory = new System.Windows.Forms.Button();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.lblPort = new System.Windows.Forms.Label();
            this.cmbPortName = new System.Windows.Forms.ComboBox();
            this.lblBaud = new System.Windows.Forms.Label();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.cmbCommMode = new System.Windows.Forms.ComboBox();
            this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lstAlarmLog = new System.Windows.Forms.ListBox();
            this.txtRegAddr = new System.Windows.Forms.TextBox();
            this.txtRegValue = new System.Windows.Forms.TextBox();
            this.btnWriteReg = new System.Windows.Forms.Button();
            this.lblRegAddr = new System.Windows.Forms.Label();
            this.lblRegValue = new System.Windows.Forms.Label();
            this.grpDevice = new System.Windows.Forms.GroupBox();
            this.grpComm = new System.Windows.Forms.GroupBox();
            this.grpAction = new System.Windows.Forms.GroupBox();
            this.grpReg = new System.Windows.Forms.GroupBox();
            this.grpChart = new System.Windows.Forms.GroupBox();
            this.grpAlarm = new System.Windows.Forms.GroupBox();
            this.grpData = new System.Windows.Forms.GroupBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).BeginInit();
            this.grpDevice.SuspendLayout();
            this.grpComm.SuspendLayout();
            this.grpAction.SuspendLayout();
            this.grpReg.SuspendLayout();
            this.grpChart.SuspendLayout();
            this.grpAlarm.SuspendLayout();
            this.grpData.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备编号";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtDeviceId
            // 
            this.txtDeviceId.Location = new System.Drawing.Point(88, 26);
            this.txtDeviceId.Name = "txtDeviceId";
            this.txtDeviceId.Size = new System.Drawing.Size(120, 21);
            this.txtDeviceId.TabIndex = 1;
            this.txtDeviceId.TextChanged += new System.EventHandler(this.txtDeviceId_TextChanged);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(6, 75);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "连接设备";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "设备名称";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "设备温度";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 21);
            this.label4.TabIndex = 5;
            this.label4.Text = "设备压力";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 21);
            this.label5.TabIndex = 6;
            this.label5.Text = "连接状态";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.Location = new System.Drawing.Point(88, 57);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Size = new System.Drawing.Size(120, 21);
            this.txtDeviceName.TabIndex = 7;
            // 
            // txtTemperature
            // 
            this.txtTemperature.Location = new System.Drawing.Point(88, 88);
            this.txtTemperature.Name = "txtTemperature";
            this.txtTemperature.Size = new System.Drawing.Size(120, 21);
            this.txtTemperature.TabIndex = 8;
            // 
            // txtPressure
            // 
            this.txtPressure.Location = new System.Drawing.Point(88, 119);
            this.txtPressure.Name = "txtPressure";
            this.txtPressure.Size = new System.Drawing.Size(120, 21);
            this.txtPressure.TabIndex = 9;
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(88, 150);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(120, 21);
            this.txtStatus.TabIndex = 10;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(8, 20);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 11;
            this.btnRead.Text = "读取数据";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(99, 75);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 12;
            this.btnDisconnect.Text = "断开设备";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(99, 20);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "清空数据";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // timerMonitor
            // 
            this.timerMonitor.Interval = 1000;
            this.timerMonitor.Tick += new System.EventHandler(this.timerMonitor_Tick);
            // 
            // lblTime
            // 
            this.lblTime.Location = new System.Drawing.Point(12, 181);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(70, 21);
            this.lblTime.TabIndex = 14;
            this.lblTime.Text = "当前时间";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTime.Click += new System.EventHandler(this.label6_Click);
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(88, 181);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(120, 21);
            this.txtTime.TabIndex = 15;
            // 
            // labCount
            // 
            this.labCount.Location = new System.Drawing.Point(12, 212);
            this.labCount.Name = "labCount";
            this.labCount.Size = new System.Drawing.Size(70, 21);
            this.labCount.TabIndex = 16;
            this.labCount.Text = "读取次数";
            this.labCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labCount.Click += new System.EventHandler(this.label6_Click_1);
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(88, 212);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(120, 21);
            this.txtCount.TabIndex = 17;
            // 
            // labalarmCount
            // 
            this.labalarmCount.Location = new System.Drawing.Point(12, 243);
            this.labalarmCount.Name = "labalarmCount";
            this.labalarmCount.Size = new System.Drawing.Size(70, 21);
            this.labalarmCount.TabIndex = 18;
            this.labalarmCount.Text = "报警次数";
            this.labalarmCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labalarmCount.Click += new System.EventHandler(this.labalarmCount_Click);
            // 
            // txtalarmCount
            // 
            this.txtalarmCount.Location = new System.Drawing.Point(88, 243);
            this.txtalarmCount.Name = "txtalarmCount";
            this.txtalarmCount.Size = new System.Drawing.Size(120, 21);
            this.txtalarmCount.TabIndex = 19;
            // 
            // t
            // 
            this.t.Location = new System.Drawing.Point(12, 277);
            this.t.Name = "t";
            this.t.Size = new System.Drawing.Size(70, 21);
            this.t.TabIndex = 21;
            this.t.Text = "故障次数";
            this.t.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.t.Click += new System.EventHandler(this.t_Click);
            // 
            // txtFaultCount
            // 
            this.txtFaultCount.Location = new System.Drawing.Point(88, 274);
            this.txtFaultCount.Name = "txtFaultCount";
            this.txtFaultCount.Size = new System.Drawing.Size(120, 21);
            this.txtFaultCount.TabIndex = 22;
            this.txtFaultCount.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnFault
            // 
            this.btnFault.Location = new System.Drawing.Point(6, 53);
            this.btnFault.Name = "btnFault";
            this.btnFault.Size = new System.Drawing.Size(75, 23);
            this.btnFault.TabIndex = 23;
            this.btnFault.Text = "模拟故障";
            this.btnFault.UseVisualStyleBackColor = true;
            this.btnFault.Click += new System.EventHandler(this.btnFault_Click);
            // 
            // btnRecover
            // 
            this.btnRecover.Location = new System.Drawing.Point(99, 53);
            this.btnRecover.Name = "btnRecover";
            this.btnRecover.Size = new System.Drawing.Size(75, 23);
            this.btnRecover.TabIndex = 24;
            this.btnRecover.Text = "恢复故障";
            this.btnRecover.UseVisualStyleBackColor = true;
            this.btnRecover.Click += new System.EventHandler(this.btnRecover_Click);
            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(251)))));
            this.dgvHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHistory.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmbMode,
            this.colTemp,
            this.colPress,
            this.colStatus});
            this.dgvHistory.Location = new System.Drawing.Point(6, 20);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvHistory.RowTemplate.Height = 23;
            this.dgvHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistory.Size = new System.Drawing.Size(768, 192);
            this.dgvHistory.TabIndex = 25;
            this.dgvHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistory_CellContentClick);
            // 
            // cmbMode
            // 
            this.cmbMode.DataPropertyName = "ReadTime";
            this.cmbMode.HeaderText = "采集时间";
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.ReadOnly = true;
            // 
            // colTemp
            // 
            this.colTemp.DataPropertyName = "Temperature";
            this.colTemp.HeaderText = "温度（℃）";
            this.colTemp.Name = "colTemp";
            this.colTemp.ReadOnly = true;
            // 
            // colPress
            // 
            this.colPress.DataPropertyName = "Pressure";
            this.colPress.HeaderText = "压力（MPa）";
            this.colPress.Name = "colPress";
            this.colPress.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "状态";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // btnShowHistory
            // 
            this.btnShowHistory.Location = new System.Drawing.Point(8, 82);
            this.btnShowHistory.Name = "btnShowHistory";
            this.btnShowHistory.Size = new System.Drawing.Size(75, 23);
            this.btnShowHistory.TabIndex = 26;
            this.btnShowHistory.Text = "显示历史";
            this.btnShowHistory.UseVisualStyleBackColor = true;
            this.btnShowHistory.Click += new System.EventHandler(this.btnShowHistory_Click);
            // 
            // btnExportCsv
            // 
            this.btnExportCsv.Location = new System.Drawing.Point(99, 82);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(75, 23);
            this.btnExportCsv.TabIndex = 27;
            this.btnExportCsv.Text = "导出CSV";
            this.btnExportCsv.UseVisualStyleBackColor = true;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(6, 23);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(41, 12);
            this.lblPort.TabIndex = 28;
            this.lblPort.Text = "串  口";
            // 
            // cmbPortName
            // 
            this.cmbPortName.FormattingEnabled = true;
            this.cmbPortName.Location = new System.Drawing.Point(53, 15);
            this.cmbPortName.Name = "cmbPortName";
            this.cmbPortName.Size = new System.Drawing.Size(120, 20);
            this.cmbPortName.TabIndex = 29;
            this.cmbPortName.SelectedIndexChanged += new System.EventHandler(this.cmbPortName_SelectedIndexChanged);
            // 
            // lblBaud
            // 
            this.lblBaud.AutoSize = true;
            this.lblBaud.Location = new System.Drawing.Point(6, 52);
            this.lblBaud.Name = "lblBaud";
            this.lblBaud.Size = new System.Drawing.Size(41, 12);
            this.lblBaud.TabIndex = 30;
            this.lblBaud.Text = "波特率";
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Location = new System.Drawing.Point(53, 49);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(121, 20);
            this.cmbBaudRate.TabIndex = 31;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(8, 111);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 32;
            this.btnSend.Text = "发    送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(99, 113);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(75, 21);
            this.txtSend.TabIndex = 33;
            this.txtSend.TextChanged += new System.EventHandler(this.txtSend_TextChanged);
            // 
            // cmbCommMode
            // 
            this.cmbCommMode.FormattingEnabled = true;
            this.cmbCommMode.Location = new System.Drawing.Point(79, 85);
            this.cmbCommMode.Name = "cmbCommMode";
            this.cmbCommMode.Size = new System.Drawing.Size(120, 20);
            this.cmbCommMode.TabIndex = 34;
            this.cmbCommMode.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // chartMain
            // 
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 100F;
            chartArea1.InnerPlotPosition.Width = 100F;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 88F;
            chartArea1.Position.Width = 94F;
            chartArea1.Position.X = 3F;
            chartArea1.Position.Y = 3F;
            this.chartMain.ChartAreas.Add(chartArea1);
            this.chartMain.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartMain.Legends.Add(legend1);
            this.chartMain.Location = new System.Drawing.Point(3, 17);
            this.chartMain.Name = "chartMain";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartMain.Series.Add(series1);
            this.chartMain.Size = new System.Drawing.Size(547, 509);
            this.chartMain.TabIndex = 35;
            this.chartMain.Text = "chart1";
            title1.Name = "实时数据趋势";
            this.chartMain.Titles.Add(title1);
            // 
            // lstAlarmLog
            // 
            this.lstAlarmLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAlarmLog.FormattingEnabled = true;
            this.lstAlarmLog.ItemHeight = 12;
            this.lstAlarmLog.Location = new System.Drawing.Point(3, 17);
            this.lstAlarmLog.Name = "lstAlarmLog";
            this.lstAlarmLog.Size = new System.Drawing.Size(270, 207);
            this.lstAlarmLog.TabIndex = 36;
            // 
            // txtRegAddr
            // 
            this.txtRegAddr.Location = new System.Drawing.Point(79, 20);
            this.txtRegAddr.Name = "txtRegAddr";
            this.txtRegAddr.Size = new System.Drawing.Size(120, 21);
            this.txtRegAddr.TabIndex = 37;
            // 
            // txtRegValue
            // 
            this.txtRegValue.Location = new System.Drawing.Point(79, 47);
            this.txtRegValue.Name = "txtRegValue";
            this.txtRegValue.Size = new System.Drawing.Size(120, 21);
            this.txtRegValue.TabIndex = 38;
            // 
            // btnWriteReg
            // 
            this.btnWriteReg.Location = new System.Drawing.Point(0, 83);
            this.btnWriteReg.Name = "btnWriteReg";
            this.btnWriteReg.Size = new System.Drawing.Size(75, 23);
            this.btnWriteReg.TabIndex = 39;
            this.btnWriteReg.Text = "写入寄存器";
            this.btnWriteReg.UseVisualStyleBackColor = true;
            this.btnWriteReg.Click += new System.EventHandler(this.btnWriteReg_Click);
            // 
            // lblRegAddr
            // 
            this.lblRegAddr.AutoSize = true;
            this.lblRegAddr.Location = new System.Drawing.Point(8, 26);
            this.lblRegAddr.Name = "lblRegAddr";
            this.lblRegAddr.Size = new System.Drawing.Size(65, 12);
            this.lblRegAddr.TabIndex = 40;
            this.lblRegAddr.Text = "寄存器地址";
            // 
            // lblRegValue
            // 
            this.lblRegValue.AutoSize = true;
            this.lblRegValue.Location = new System.Drawing.Point(8, 54);
            this.lblRegValue.Name = "lblRegValue";
            this.lblRegValue.Size = new System.Drawing.Size(53, 12);
            this.lblRegValue.TabIndex = 41;
            this.lblRegValue.Text = "寄存器值";
            // 
            // grpDevice
            // 
            this.grpDevice.Controls.Add(this.label1);
            this.grpDevice.Controls.Add(this.txtDeviceId);
            this.grpDevice.Controls.Add(this.label2);
            this.grpDevice.Controls.Add(this.txtDeviceName);
            this.grpDevice.Controls.Add(this.label3);
            this.grpDevice.Controls.Add(this.txtTemperature);
            this.grpDevice.Controls.Add(this.label4);
            this.grpDevice.Controls.Add(this.txtPressure);
            this.grpDevice.Controls.Add(this.label5);
            this.grpDevice.Controls.Add(this.txtStatus);
            this.grpDevice.Controls.Add(this.lblTime);
            this.grpDevice.Controls.Add(this.txtTime);
            this.grpDevice.Controls.Add(this.labCount);
            this.grpDevice.Controls.Add(this.txtCount);
            this.grpDevice.Controls.Add(this.labalarmCount);
            this.grpDevice.Controls.Add(this.txtalarmCount);
            this.grpDevice.Controls.Add(this.t);
            this.grpDevice.Controls.Add(this.txtFaultCount);
            this.grpDevice.Location = new System.Drawing.Point(14, 41);
            this.grpDevice.Name = "grpDevice";
            this.grpDevice.Size = new System.Drawing.Size(265, 391);
            this.grpDevice.TabIndex = 42;
            this.grpDevice.TabStop = false;
            this.grpDevice.Text = "设备状态";
            this.grpDevice.Enter += new System.EventHandler(this.grpDevice_Enter);
            // 
            // grpComm
            // 
            this.grpComm.Controls.Add(this.lblPort);
            this.grpComm.Controls.Add(this.cmbPortName);
            this.grpComm.Controls.Add(this.lblBaud);
            this.grpComm.Controls.Add(this.cmbBaudRate);
            this.grpComm.Controls.Add(this.btnConnect);
            this.grpComm.Controls.Add(this.btnDisconnect);
            this.grpComm.Location = new System.Drawing.Point(296, 41);
            this.grpComm.Name = "grpComm";
            this.grpComm.Size = new System.Drawing.Size(229, 110);
            this.grpComm.TabIndex = 43;
            this.grpComm.TabStop = false;
            this.grpComm.Text = "通信配置";
            // 
            // grpAction
            // 
            this.grpAction.Controls.Add(this.btnRead);
            this.grpAction.Controls.Add(this.btnSend);
            this.grpAction.Controls.Add(this.txtSend);
            this.grpAction.Controls.Add(this.btnClear);
            this.grpAction.Controls.Add(this.btnFault);
            this.grpAction.Controls.Add(this.btnRecover);
            this.grpAction.Controls.Add(this.btnShowHistory);
            this.grpAction.Controls.Add(this.btnExportCsv);
            this.grpAction.Location = new System.Drawing.Point(296, 162);
            this.grpAction.Name = "grpAction";
            this.grpAction.Size = new System.Drawing.Size(229, 150);
            this.grpAction.TabIndex = 44;
            this.grpAction.TabStop = false;
            this.grpAction.Text = "操作按钮";
            // 
            // grpReg
            // 
            this.grpReg.Controls.Add(this.btnWriteReg);
            this.grpReg.Controls.Add(this.lblRegAddr);
            this.grpReg.Controls.Add(this.lblRegValue);
            this.grpReg.Controls.Add(this.txtRegAddr);
            this.grpReg.Controls.Add(this.txtRegValue);
            this.grpReg.Controls.Add(this.cmbCommMode);
            this.grpReg.Location = new System.Drawing.Point(296, 318);
            this.grpReg.Name = "grpReg";
            this.grpReg.Size = new System.Drawing.Size(229, 114);
            this.grpReg.TabIndex = 45;
            this.grpReg.TabStop = false;
            this.grpReg.Text = "寄存器";
            // 
            // grpChart
            // 
            this.grpChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpChart.Controls.Add(this.chartMain);
            this.grpChart.Location = new System.Drawing.Point(531, 41);
            this.grpChart.Name = "grpChart";
            this.grpChart.Size = new System.Drawing.Size(553, 529);
            this.grpChart.TabIndex = 46;
            this.grpChart.TabStop = false;
            this.grpChart.Text = "实时趋势";
            // 
            // grpAlarm
            // 
            this.grpAlarm.Controls.Add(this.lstAlarmLog);
            this.grpAlarm.Location = new System.Drawing.Point(14, 438);
            this.grpAlarm.Name = "grpAlarm";
            this.grpAlarm.Size = new System.Drawing.Size(276, 227);
            this.grpAlarm.TabIndex = 47;
            this.grpAlarm.TabStop = false;
            this.grpAlarm.Text = "报警日志";
            // 
            // grpData
            // 
            this.grpData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpData.Controls.Add(this.dgvHistory);
            this.grpData.Location = new System.Drawing.Point(296, 446);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(788, 219);
            this.grpData.TabIndex = 48;
            this.grpData.TabStop = false;
            this.grpData.Text = "数据历史";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(106, 22);
            this.lblTitle.TabIndex = 49;
            this.lblTitle.Text = "实时数据监控";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Click += new System.EventHandler(this.label6_Click_2);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 665);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1127, 22);
            this.statusStrip1.TabIndex = 50;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel1.Text = "就绪 ";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(90)))), ((int)(((byte)(140)))));
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1127, 21);
            this.panel1.TabIndex = 51;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1127, 687);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.grpData);
            this.Controls.Add(this.grpAlarm);
            this.Controls.Add(this.grpChart);
            this.Controls.Add(this.grpReg);
            this.Controls.Add(this.grpAction);
            this.Controls.Add(this.grpComm);
            this.Controls.Add(this.grpDevice);
            this.Name = "Form1";
            this.Text = "数据采集工具 v1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).EndInit();
            this.grpDevice.ResumeLayout(false);
            this.grpDevice.PerformLayout();
            this.grpComm.ResumeLayout(false);
            this.grpComm.PerformLayout();
            this.grpAction.ResumeLayout(false);
            this.grpAction.PerformLayout();
            this.grpReg.ResumeLayout(false);
            this.grpReg.PerformLayout();
            this.grpChart.ResumeLayout(false);
            this.grpAlarm.ResumeLayout(false);
            this.grpData.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDeviceId;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.TextBox txtTemperature;
        private System.Windows.Forms.TextBox txtPressure;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnClear;
        internal System.Windows.Forms.Timer timerMonitor;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label labCount;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Label labalarmCount;
        private System.Windows.Forms.TextBox txtalarmCount;
        private System.Windows.Forms.Label t;
        private System.Windows.Forms.TextBox txtFaultCount;
        private System.Windows.Forms.Button btnFault;
        private System.Windows.Forms.Button btnRecover;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmbMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTemp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.Button btnShowHistory;
        private System.Windows.Forms.Button btnExportCsv;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.ComboBox cmbPortName;
        private System.Windows.Forms.Label lblBaud;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.ComboBox cmbCommMode;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMain;
        private System.Windows.Forms.ListBox lstAlarmLog;
        private System.Windows.Forms.TextBox txtRegAddr;
        private System.Windows.Forms.TextBox txtRegValue;
        private System.Windows.Forms.Button btnWriteReg;
        private System.Windows.Forms.Label lblRegAddr;
        private System.Windows.Forms.Label lblRegValue;
        private System.Windows.Forms.GroupBox grpDevice;
        private System.Windows.Forms.GroupBox grpComm;
        private System.Windows.Forms.GroupBox grpAction;
        private System.Windows.Forms.GroupBox grpReg;
        private System.Windows.Forms.GroupBox grpChart;
        private System.Windows.Forms.GroupBox grpAlarm;
        private System.Windows.Forms.GroupBox grpData;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
    }
}


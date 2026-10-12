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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.label6 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备编号";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtDeviceId
            // 
            this.txtDeviceId.Location = new System.Drawing.Point(130, 32);
            this.txtDeviceId.Name = "txtDeviceId";
            this.txtDeviceId.Size = new System.Drawing.Size(100, 21);
            this.txtDeviceId.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(322, 32);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "连接设备";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "设备名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "设备温度";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "设备压力";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 171);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "连接状态";
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.Location = new System.Drawing.Point(130, 66);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Size = new System.Drawing.Size(100, 21);
            this.txtDeviceName.TabIndex = 7;
            // 
            // txtTemperature
            // 
            this.txtTemperature.Location = new System.Drawing.Point(130, 95);
            this.txtTemperature.Name = "txtTemperature";
            this.txtTemperature.Size = new System.Drawing.Size(100, 21);
            this.txtTemperature.TabIndex = 8;
            // 
            // txtPressure
            // 
            this.txtPressure.Location = new System.Drawing.Point(130, 129);
            this.txtPressure.Name = "txtPressure";
            this.txtPressure.Size = new System.Drawing.Size(100, 21);
            this.txtPressure.TabIndex = 9;
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(130, 162);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(100, 21);
            this.txtStatus.TabIndex = 10;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(432, 30);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 11;
            this.btnRead.Text = "读取数据";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(322, 200);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 12;
            this.btnDisconnect.Text = "断开设备";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(432, 200);
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
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(38, 205);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(53, 12);
            this.lblTime.TabIndex = 14;
            this.lblTime.Text = "当前时间";
            this.lblTime.Click += new System.EventHandler(this.label6_Click);
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(130, 196);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(100, 21);
            this.txtTime.TabIndex = 15;
            // 
            // labCount
            // 
            this.labCount.AutoSize = true;
            this.labCount.Location = new System.Drawing.Point(38, 240);
            this.labCount.Name = "labCount";
            this.labCount.Size = new System.Drawing.Size(53, 12);
            this.labCount.TabIndex = 16;
            this.labCount.Text = "读取次数";
            this.labCount.Click += new System.EventHandler(this.label6_Click_1);
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(130, 231);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(100, 21);
            this.txtCount.TabIndex = 17;
            // 
            // labalarmCount
            // 
            this.labalarmCount.AutoSize = true;
            this.labalarmCount.Location = new System.Drawing.Point(38, 281);
            this.labalarmCount.Name = "labalarmCount";
            this.labalarmCount.Size = new System.Drawing.Size(53, 12);
            this.labalarmCount.TabIndex = 18;
            this.labalarmCount.Text = "报警次数";
            this.labalarmCount.Click += new System.EventHandler(this.labalarmCount_Click);
            // 
            // txtalarmCount
            // 
            this.txtalarmCount.Location = new System.Drawing.Point(130, 272);
            this.txtalarmCount.Name = "txtalarmCount";
            this.txtalarmCount.Size = new System.Drawing.Size(100, 21);
            this.txtalarmCount.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "label6";
            // 
            // t
            // 
            this.t.AutoSize = true;
            this.t.Location = new System.Drawing.Point(38, 316);
            this.t.Name = "t";
            this.t.Size = new System.Drawing.Size(53, 12);
            this.t.TabIndex = 21;
            this.t.Text = "故障次数";
            // 
            // txtFaultCount
            // 
            this.txtFaultCount.Location = new System.Drawing.Point(130, 313);
            this.txtFaultCount.Name = "txtFaultCount";
            this.txtFaultCount.Size = new System.Drawing.Size(100, 21);
            this.txtFaultCount.TabIndex = 22;
            this.txtFaultCount.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnFault
            // 
            this.btnFault.Location = new System.Drawing.Point(322, 256);
            this.btnFault.Name = "btnFault";
            this.btnFault.Size = new System.Drawing.Size(75, 23);
            this.btnFault.TabIndex = 23;
            this.btnFault.Text = "模拟故障";
            this.btnFault.UseVisualStyleBackColor = true;
            this.btnFault.Click += new System.EventHandler(this.btnFault_Click);
            // 
            // btnRecover
            // 
            this.btnRecover.Location = new System.Drawing.Point(432, 256);
            this.btnRecover.Name = "btnRecover";
            this.btnRecover.Size = new System.Drawing.Size(75, 23);
            this.btnRecover.TabIndex = 24;
            this.btnRecover.Text = "恢复故障";
            this.btnRecover.UseVisualStyleBackColor = true;
            this.btnRecover.Click += new System.EventHandler(this.btnRecover_Click);
            // 
            // dgvHistory
            // 
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmbMode,
            this.colTemp,
            this.colPress,
            this.colStatus});
            this.dgvHistory.Location = new System.Drawing.Point(24, 479);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.RowTemplate.Height = 23;
            this.dgvHistory.Size = new System.Drawing.Size(439, 53);
            this.dgvHistory.TabIndex = 25;
            this.dgvHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistory_CellContentClick);
            // 
            // cmbMode
            // 
            this.cmbMode.DataPropertyName = "ReadTime";
            this.cmbMode.HeaderText = "采集时间";
            this.cmbMode.Name = "cmbMode";
            // 
            // colTemp
            // 
            this.colTemp.DataPropertyName = "Temperature";
            this.colTemp.HeaderText = "温度（℃）";
            this.colTemp.Name = "colTemp";
            // 
            // colPress
            // 
            this.colPress.DataPropertyName = "Pressure";
            this.colPress.HeaderText = "压力（MPa）";
            this.colPress.Name = "colPress";
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "状态";
            this.colStatus.Name = "colStatus";
            // 
            // btnShowHistory
            // 
            this.btnShowHistory.Location = new System.Drawing.Point(322, 305);
            this.btnShowHistory.Name = "btnShowHistory";
            this.btnShowHistory.Size = new System.Drawing.Size(75, 23);
            this.btnShowHistory.TabIndex = 26;
            this.btnShowHistory.Text = "显示历史";
            this.btnShowHistory.UseVisualStyleBackColor = true;
            this.btnShowHistory.Click += new System.EventHandler(this.btnShowHistory_Click);
            // 
            // btnExportCsv
            // 
            this.btnExportCsv.Location = new System.Drawing.Point(432, 305);
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
            this.lblPort.Location = new System.Drawing.Point(337, 78);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 12);
            this.lblPort.TabIndex = 28;
            this.lblPort.Text = "串口";
            // 
            // cmbPortName
            // 
            this.cmbPortName.FormattingEnabled = true;
            this.cmbPortName.Location = new System.Drawing.Point(422, 75);
            this.cmbPortName.Name = "cmbPortName";
            this.cmbPortName.Size = new System.Drawing.Size(121, 20);
            this.cmbPortName.TabIndex = 29;
            this.cmbPortName.SelectedIndexChanged += new System.EventHandler(this.cmbPortName_SelectedIndexChanged);
            // 
            // lblBaud
            // 
            this.lblBaud.AutoSize = true;
            this.lblBaud.Location = new System.Drawing.Point(337, 120);
            this.lblBaud.Name = "lblBaud";
            this.lblBaud.Size = new System.Drawing.Size(41, 12);
            this.lblBaud.TabIndex = 30;
            this.lblBaud.Text = "波特率";
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Location = new System.Drawing.Point(422, 120);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(121, 20);
            this.cmbBaudRate.TabIndex = 31;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(322, 162);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 32;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(422, 162);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(100, 21);
            this.txtSend.TabIndex = 33;
            this.txtSend.TextChanged += new System.EventHandler(this.txtSend_TextChanged);
            // 
            // cmbCommMode
            // 
            this.cmbCommMode.FormattingEnabled = true;
            this.cmbCommMode.Location = new System.Drawing.Point(322, 423);
            this.cmbCommMode.Name = "cmbCommMode";
            this.cmbCommMode.Size = new System.Drawing.Size(121, 20);
            this.cmbCommMode.TabIndex = 34;
            this.cmbCommMode.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // chartMain
            // 
            chartArea1.Name = "ChartArea1";
            this.chartMain.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartMain.Legends.Add(legend1);
            this.chartMain.Location = new System.Drawing.Point(566, 41);
            this.chartMain.Name = "chartMain";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartMain.Series.Add(series1);
            this.chartMain.Size = new System.Drawing.Size(317, 238);
            this.chartMain.TabIndex = 35;
            this.chartMain.Text = "chart1";
            // 
            // lstAlarmLog
            // 
            this.lstAlarmLog.FormattingEnabled = true;
            this.lstAlarmLog.ItemHeight = 12;
            this.lstAlarmLog.Location = new System.Drawing.Point(40, 355);
            this.lstAlarmLog.Name = "lstAlarmLog";
            this.lstAlarmLog.Size = new System.Drawing.Size(120, 88);
            this.lstAlarmLog.TabIndex = 36;
            // 
            // txtRegAddr
            // 
            this.txtRegAddr.Location = new System.Drawing.Point(535, 352);
            this.txtRegAddr.Name = "txtRegAddr";
            this.txtRegAddr.Size = new System.Drawing.Size(100, 21);
            this.txtRegAddr.TabIndex = 37;
            // 
            // txtRegValue
            // 
            this.txtRegValue.Location = new System.Drawing.Point(535, 393);
            this.txtRegValue.Name = "txtRegValue";
            this.txtRegValue.Size = new System.Drawing.Size(100, 21);
            this.txtRegValue.TabIndex = 38;
            // 
            // btnWriteReg
            // 
            this.btnWriteReg.Location = new System.Drawing.Point(322, 369);
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
            this.lblRegAddr.Location = new System.Drawing.Point(442, 355);
            this.lblRegAddr.Name = "lblRegAddr";
            this.lblRegAddr.Size = new System.Drawing.Size(65, 12);
            this.lblRegAddr.TabIndex = 40;
            this.lblRegAddr.Text = "寄存器地址";
            // 
            // lblRegValue
            // 
            this.lblRegValue.AutoSize = true;
            this.lblRegValue.Location = new System.Drawing.Point(442, 393);
            this.lblRegValue.Name = "lblRegValue";
            this.lblRegValue.Size = new System.Drawing.Size(53, 12);
            this.lblRegValue.TabIndex = 41;
            this.lblRegValue.Text = "寄存器值";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 568);
            this.Controls.Add(this.lblRegValue);
            this.Controls.Add(this.lblRegAddr);
            this.Controls.Add(this.btnWriteReg);
            this.Controls.Add(this.txtRegValue);
            this.Controls.Add(this.txtRegAddr);
            this.Controls.Add(this.lstAlarmLog);
            this.Controls.Add(this.chartMain);
            this.Controls.Add(this.cmbCommMode);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.cmbBaudRate);
            this.Controls.Add(this.lblBaud);
            this.Controls.Add(this.cmbPortName);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.btnExportCsv);
            this.Controls.Add(this.btnShowHistory);
            this.Controls.Add(this.dgvHistory);
            this.Controls.Add(this.btnRecover);
            this.Controls.Add(this.btnFault);
            this.Controls.Add(this.txtFaultCount);
            this.Controls.Add(this.t);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtalarmCount);
            this.Controls.Add(this.labalarmCount);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.labCount);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtPressure);
            this.Controls.Add(this.txtTemperature);
            this.Controls.Add(this.txtDeviceName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtDeviceId);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).EndInit();
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
        private System.Windows.Forms.Label label6;
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
    }
}


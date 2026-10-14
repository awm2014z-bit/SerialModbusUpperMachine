// 全局配置常量：通信参数、量程换算、报警阈值、各类上限
// 独立成文件，一是方便现场改参数，二是避免顶到窗体类前面导致设计器认不出 Form1.cs

namespace WindowsFormsApp1
{
    internal static class AppConfig
    {
        // 通信模式
        public const string ModeCustom = "自定义协议";
        public const string ModeModbus = "Modbus RTU";

        // 设备信息
        public const string DeviceId = "10001";
        public const string DeviceName = "温度采集器";

        // Modbus
        public const byte ModbusSlaveId = 1;
        public const int ModbusReadStartAddress = 0;
        public const int ModbusReadRegisterCount = 2;
        public const int ModbusConnectTimeoutMs = 1000;

        // 采集周期（ms）
        public const int PollIntervalMs = 1000;

        // 量程换算
        public const double TempScale = 10.0;        // 原始值 / 10  -> ℃
        public const double PressureScale = 100.0;   // 原始值 / 100 -> MPa

        // 压力报警阈值（MPa）
        public const double PressureAlarmLimit = 0.9;

        // 上限
        public const int MaxHistoryCount = 1000;
        public const int MaxAlarmLogCount = 100;
        public const int MaxChartPoints = 50;
        public const int MaxFailCount = 3;           // 连续失败几次自动断开
        public const int ReceiveBufferLimit = 4096;

        // 数据库
        public const int DbBatchSize = 50;           // 单事务最多写入条数
        public const int DbFlushIntervalMs = 500;    // 凑批等待间隔
        public const int DbQueueCapacity = 20000;
        public const int DbEnqueueTimeoutMs = 100;   // 入队超时，满则丢帧
        public const int DbShutdownTimeoutMs = 3000;
    }
}

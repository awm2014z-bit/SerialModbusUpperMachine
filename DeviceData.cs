using System;

namespace WindowsFormsApp1
{
    /// <summary>
    /// 单次采集的设备数据
    /// </summary>
    public class DeviceData
    {
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public DateTime ReadTime { get; set; }
        public bool IsAlarm { get; set; }
        public string Status { get; set; }

        public DeviceData(double temp, double press, bool isAlarm, string status)
        {
            Temperature = temp;
            Pressure = press;
            ReadTime = DateTime.Now;
            IsAlarm = isAlarm;
            Status = status;
        }
    }
}
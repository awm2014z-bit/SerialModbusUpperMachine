// 落库记录：只带数据库需要的列，与界面对象解耦
// 用 struct 而不是 class，每秒 new 多次也不产生 GC 压力

namespace WindowsFormsApp1
{
    internal struct DbRecord
    {
        public readonly string RecordTime;
        public readonly double Temperature;
        public readonly double Pressure;
        public readonly string VisionResult; 

        // 构造函数必须写在大括号里面！
        public DbRecord(string time, double temperature, double pressure, string visionResult)
        {
            RecordTime = time;
            Temperature = temperature;
            Pressure = pressure;
            VisionResult = visionResult;
        }
    }
}
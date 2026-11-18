using System;
using System.IO;
using System.Threading;

namespace WindowsFormsApp1
{
    public static class VisionHelper
    {
        // 模拟视觉处理接口
        public static string RunVisionProcess()
        {
            try
            {
                
                string imagePath = "test.jpg";
                if (!File.Exists(imagePath))
                {
                    return "找不到图片";
                }

                // 2. 模拟Halcon算法处理耗时
                Thread.Sleep(100);

                // 3. 假装Halcon算完了，返回结果
                // 真实项目中，这里会根据 Halcon 算子（比如 Blob分析）的返回值来判断
                Random r = new Random();
                bool isOk = r.Next(100) > 20; // 80%的概率是OK
                return isOk ? "OK" : "NG";
            }
            catch (Exception ex)
            {
                return "视觉报错";
            }
        }
    }
}
// 统一日志出口：VS 输出窗口 + 按日分文件的本地日志
// 采集线程、串口线程、写库线程都会调用，写文件必须加锁

using System;
using System.IO;
using System.Text;
using System.Threading;

namespace WindowsFormsApp1
{
    internal static class Logger
    {
        private static readonly object SyncRoot = new object();
        private static readonly string LogDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        public static void Debug(string message) { Write("DEBUG", message, null); }
        public static void Info(string message) { Write("INFO ", message, null); }
        public static void Warn(string message) { Write("WARN ", message, null); }
        public static void Error(Exception ex, string message) { Write("ERROR", message, ex); }

        private static void Write(string level, string message, Exception ex)
        {
            // 带时间戳和线程号，多线程下能分辨是谁打的
            string line = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} [{1}] [T{2:D2}] {3}",
                                        DateTime.Now, level, Thread.CurrentThread.ManagedThreadId, message);
            if (ex != null)
                line += Environment.NewLine + ex;

            System.Diagnostics.Debug.WriteLine(line);

            try
            {
                lock (SyncRoot)
                {
                    if (!Directory.Exists(LogDir))
                        Directory.CreateDirectory(LogDir);

                    string file = Path.Combine(LogDir, "app-" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                    File.AppendAllText(file, line + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch (Exception logEx)
            {
                // 日志写不进去也不能往上抛，否则一个日志问题会把主流程带崩
                System.Diagnostics.Debug.WriteLine("写日志文件失败：" + logEx.Message);
            }
        }
    }
}

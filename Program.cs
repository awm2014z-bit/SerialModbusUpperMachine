using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 全局异常捕获
            Application.ThreadException += (sender, e) =>
            {
                MessageBox.Show("发生未处理的错误：" + e.Exception.Message, "系统错误");
            };
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                MessageBox.Show("严重错误：" + e.ExceptionObject.ToString(), "系统崩溃");
            };

            Application.Run(new Form1());
        }
    }
}
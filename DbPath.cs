// 数据库路径与连接工厂
//
// 用 BaseDirectory 而不是相对路径：相对路径跟着"当前工作目录"走，
// VS 调试是 bin\Debug\，双击快捷方式是 exe 所在目录，会写出两个库，
// 现场表现就是"历史记录不见了"。

using System;
using System.Data.SQLite;
using System.IO;

namespace WindowsFormsApp1
{
    internal static class DbPath
    {
        public static readonly string DataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
        public static readonly string File = Path.Combine(DataDir, "HistoryData.db");

        public static string ConnectionString
        {
            get
            {
                // Pooling：复用连接，省掉每条数据一次的打开/关闭开销
                return "Data Source=" + File + ";Version=3;Pooling=True;FailIfMissing=False;";
            }
        }

        /// <summary>统一连接入口。PRAGMA 是连接级设置，所以不能从别处 new 连接，必须都走这里。</summary>
        public static SQLiteConnection OpenConnection()
        {
            if (!Directory.Exists(DataDir))
                Directory.CreateDirectory(DataDir);   // SQLite 不会自动建目录

            SQLiteConnection conn = new SQLiteConnection(ConnectionString);
            conn.Open();

            // WAL：读写并发，UI 查历史时不会把写线程顶死（否则报 database is locked）
            using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", conn))
                cmd.ExecuteNonQuery();
            // 撞锁时最多等 3 秒，而不是立刻失败
            using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA busy_timeout=3000;", conn))
                cmd.ExecuteNonQuery();

            return conn;
        }
    }
}

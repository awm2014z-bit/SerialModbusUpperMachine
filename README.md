# 串行Modbus上位机（Serial Modbus Upper Machine）

这是一个基于 C# WinForms 开发的 Modbus RTU 串口采集上位机演示项目，用于学习和展示标准上位机开发的工程化分层、多线程异步和数据库持久化。

## 核心功能
- **数据采集**：支持自定义 ASCII 协议和 Modbus RTU 协议，支持模拟数据采集。
- **实时显示**：采集数据实时刷新图表，支持温度、压力双 Y 轴曲线。
- **报警监控**：压力越限边沿触发报警（非持续报警），支持故障模拟与恢复。
- **历史存储**：采集数据自动落库 SQLite，支持查询历史记录与 CSV 导出。

## 技术栈与架构
- **语言框架**：C# / .NET Framework 4.8 / WinForms
- **通信库**：EasyModbus (Modbus RTU), System.IO.Ports (串口)
- **数据库**：SQLite (System.Data.SQLite)
- **工程化分层**：
  - `AppConfig.cs`：全局配置常量（消灭魔法数字）
  - `Logger.cs`：统一日志组件（记录异常，输出到 logs 文件夹）
  - `DbPath.cs`：统一数据库连接工厂（绝对路径 + WAL 并发模式）
  - `DbRecord.cs`：数据落库实体（轻量 struct）
  - `Form1.cs`：主窗体（仅负责界面逻辑与业务调度）

## 关键技术优化
- **UI 无阻塞**：串口轮询在后台线程进行，通过 `BeginInvoke` 安全更新界面；数据库写入采用生产者-消费者模式（内存队列 + 批量事务），确保高频率采集时界面不卡顿。
- **数据可靠性**：设置 `WAL` 模式和 `busy_timeout`，解决 SQLite 并发读写锁死问题；关闭窗口时强制排空写入队列，防止数据丢失。
- **异常可追溯**：杜绝空 `catch`，所有异常统一记录到按日期滚动的本地日志文件。

## 如何运行
1. 使用 Visual Studio 打开 `WindowsFormsApp1.sln`。
2. 还原 NuGet 包（右键解决方案 -> 还原 NuGet 包）。
3. 编译运行。若无下位机，可点击“读取数据”按钮开启模拟采集。
4. 数据库文件生成在 `bin/x86/Debug/data/HistoryData.db`，日志生成在 `bin/x86/Debug/logs/`。

## 后续规划
- [ ] 引入配置文件 (appsettings.json)，支持运行时修改参数。
- [ ] 引入依赖注入，进一步解耦通信层与界面层。
- [ ] 支持 OPC UA 协议接入。
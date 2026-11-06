# 串行Modbus上位机（Serial Modbus Upper Machine）

基于 C# WinForms 开发的 Modbus RTU / ASCII 串口采集上位机。项目重点不在于界面本身，而在于演示一套**标准上位机应有的工程化分层**：多线程采集与 UI 解耦、生产者-消费者异步落库、SQLite 并发调优、统一日志与配置抽离。

## 界面预览

![主界面](docs/screenshot.png)

## 核心功能

- **数据采集**：支持自定义 ASCII 协议和 Modbus RTU 协议，支持无下位机时的模拟数据采集。
- **实时显示**：采集数据实时刷新图表，支持温度、压力双 Y 轴曲线。
- **报警监控**：压力越限边沿触发报警（非持续报警），支持故障模拟与恢复。
- **历史存储**：采集数据自动落库 SQLite，支持查询历史记录与 CSV 导出。

## 技术栈

| 项目 | 说明 |
| --- | --- |
| 语言 / 框架 | C# / .NET Framework **4.7.2** / WinForms |
| 通信 | EasyModbusTCP 5.6.0（Modbus RTU）、System.IO.Ports（串口） |
| 存储 | System.Data.SQLite 1.0.119 |
| 图表 | WinForms Chart（双 Y 轴） |

## 工程化分层

| 文件 | 职责 |
| --- | --- |
| `AppConfig.cs` | 全局配置常量（消灭魔法数字，参数集中管理） |
| `Logger.cs` | 统一日志组件（分级记录异常，输出到 `logs/`，按日期滚动） |
| `DbPath.cs` | 数据库连接工厂（绝对路径 + WAL 并发模式 + 连接池） |
| `DbRecord.cs` | 数据落库实体（轻量 struct，与 UI 解耦） |
| `DeviceData.cs` | 采集数据结构 |
| `Form1.cs` | 主窗体（仅负责界面逻辑与业务调度） |

## 关键技术优化

- **UI 无阻塞**：串口轮询在后台线程进行，通过 `BeginInvoke` 安全更新界面（`SafeInvoke` 内含 `IsDisposed` / `IsHandleCreated` / `isClosing` 三重判断）；数据库写入采用生产者-消费者模式（`BlockingCollection` 内存队列 + 后台线程批量事务），高频采集时界面不卡顿。
- **数据可靠性**：开启 `journal_mode=WAL` 与 `busy_timeout=3000`，解决 SQLite 并发读写 `database is locked`；关闭窗口时强制排空写入队列，末尾数据不丢失。
- **异常可追溯**：杜绝空 `catch`，所有异常统一记录到按日期滚动的本地日志文件。

## 环境要求

- Windows 7 及以上
- Visual Studio 2019 或 2022（含「.NET 桌面开发」工作负载）
- .NET Framework 4.7.2 运行时

## 如何运行

1. 用 Visual Studio 打开 **`WindowsFormsApp1.sln`**。
   （若使用 VS 2022 17.14 及以上版本，也可打开 `WindowsFormsApp1.slnx`。）
2. 右键解决方案 → **还原 NuGet 包**。
3. 平台选择 `x86`，编译运行。
4. 无下位机时，点击「读取数据」按钮即可开启模拟采集。
5. 运行时生成的文件：
   - 数据库：`bin/x86/Debug/data/HistoryData.db`
   - 日志：`bin/x86/Debug/logs/`

## 后续规划

- [ ] 引入配置文件（appsettings.json），支持运行时修改参数而无需重新编译。
- [ ] 引入依赖注入，进一步解耦通信层与界面层。
- [ ] 支持 OPC UA 协议接入。
- [ ] 补充单元测试（协议解析、报警边沿判定）。

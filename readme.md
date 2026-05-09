![](https://cdn.discordapp.com/attachments/374517624228544514/399216250057916436/unknown.png)

Triggernometry 是 [Advanced Combat Tracker](https://advancedcombattracker.com/) 的触发器插件，提供变量、条件、表达式等更高级的功能，包含更多类型的动作和更丰富的配置选项。

由于原作者自 2024 年（1.2.0.7 版本）后已经停更，此仓库目前独立于原始仓库维护。

## 安装与更新

由于此版本的插件使用者主要为中国大陆用户，故插件使用国内 123 网盘分发下载，并使用 123 网盘的直链自动更新。

为方便刚接触 ACT 的用户，提供了简易安装程序。

将简易安装程序置于 ACT 安装目录并运行，按照其中的文本提示操作，即可向 ACT 自动添加插件。

此安装程序包含了 Triggernometry 插件本体、汉化文件、及很多触发器功能需要依赖的[鲶鱼精邮差](https://github.com/Natsukage/PostNamazu)插件。

· [一键安装器下载链接](https://www.123865.com/s/1xRXjv-n9qBH)

（此安装脚本源码见：[链接](https://github.com/MnFeN/Triggernometry/blob/readme/%E4%B8%80%E9%94%AE%E5%AE%89%E8%A3%85%E8%A7%A6%E5%8F%91%E5%99%A8%EF%BC%8FPostNamazu/%E4%B8%80%E9%94%AE%E5%AE%89%E8%A3%85%E8%A7%A6%E5%8F%91%E5%99%A8%EF%BC%8FPostNamazu.py)）

如果你倾向于自己安装，可以使用：

· [插件本体下载链接](https://1824544011.v.123pan.cn/1824544011/Triggernometry_Release_CN/Triggernometry.dll) 

· [汉化文件下载链接](https://1824544011.v.123pan.cn/1824544011/Triggernometry_Release_CN/zh-CN.triglations.xml)  

默认设置下，插件会在每次启动 ACT 时自动检查并安装更新。

## 故障排查

此版本的插件默认添加一系列远程仓库，其中包含自检工具箱。

成功使用上述安装程序添加插件后，便可以使用此远程仓库解决使用过程中可能遇到的大部分问题。

## 更新日志

[Triggernometry 更新日志](https://github.com/MnFeN/Triggernometry/wiki/%E6%9B%B4%E6%96%B0%E6%97%A5%E5%BF%97)（2.0.0.0 版以后）

## 文档

Triggernometry Wiki 中包含了一些有用的信息和文档说明：

https://github.com/MnFeN/Triggernometry/wiki

## 编译

本项目使用 .NET Framework 4.8.1。

解决方案包含三个项目：

- `Triggernometry`：
  
  核心，编译生成 `TriggernometryPlugin.dll`
  
- `TriggernometryProxy`：

  包装核心功能的 ACT 插件，编译生成 `Triggernometry.dll`，用于导入 ACT。
  
- `TriggernometryScripting`：
  
  并非实际项目，不参与编译。提供前两个项目代码的智能补全提示环境，方便开发者编写触发器 C# 脚本动作的代码。

编译时直接生成 `TriggernometryProxy` 项目即可。

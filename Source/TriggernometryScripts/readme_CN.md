# TriggernometryScripting 项目说明

TriggernometryScripting 项目为开发者提供脚本编辑环境，不参与构建，也不会生成输出文件。
其用途是为脚本提供 IntelliSense，并引用 Triggernometry 相关 API。

## UserScripts 文件夹

`UserScripts` 用于存放用户编写的脚本文件。  
目录本身会提交，但内部内容已被 Git 忽略，可自由增删。

## 注意事项

- 本项目不执行脚本、不生成 DLL  
- 此处编写的脚本只有在复制到触发器的“执行脚本动作（Execute C# Script）”中时才会真正运行  
- `UserScripts` 内所有文件均被忽略，不影响仓库  

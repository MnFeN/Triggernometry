using Advanced_Combat_Tracker;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

List<string> pluginNames = ActGlobals.oFormActMain.ActPlugins
    .Where(p => p.cbEnabled.Checked)
    .Select(p => p.pluginObj.GetType().ToString())
    .ToList();

var hasCafe = pluginNames.Contains("ACT.FoxTTS.FoxTTSPlugin");
var hasDieMoe = pluginNames.Contains("ACT.TTS_CN.PluginMain");
string msg;
var icon = MessageBoxIcon.Information;

if (hasCafe && hasDieMoe)
{
    msg = @"
检测到已启用 FoxTTS (Cafe) 和 TTS_CN (呆萌) 两个插件。
你需要在 ACT 插件列表中禁用其中一个。";
    icon = MessageBoxIcon.Error;
}

else if (hasDieMoe)
{
    msg = @"
已启用 TTS_CN (呆萌) 插件，可以继续测试下一项。

如果你在下一项测试中发现 TTS 无法正常工作，
可以换用 Cafe 的 FoxTTS：

Cafe 整合可直接在插件中心安装
呆萌整合需要在网盘下载（见自检工具箱使用说明）。";
}
else if (hasCafe)
{
    msg = @"
已启用 FoxTTS (Cafe) 插件，可以继续测试下一项。

如果你在下一项测试中发现 TTS 无法正常工作，
可以换用呆萌的 ACT.TTS_CN（自行搜索下载）。";
}
else
{
    msg = @"
未检测到已知的整合版 TTS 插件。

除非你自己安装了其他 TTS 插件，
否则需要启用一个 TTS 插件（呆萌和 Cafe 整合都自带一个）。";
    icon = MessageBoxIcon.Warning;
}

MessageBox.Show(msg, "TTS 插件检查", MessageBoxButtons.OK, icon);
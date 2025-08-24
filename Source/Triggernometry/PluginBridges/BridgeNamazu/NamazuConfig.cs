using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Triggernometry;
using Triggernometry.Forms;
using Triggernometry.PluginBridges.BridgeNamazu.Modules;
using static Triggernometry.Forms.GameConfigForm;
using static Triggernometry.Utilities.DataStringHelper;


namespace Triggernometry.PluginBridges.BridgeNamazu
{
    internal class NamazuConfig
    {
        internal static ConfigInfo Info = new ConfigInfo("鲶鱼精邮差扩展", null, null, "PNE_cfg");

        public static void TryRunConfigForm()
        {
            try
            {
                RunConfigForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置界面运行时遇到问题：\n\n" + ex, Info.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void RunConfigForm()
        {
            GameConfigForm form = new GameConfigForm(Info);
            form.Shown += (sender, e) => RealPlugin.plug.InvokeNamedCallback("command", "/e <se.9>");
            BijectDictionary<string, string> cbxItems;
            Option optionCtrl;
            Label label;
            string hint = "";

            // 在下面倒序添加各个选项组（组内的选项正序）

            #region 杂项

            OptionsTableLayoutPanel tableMisc = form.AddOptionGroup(" 杂项 ");

            hint = "禁用后可无视所有挂机计时器（防止踢出副本或掉线）";
            optionCtrl = new OptionChk("　禁用挂机计时器", "InstanceAfkTimer", true, hint);
            form.AddOption(optionCtrl, tableMisc);

            #endregion VFX

            #region 实体

            OptionsTableLayoutPanel tableEntity = form.AddOptionGroup(" 实体属性");

            optionCtrl = new OptionChk("　允许调用实体缩放", "ObjectScale", true, hint);
            form.AddOption(optionCtrl, tableEntity);

            optionCtrl = new OptionChk("　允许调整实体透明度", "Opacity", true, hint);
            form.AddOption(optionCtrl, tableEntity);

            #endregion VFX

            #region VFX

            OptionsTableLayoutPanel tableVfx = form.AddOptionGroup(" VFX 特效（绘图）");

            form.AddLabel("　选项仅代表是否可使用触发器调用此类特效，不影响游戏默认行为。", tableVfx);

            hint = "实体特效：点名（LockOn）、连线（Channeling）、咏唱（ActorCastVfx）等绑定于实体的特效。\n通过直接写内存产生的 buff 附带特效（StatusLoopVfx）无需调用特效函数，不受此影响。";
            optionCtrl = new OptionChk("　允许调用实体特效 (ActorVfx)", "ActorVfx", true, hint);
            form.AddOption(optionCtrl, tableVfx);

            hint = "静态特效：技能预兆（Omen）等不依靠实体的特效。";
            optionCtrl = new OptionChk("　允许调用静态特效 (StaticVfx)", "StaticVfx", true, hint);
            form.AddOption(optionCtrl, tableVfx);

            hint = "地图特效：副本场地特有的某些特效。";
            optionCtrl = new OptionChk("　允许调用地图特效 (MapEffect)", "MapEffect", true, hint);
            form.AddOption(optionCtrl, tableVfx);

            #endregion VFX

            #region 相机参数

            OptionsTableLayoutPanel tableCamera = form.AddOptionGroup(" 相机参数 ");

            hint = "启用后会在 ACT 开启时将游戏的相机参数自动改写为以下数值。\n以下参数文本修改后立刻可预览。";
            var optionCameraEnabled = new OptionChk("　自动修改相机参数", "camera_enabled", true);
            form.AddOption(optionCameraEnabled, tableCamera);

            var optionCameraMinDistance = new OptionTxt("　最小视距（原始值 1.5）", "camera_MinDistance", "0.5");
            form.AddOption(optionCameraMinDistance, tableCamera);
            SetLostFocusEventForCameraTxt(optionCameraMinDistance);

            var optionCameraMaxDistance = new OptionTxt("　最大视距（原始值 20）", "camera_MaxDistance", "9999");
            form.AddOption(optionCameraMaxDistance, tableCamera);
            SetLostFocusEventForCameraTxt(optionCameraMaxDistance);

            hint = "纵向视场角代表相机纵向视角的大小，单位为弧度（rad）。";
            var optionCameraMinFoV = new OptionTxt("　最小纵向视场角（原始值 0.69）", "camera_MinFoV", "0.69", hint);
            form.AddOption(optionCameraMinFoV, tableCamera);
            SetLostFocusEventForCameraTxt(optionCameraMinFoV);

            hint = "纵向视场角代表相机纵向视角的大小，单位为弧度（rad）。";
            var optionCameraMaxFoV = new OptionTxt("　最大纵向视场角（原始值 0.78）", "camera_MaxFoV", "0.78", hint);
            form.AddOption(optionCameraMaxFoV, tableCamera);
            SetLostFocusEventForCameraTxt(optionCameraMaxFoV);

            hint = "纵向视角方向代表视角与水平面的夹角，上方为正，单位为弧度（rad）。\n过于接近 -π/2 (-1.5708) 会导致某些情况下视角无法旋转，影响游戏。";
            var optionCameraMinAngleV = new OptionTxt("　最低纵向视角方向（原始值 -1.483）", "camera_MinAngleV", "-1.569", hint);
            form.AddOption(optionCameraMinAngleV, tableCamera);
            SetLostFocusEventForCameraTxt(optionCameraMinAngleV);

            hint = "纵向视角方向代表视角与水平面的夹角，上方为正，单位为弧度（rad）。\n超过 π/2 (1.5708) 会导致视角反向。";
            var optionCameraMaxAngleV = new OptionTxt("　最高纵向视角方向（原始值 0.785）", "camera_MaxAngleV", "1.569", hint);
            form.AddOption(optionCameraMaxAngleV, tableCamera);
            SetLostFocusEventForCameraTxt(optionCameraMaxAngleV);

            optionCameraEnabled.Chk.CheckedChanged += (sender, e) =>
            {
                var enabled = ((CheckBox)sender).Checked;
                optionCameraMinDistance.Txt.Enabled = enabled;
                optionCameraMaxDistance.Txt.Enabled = enabled;
                optionCameraMinFoV.Txt.Enabled = enabled;
                optionCameraMaxFoV.Txt.Enabled = enabled;
                optionCameraMinAngleV.Txt.Enabled = enabled;
                optionCameraMaxAngleV.Txt.Enabled = enabled;
            };

            #endregion 相机参数

            Application.OpenForms.OfType<GameConfigForm>().ToList().ForEach(f => f.Close());
            form.Run();
        }

        static void SetLostFocusEventForCameraTxt(OptionTxt optionTxt)
        { 
            optionTxt.Txt.LostFocus += (sender, e) =>
            {
                if (!optionTxt.Txt.Enabled) return;
                try
                {
                    var value = ParseArgs<float>(optionTxt.Txt.Text);
                    var key = optionTxt.ConfigKey.Substring(7); // camera_xxx
                    BridgeNamazu.GetModule<CameraModule>().SetParam(key, value);
                }
                catch
                {
                    MessageBox.Show("请输入有效的数值！", optionTxt.Lbl.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    optionTxt.Txt.Focus();
                }
            };
        }

    }
}
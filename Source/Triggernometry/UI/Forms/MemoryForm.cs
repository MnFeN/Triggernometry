using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Triggernometry.UI.CustomControls;
using Triggernometry.Localization;
using Triggernometry.Core;

namespace Triggernometry.UI.Forms
{

    public class MemoryForm<T> : Form
    {

        internal static bool SavedSet { get; set; } = false;
        internal static int SavedWidth { get; set; }
        internal static int SavedHeight { get; set; }

        private Font _FontOverride = null;

        public MemoryForm()
        {
            FormClosing += MemoryForm_FormClosing;
            Disposed += MemoryForm_Disposed;
        }

        private void MemoryForm_Disposed(object sender, EventArgs e)
        {
            if (_FontOverride != null)
            {
                _FontOverride.Dispose();
                _FontOverride = null;
            }
        }

        protected void RestoredSavedDimensions()
        {
            // 翻译、字体和窗口尺寸会连续改变子控件，批量处理可避免每次都重新布局。
            var layoutControls = new List<Control> { this };
            for (int i = 0; i < layoutControls.Count; i++)
            {
                Control control = layoutControls[i];
                control.SuspendLayout();
                foreach (Control child in control.Controls)
                    layoutControls.Add(child);
            }
            try
            {
                I18n.TranslateForm(this);
                Configuration cfg = RealPlugin.Instance.Config;
                if (cfg.UiFontDefault == false)
                {
                    try
                    {
                        _FontOverride = RealPlugin.CreateFontFromDefinition(cfg.UiFontName, cfg.UiFontSize, cfg.UiFontEffect);
                        RealPlugin.ApplyFontOverrideToForm(this, _FontOverride);
                    }
                    catch (Exception)
                    {
                    }
                }
                MaximizeBox = false;
                MinimizeBox = false;
                if (SavedSet == true)
                {
                    Width = SavedWidth;
                    Height = SavedHeight;
                }
            }
            finally
            {
                // 从子控件开始恢复，父容器最后统一布局。
                for (int i = layoutControls.Count - 1; i >= 0; i--)
                    layoutControls[i].ResumeLayout(true);
            }
        }

        private void MemoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SavedSet = true;
            SavedWidth = Width;
            SavedHeight = Height;
        }

    }

}

using System.Drawing;
using System.Windows.Forms;

namespace Triggernometry.UI.Forms
{
    public partial class GameConfigForm
    {
        private class MyGroupBox : GroupBox
        {
            public MyGroupBox(string text) : base()
            {
                Dock = DockStyle.Top;
                AutoSize = true;
                AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Margin = new Padding(20);
                Text = text;
            }
        }

        private class MyCheckBox : System.Windows.Forms.CheckBox
        {
            public MyCheckBox() : base()
            {
                AutoSize = true;
                Dock = DockStyle.Fill;
                Margin = new Padding(10);
            }
        }

        private class MyTextBox : System.Windows.Forms.TextBox
        {
            public MyTextBox() : base()
            {
                AutoSize = true;
                Dock = DockStyle.Fill;
                Margin = new Padding(10);
            }
        }

        private class MyComboBox : System.Windows.Forms.ComboBox
        {
            public MyComboBox() : base()
            {
                AutoSize = true;
                Dock = DockStyle.Fill;
                Margin = new Padding(10);
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 0x020A)  // WM_MOUSEWHEEL
                {
                    return;  // No-scroll
                }
                base.WndProc(ref m);
            }
        }

        private class MyNumericUpDown : System.Windows.Forms.NumericUpDown
        {
            public MyNumericUpDown() : base()
            {
                AutoSize = true;
                Dock = DockStyle.Fill;
                Margin = new Padding(10);
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 0x020A)  // WM_MOUSEWHEEL
                {
                    return;  // No-scroll
                }
                base.WndProc(ref m);
            }
        }

        private class MyLabel : System.Windows.Forms.Label
        {
            public MyLabel() : base()
            {
                AutoSize = true;
                Dock = DockStyle.Fill;
                Margin = new Padding(10);
            }
        }

        private class MyButton : System.Windows.Forms.Button
        {
            public MyButton() : base()
            {
                Anchor = AnchorStyles.None;
                AutoSize = true;
                Margin = new Padding(10);
                Padding = new Padding(5);
            }
        }

        private class SeperatorPanel : System.Windows.Forms.Panel
        {
            public SeperatorPanel() : base()
            {
                Height = 2;
                BackColor = Color.DarkGray;
                Dock = DockStyle.Fill;
                AutoSize = true;
                Margin = new Padding(10);
            }
        }

        private class BackgroundPanel : System.Windows.Forms.Panel
        {
            public BackgroundPanel() : base()
            {
                AutoSize = true;
                AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Dock = DockStyle.Fill;
                AutoScroll = true;
            }

            protected override Point ScrollToControl(Control activeControl)
            {
                // 防止自动滚动，使页面突然跳转到窗口范围外的 txtbox 等
                return this.DisplayRectangle.Location;
            }
        }

        private class GroupPanel : System.Windows.Forms.Panel
        {
            public GroupPanel() : base()
            {
                AutoSize = true;
                AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Dock = DockStyle.Top;
                Padding = new Padding(20, 20, 20, 0);
            }
        }

        public class OptionsTableLayoutPanel : System.Windows.Forms.TableLayoutPanel
        {
            public OptionsTableLayoutPanel() : base()
            {
                AutoSize = true;
                AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Dock = DockStyle.Fill;
                RowCount = 0;
                ColumnCount = 2;
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            }
        }

        private class BottomTableLayoutPanel : System.Windows.Forms.TableLayoutPanel
        {
            public BottomTableLayoutPanel() : base()
            {
                Dock = DockStyle.Bottom;
                ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            }
        }

        private class MyToolTip : System.Windows.Forms.ToolTip
        {
            public MyToolTip() : base()
            {
                InitialDelay = 0;
                AutoPopDelay = 30000;
                ReshowDelay = 0;
                ShowAlways = true;
            }
        }
    }
}
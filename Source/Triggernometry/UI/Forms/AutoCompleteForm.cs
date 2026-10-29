using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Triggernometry.UI.Forms
{

    public partial class AutoCompleteForm : Form
    {

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        public AutoCompleteForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }


        public string GetChosenAutocomplete()
        {
            if (listBox1.SelectedItems.Count == 1)
            {
                return listBox1.SelectedItem.ToString();
            }
            return "";
        }

        public void NextAutocompleteItem()
        {
            if (listBox1.Items.Count == 0) return;
            int i = listBox1.SelectedIndex + 1;
            if (i > listBox1.Items.Count - 1)
            {
                i = 0;
            }
            listBox1.SelectedIndex = i;
        }

        public void PreviousAutocompleteItem()
        {
            if (listBox1.Items.Count == 0) return;
            int i = listBox1.SelectedIndex - 1;
            if (i < 0)
            {
                i = listBox1.Items.Count - 1;
            }
            listBox1.SelectedIndex = i;
        }

        public void NextAutocompletePage()
        {
            if (listBox1.Items.Count == 0) return;
            int i = listBox1.SelectedIndex + 5;
            if (i > listBox1.Items.Count - 1)
            {
                i = listBox1.Items.Count - 1;
            }
            listBox1.SelectedIndex = i;
        }

        public void PreviousAutocompletePage()
        {
            if (listBox1.Items.Count == 0) return;
            int i = listBox1.SelectedIndex - 5;
            if (i < 0)
            {
                i = 0;
            }
            listBox1.SelectedIndex = i;
        }

        /// <summary>一次物化候选并批量替换列表，尽量保留原选中项。</summary>
        public void BuildList(IEnumerable<string> strs)
        {
            var items = strs as string[] ?? strs.ToArray();
            SuspendLayout();
            listBox1.BeginUpdate();
            try
            {
                int height = (1 + Math.Min(10, items.Length)) * listBox1.ItemHeight;
                float longest = 0;
                using (Graphics graphics = CreateGraphics())
                    foreach (string item in items)
                        longest = Math.Max(longest, graphics.MeasureString(item, listBox1.Font).Width);
                listBox1.Width = (int)Math.Ceiling(longest) + 20;
                listBox1.Height = height;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(items);
                int index = !(listBox1.SelectedItem is string selected) ? -1 
                    : Array.FindIndex(items, item => string.Equals(item, selected, StringComparison.OrdinalIgnoreCase));
                listBox1.SelectedIndex = items.Length == 0 ? -1 : Math.Max(0, index);
            }
            finally
            {
                listBox1.EndUpdate();
                ResumeLayout();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                m.Result = (IntPtr)(-1);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

    }

}

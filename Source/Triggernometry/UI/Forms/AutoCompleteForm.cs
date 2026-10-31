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
            if (lstSuggestions.SelectedItems.Count == 1)
            {
                return lstSuggestions.SelectedItem.ToString();
            }
            return "";
        }

        public void NextAutocompleteItem()
        {
            if (lstSuggestions.Items.Count == 0) return;
            int i = lstSuggestions.SelectedIndex + 1;
            if (i > lstSuggestions.Items.Count - 1)
            {
                i = 0;
            }
            lstSuggestions.SelectedIndex = i;
        }

        public void PreviousAutocompleteItem()
        {
            if (lstSuggestions.Items.Count == 0) return;
            int i = lstSuggestions.SelectedIndex - 1;
            if (i < 0)
            {
                i = lstSuggestions.Items.Count - 1;
            }
            lstSuggestions.SelectedIndex = i;
        }

        public void NextAutocompletePage()
        {
            if (lstSuggestions.Items.Count == 0) return;
            int i = lstSuggestions.SelectedIndex + 5;
            if (i > lstSuggestions.Items.Count - 1)
            {
                i = lstSuggestions.Items.Count - 1;
            }
            lstSuggestions.SelectedIndex = i;
        }

        public void PreviousAutocompletePage()
        {
            if (lstSuggestions.Items.Count == 0) return;
            int i = lstSuggestions.SelectedIndex - 5;
            if (i < 0)
            {
                i = 0;
            }
            lstSuggestions.SelectedIndex = i;
        }

        /// <summary>一次物化候选并批量替换列表，尽量保留原选中项。</summary>
        public void BuildList(IEnumerable<string> strs)
        {
            var items = strs as string[] ?? strs.ToArray();
            SuspendLayout();
            lstSuggestions.BeginUpdate();
            try
            {
                int height = (1 + Math.Min(10, items.Length)) * lstSuggestions.ItemHeight;
                float longest = 0;
                using (Graphics graphics = CreateGraphics())
                    foreach (string item in items)
                        longest = Math.Max(longest, graphics.MeasureString(item, lstSuggestions.Font).Width);
                lstSuggestions.Width = (int)Math.Ceiling(longest) + 20;
                lstSuggestions.Height = height;
                lstSuggestions.Items.Clear();
                lstSuggestions.Items.AddRange(items);
                int index = !(lstSuggestions.SelectedItem is string selected) ? -1 
                    : Array.FindIndex(items, item => string.Equals(item, selected, StringComparison.OrdinalIgnoreCase));
                lstSuggestions.SelectedIndex = items.Length == 0 ? -1 : Math.Max(0, index);
            }
            finally
            {
                lstSuggestions.EndUpdate();
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

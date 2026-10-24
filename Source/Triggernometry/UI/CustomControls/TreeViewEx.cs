using Triggernometry.Localization;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Triggernometry.Core;

namespace Triggernometry.UI.CustomControls
{

    public partial class TreeViewEx : TreeView
    {

        // Pinvoke:
        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        protected override void OnHandleCreated(EventArgs e)
        {
            SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }

        public TreeViewEx()
        {
            InitializeComponent();
            this.BeforeExpand += TreeViewEx_BeforeExpand;
            this.BeforeCheck += TreeViewEx_BeforeCheck;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x203) // identified double click
            {
                var localPos = PointToClient(Cursor.Position);
                var hitTestInfo = HitTest(localPos);
                if (hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
                {
                    m.Msg = 0x201; // lmb down
                    base.WndProc(ref m);
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void TreeViewEx_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is Folder folder && folder.Repo != null && folder.DisableRemoteExpand)
            {
                MessageBox.Show(I18n.Translate("internal/TreeViewEx/nobrowserequired", "You do not need to browse this folder."), I18n.Translate("internal/TreeViewEx/remotetriggers", "Remote triggers"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void TreeViewEx_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is Folder folder && folder.Repo != null && folder.DisableRemoteToggle)
            {
                MessageBox.Show(I18n.Translate("internal/TreeViewEx/noeditrequired", "You do not need to modify this folder."), I18n.Translate("internal/TreeViewEx/remotetriggers", "Remote triggers"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

    }

}
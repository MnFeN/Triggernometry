using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Triggernometry.Localization;
using Triggernometry.UI.Forms;
using Triggernometry.Core;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.UI.Forms
{

    public partial class BenchmarkForm : MemoryForm<BenchmarkForm>
    {

        public RealPlugin plug { get; set; }
        public List<BenchmarkItem> temp;
        public BenchmarkProgressForm bpf;

        public class BenchmarkItem
        {

            public Trigger trig;
            public long TimeOnMatch;
            public long TimeOnParent;
            public long TimeOnConditions;

            public long TimeTotal
            {
                get
                {
                    return TimeOnMatch + TimeOnParent + TimeOnConditions;
                }
            }

            public BenchmarkItem()
            {
                TimeOnMatch = 0;
                TimeOnParent = 0;
                TimeOnConditions = 0;
            }

        }

        public BenchmarkForm()
        {
            InitializeComponent();
            temp = new List<BenchmarkItem>();
            workerBenchmark.ProgressChanged += workerBenchmark_ProgressChanged;
            workerBenchmark.RunWorkerCompleted += workerBenchmark_RunWorkerCompleted;
            Disposed += BenchmarkForm_Disposed;
            RestoredSavedDimensions();
        }

        private void BenchmarkForm_Disposed(object sender, EventArgs e)
        {
            if (bpf != null)
            {
                bpf.Close();
                bpf.Dispose();
                bpf = null;
            }
        }

        private void workerBenchmark_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bpf.DialogResult = e.Cancelled || e.Error != null ? DialogResult.Cancel : DialogResult.OK;
            bpf.Close();
            bpf.Dispose();          
            bpf = null;
        }

        private void workerBenchmark_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            bpf.prgBenchmark.Value = e.ProgressPercentage;
        }

        internal void RunBenchmark()
        {
            temp.Clear();
            dgvResults.Rows.Clear();            
            workerBenchmark.RunWorkerAsync();
        }

        internal string TimeFormat(double seconds)
        {
            seconds = Math.Max(0, seconds);
            if (seconds < 1E-6) 
                return Math.Round(seconds * 1E9, 3).ToStringInvariant() + " ns";
            if (seconds < 1E-3) 
                return Math.Round(seconds * 1E6, 3).ToStringInvariant() + " µs";
            if (seconds < 1) 
                return Math.Round(seconds * 1E3, 3).ToStringInvariant() + " ms";
            else
                return Math.Round(seconds, 3).ToStringInvariant() + " s";
        }

        internal void ShowResults()
        {
            try
            {
                long totaltime = 0;
                foreach (BenchmarkItem bi in temp)
                {
                    totaltime += bi.TimeTotal;
                }                
                temp.Sort((a, b) => b.TimeTotal.CompareTo(a.TimeTotal));
                double dx = (double)(totaltime / 10000.0) / (double)Stopwatch.Frequency;
                dgvResults.Rows.Add(new object[] { I18n.Translate("internal/BenchmarkForm/esttotal", "(Estimated total evaluation time per event)"), null, TimeFormat(dx), null, null, null });
                dgvResults.Rows.Add(new object[] { I18n.Translate("internal/BenchmarkForm/estmaximum", "(Estimated maximum sustainable event load per second)"), null, dx > 0 ? (object)Math.Round(1.0 / dx, 2) : null, null, null });
                foreach (BenchmarkItem bi in temp)
                {                    
                    dgvResults.Rows.Add(new object[] {
                        bi.trig.FullPath, // name
                        Math.Round(totaltime > 0 ? (double)bi.TimeTotal / totaltime * 100.0 : 0, 2) + " %", // relative exec time
                        TimeFormat((double)(bi.TimeTotal / 10000.0) / (double)Stopwatch.Frequency), // average exec time
                        Math.Round(bi.TimeTotal > 0 ? (double)bi.TimeOnMatch / bi.TimeTotal * 100.0 : 0, 2) + " %", // % on regexp
                        Math.Round(bi.TimeTotal > 0 ? (double)bi.TimeOnParent / bi.TimeTotal * 100.0 : 0, 2) + " %", // % on folder
                        Math.Round(bi.TimeTotal > 0 ? (double)bi.TimeOnConditions / bi.TimeTotal * 100.0 : 0, 2) + " %", // % on conditions
                    });
                }
                dgvResults.Invalidate();
            }
            catch (Exception)
            {
                plug.FilteredAddToLog(RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/BenchmarkForm/resultexception", "An exception occurred during compiling benchmark results"));
            }
        }

        internal BenchmarkItem BenchmarkTrigger(Trigger t)
        {
            int i = 0;
            BenchmarkItem bi = new BenchmarkItem();
            bi.trig = t;
            string temp = "this is just a benchmark test string that has no bearing on anything important";
            Stopwatch st = Stopwatch.StartNew();
            while (i < 10000)
            {
                long stTime = st.ElapsedTicks;
                if (workerBenchmark.CancellationPending == true)
                {
                    return null;
                }                
                t.CheckMatch(temp);
                bi.TimeOnMatch += st.ElapsedTicks - stTime;
                stTime = st.ElapsedTicks;
                LogEvent le = new LogEvent() { ZoneId = temp, ZoneName = temp, Text = temp };
                t.Parent.PassesFilter(le);
                bi.TimeOnParent += st.ElapsedTicks - stTime;
                stTime = st.ElapsedTicks;
                if (t.Condition != null && t.Condition.Enabled == true)
                {
                    t.Condition.Check(Context.Unbound, t.TriggerContextLogger, plug);
                }
                bi.TimeOnConditions += st.ElapsedTicks - stTime;
                i++;
            }            
            st.Stop();
            return bi;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            btnCopyToClipboard.Enabled = false;
            using (bpf = new BenchmarkProgressForm())
            {
                bpf.bf = this;                
                switch (bpf.ShowDialog())
                {
                    case DialogResult.OK:
                        ShowResults();
                        btnCopyToClipboard.Enabled = (dgvResults.RowCount > 0);
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }            
        }

        private bool FoldersEnabled(Folder f)
        {
            if (f.Enabled == false)
            {
                return false;
            }
            if (f.Parent != null)
            {
                if (FoldersEnabled(f.Parent) == false)
                {
                    return false;
                }
            }
            return f.Enabled;
        }

        private void workerBenchmark_DoWork(object sender, DoWorkEventArgs e)
        {
            bool wasLoggingDisabled = plug.DisableLogging;
            try
            {
                plug.DisableLogging = true;
                List<Trigger> trigs = new List<Trigger>();
                lock (plug.Triggers)
                {
                    trigs.AddRange(plug.Triggers);
                }
                int i = 0;
                int j = trigs.Count;
                foreach (Trigger t in trigs)
                {
                    if (workerBenchmark.CancellationPending == true)
                    {
                        e.Cancel = true;
                        return;
                    }
                    if (t.Enabled == false)
                    {
                        continue;
                    }
                    if (FoldersEnabled(t.Parent) == false)
                    {
                        continue;
                    }
                    BenchmarkItem result = BenchmarkTrigger(t);
                    if (result == null)
                    {
                        e.Cancel = true;
                        return;
                    }
                    temp.Add(result);
                    i++;
                    workerBenchmark.ReportProgress((int)Math.Ceiling(100.0f * (float)i / (float)j));
                }
            }
            catch (Exception)
            {
                e.Cancel = true;
                plug.FilteredAddToLog(RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/BenchmarkForm/exception", "An exception occurred during benchmark"));
            }
            finally
            {
                plug.DisableLogging = wasLoggingDisabled;
            }
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataGridViewRow r in dgvResults.Rows)
                {
                    List<string> wot = new List<string>();
                    foreach (DataGridViewCell c in r.Cells)
                    {                        
                        wot.Add(c.Value != null ? "\"" + c.Value.ToString() + "\"" : "");
                    }
                    sb.AppendLine(string.Join(",", wot));
                }
                Clipboard.SetText(sb.ToString());
            }
            catch (Exception)
            {
            }
        }

    }

}

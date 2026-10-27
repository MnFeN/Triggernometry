using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Core.Actions;
using Triggernometry.Core.Conditions;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using ActionTypeEnum = Triggernometry.Core.ActionOld.ActionTypeEnum;
using static Triggernometry.UI.CustomControls.ColorSelector;

namespace Triggernometry.UI.Forms
{

    public partial class ActionForm : MemoryForm<ActionForm>
    {

        private bool IsReadonly { get; set; } = false;
        private ActionBase editedAction;

        // Keep the concrete types in the same order as the action selector and its existing pages.
        private static readonly Type[] actionTypes =
        {
            typeof(ActionBeep),
            typeof(ActionPlaySound),
            typeof(ActionPlaySpeech),
            typeof(ActionVariableScalar),
            typeof(ActionVariableList),
            typeof(ActionVariableTable),
            typeof(ActionVariableDict),
            typeof(ActionMessageBox),
            typeof(ActionLogMessage),
            typeof(ActionOverlayText),
            typeof(ActionOverlayImage),
            typeof(ActionMouse),
            typeof(ActionKeypress),
            typeof(ActionNamedCallback),
            typeof(ActionWindowMessage),
            typeof(ActionDiskOperation),
            typeof(ActionLaunchProcess),
            typeof(ActionExecuteScript),
            typeof(ActionMutex),
            typeof(ActionLoop),
            typeof(ActionJsonRequest),
            typeof(ActionDiscordWebhook),
            typeof(ActionLiveSplitControl),
            typeof(ActionObsControl),
            typeof(ActionActInteraction),
            typeof(ActionTriggerOperation),
            typeof(ActionFolderOperation),
            typeof(ActionRepository),
            typeof(ActionPlaceholder),
        };

        private string EditorTempFn { get; set; }
        private Process ExtEditor { get; set; } = null;

        private Context _uiContext;
        internal Context UiContext
        {
            get => _uiContext;
            private set
            {
                _uiContext = value;
                loopActionViewer.UiContext = value;
                cndCondition.UiContext = value;
                cndLoopCondition.UiContext = value;
            }
        }

        internal Trigger ParentTrigger => UiContext?.Trigger;
        internal RealPlugin Plugin => UiContext?.Plugin;

        private ImageList _images;
        internal ImageList Images
        {
            get => _images;
            set
            {
                _images = value;
                loopActionViewer.Images = value;
                trvTrigger.ImageList = value;
                trvFolder.ImageList = value;
                trvRepositoryLink.ImageList = value;
            }
        }

        private TreeView _treeView;
        internal TreeView TreeView
        {
            get => _treeView;
            set
            {
                _treeView = value;
                loopActionViewer.TreeView = value;
            }
        }

        internal static ImageConverter imageConverter = new ImageConverter();

        public class FontInfoContainer
        {

            public string Name { get; set; }
            public float Size { get; set; }
            public ActionOld.TextAuraEffectEnum Effect { get; set; }

        }

        public ActionForm(ActionBase action, Context uiContext, TreeView treeView, ImageList imageList)
        {
            InitializeComponent();
            expAuraImage.textBox1.TextChanged += TextBox1_TextChanged;
            if (DesignMode == false)
            {
                tbcActionSettings.ItemSize = new Size(0, 1);
                tbcActionSettings.SizeMode = TabSizeMode.Fixed;
            }
            btnTest.Tag = I18n.DoNotTranslate;
            btnTest.Text = I18n.Translate("ActionViewer/ctxTestAction", "Test Action");
            btnTest.ContextMenuStrip = new ContextMenuStrip();
            ToolStripItem tsi = btnTest.ContextMenuStrip.Items.Add(I18n.Translate("ActionViewer/ctxTestPlaceholder", "Test action with placeholder values"));
            tsi.Image = btnTest.Image;
            tsi.Click += Tsi_Click1;
            tsi = btnTest.ContextMenuStrip.Items.Add(I18n.Translate("ActionViewer/ctxTestLive", "Test action with live values"));
            tsi.Image = btnTest.Image;
            tsi.Click += Tsi_Click2;
            tsi = btnTest.ContextMenuStrip.Items.Add(I18n.Translate("ActionViewer/ctxTestLiveIgnoreCnd", "Test action with live values (ignore conditions)"));
            tsi.Image = btnTest.Image;
            tsi.Click += Tsi_Click3;
            txtSendKeysLink.Tag = I18n.DoNotTranslate;
            txtObsWebsocketLink.Tag = I18n.DoNotTranslate;
            RestoredSavedDimensions();

            rtbVariableHelper = new RichTextBoxHelper("rtbVariableHelper", this, tableLayoutPanel9);
            rtbLvarHelper = new RichTextBoxHelper("rtbLvarHelper", this, tableLayoutPanel17);
            rtbTvarHelper = new RichTextBoxHelper("rtbTvarHelper", this, tableLayoutPanel21);
            rtbDictHelper = new RichTextBoxHelper("rtbDictHelper", this, tableLayoutPanelDict);
            rtbSendKeysHelper = new RichTextBoxHelper("rtbSendKeysHelper", this, tableLayoutPanel6);
            rtbCallbackHelper = new RichTextBoxHelper("rtbCallbackHelper", this, tableLayoutPanel24);
            rtbWmsgHelper = new RichTextBoxHelper("rtbWmsgHelper", this, tableLayoutPanel19);
            rtbJsonHelper = new RichTextBoxHelper("rtbJsonHelper", this, jsonTableLayout);

            prsScalarTarget.RelatedTextbox = expVariableTarget;
            prsScalarName.RelatedTextbox = expVariableName;
            prsListTarget.RelatedTextbox = expLvarTarget;
            prsListSource.RelatedTextbox = expLvarName;
            prsJsonVariable.RelatedTextbox = expJsonVariable;
            prsFileVariable.RelatedTextbox = expFileOpVariable;
            prsTableTarget.RelatedTextbox = expTvarTarget;
            prsTableSource.RelatedTextbox = expTvarName;
            prsDictSource.RelatedTextbox = expDictName;
            prsDictTarget.RelatedTextbox = expDictTarget;

            expCallbackName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Callback;
            expJsonVariable.AutofillType = ExpressionTextBox.AutofillTypeEnum.Scalar;

            Disposed += ActionForm_Disposed;
            FormClosing += ActionForm_FormClosing;

            txtDescription.GotFocus += ExpressionTextBox.ReplaceIncompleteLineBreaksInClipboard;

            colorSelector1.ColorChanged += colorSelector1_ColorChanged;
            expTextForeColor.TextChanged += expTextForeColor_TextChanged;
            expTextBackColor.TextChanged += expTextBackColor_TextChanged;
            expTextOutlineColor.TextChanged += expTextOutlineColor_TextChanged;

            // finished UI initialization, fill in data

            UiContext = uiContext;
            Images = imageList;
            TreeView = treeView;

            trvTrigger.Nodes.Add((TreeNode)treeView.Nodes[0].Clone());
            CloseTree(trvTrigger.Nodes[0]);

            trvRepositoryLink.Nodes.Add((TreeNode)treeView.Nodes[1].Clone());
            RemoveNonRepositoryNodesFromTree(trvRepositoryLink.Nodes[0]);
            CloseTree(trvRepositoryLink.Nodes[0]);

            trvFolder.Nodes.Add((TreeNode)treeView.Nodes[0].Clone());
            RemoveTriggerNodesFromTree(trvFolder.Nodes[0]);
            CloseTree(trvFolder.Nodes[0]);

            SettingsFromAction(action);
        }

        private void ActionForm_Disposed(object sender, EventArgs e)
        {
            if (ExtEditor != null)
            {
                ExtEditor.Dispose();
                ExtEditor = null;
            }
        }

        private void ActionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ExtEditor != null)
            {
                switch (MessageBox.Show(
                    this,
                    I18n.Translate("internal/ActionForm/exteditorstillup", "External script editor is still running, and any changes made have not yet been reflected to the script code. Ignore changes and close the action editor anyway?"),
                    I18n.Translate("internal/ConfigurationForm/warning", "Warning"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                ))
                {
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                }
            }
            ExpressionTextBox.RegisterAllDynamicVarNamesOnForm(this);
        }

        private void CloseTree(TreeNode tn)
        {
            if (tn.Tag is Folder f)
            {
                tn.ImageIndex = (int)UserInterface.GetImageIndexForClosedFolder(f);
                tn.SelectedImageIndex = tn.ImageIndex;
            }
            foreach (TreeNode tc in tn.Nodes)
            {
                CloseTree(tc);
            }
        }

        private bool RemoveNonRepositoryNodesFromTree(TreeNode tn)
        {
            if (tn == null)
            {
                return true;
            }
            if ((tn.Tag is Repository) == false && tn.Parent != null)
            {
                return true;
            }
            List<TreeNode> rems = new List<TreeNode>();
            foreach (TreeNode tnx in tn.Nodes)
            {
                if (RemoveNonRepositoryNodesFromTree(tnx) == true)
                {
                    rems.Add(tnx);
                }
            }
            foreach (TreeNode tnr in rems)
            {
                tnr.Remove();
            }
            return false;
        }

        private bool RemoveTriggerNodesFromTree(TreeNode tn)
        {
            if (tn == null)
            {
                return true;
            }
            if (tn.Tag is Trigger)
            {
                return true;
            }
            List<TreeNode> rems = new List<TreeNode>();
            foreach (TreeNode tnx in tn.Nodes)
            {
                if (RemoveTriggerNodesFromTree(tnx) == true)
                {
                    rems.Add(tnx);
                }
            }
            foreach (TreeNode tnr in rems)
            {
                tnr.Remove();
            }
            return false;
        }

        private void Tsi_Click3(object sender, EventArgs e)
        {
            TestActionPrepare(liveValues: true, ignoreConditions: true);
        }

        private void Tsi_Click2(object sender, EventArgs e)
        {
            TestActionPrepare(liveValues: true, ignoreConditions: false);
        }

        private void Tsi_Click1(object sender, EventArgs e)
        {
            TestActionPrepare(liveValues: false, ignoreConditions: false);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            btnAuraGuide.Enabled = (expAuraImage.textBox1.Text.Length > 0);
        }

        private RichTextBoxHelper rtbVariableHelper, rtbLvarHelper, rtbTvarHelper, rtbDictHelper, 
            rtbSendKeysHelper, rtbCallbackHelper, rtbWmsgHelper, rtbJsonHelper;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int oldsel = tbcActionSettings.SelectedIndex;
            tbcActionSettings.SelectedIndex = cbxActionType.SelectedIndex;
            if (cbxActionType.SelectedIndex == (int)ActionTypeEnum.Aura
                || cbxActionType.SelectedIndex == (int)ActionTypeEnum.TextAura
                || cbxActionType.SelectedIndex == (int)ActionTypeEnum.NamedCallback)
            {
                timer2.Enabled = true;
                stsMouseHelp.Visible = true;
            }
            else if (oldsel == (int)ActionTypeEnum.Aura ||
                     oldsel == (int)ActionTypeEnum.TextAura ||
                     oldsel == (int)ActionTypeEnum.NamedCallback)
            {
                stsMouseHelp.Visible = false;
                timer2.Enabled = false;
            }
            switch (cbxActionType.SelectedIndex)
            {
                case ((int)ActionTypeEnum.Variable): rtbVariableHelper.Expanded = RichTextBoxHelper._Expanded; break;
                case ((int)ActionTypeEnum.ListVariable): rtbLvarHelper.Expanded = RichTextBoxHelper._Expanded; break;
                case ((int)ActionTypeEnum.TableVariable): rtbTvarHelper.Expanded = RichTextBoxHelper._Expanded; break;
                case ((int)ActionTypeEnum.DictVariable): rtbDictHelper.Expanded = RichTextBoxHelper._Expanded; break;
                case ((int)ActionTypeEnum.KeyPress): rtbSendKeysHelper.Expanded = RichTextBoxHelper._Expanded; break;
                case ((int)ActionTypeEnum.NamedCallback): rtbCallbackHelper.Expanded = RichTextBoxHelper._Expanded; break;
                case ((int)ActionTypeEnum.WindowMessage): rtbWmsgHelper.Expanded = RichTextBoxHelper._Expanded; break;
                case ((int)ActionTypeEnum.GenericJson): rtbJsonHelper.Expanded = RichTextBoxHelper._Expanded; break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                expSoundFile.Expression = openFileDialog1.FileName;
            }
        }

        internal void SetReadOnlyRecursive(Control c)
        {
            if (c is ConditionViewer viewer)
            {
                viewer.SetReadOnly();
                return;
            }
            if (c is CustomControls.ExpressionTextBox)
            {
                ((CustomControls.ExpressionTextBox)c).ReadOnly = true;
                return;
            }
            if (c is TextBoxBase textBox)
            {
                textBox.ReadOnly = true;
                return;
            }
            foreach (Control cc in c.Controls)
            {
                SetReadOnlyRecursive(cc);
            }
            if (c is Panel || c is TabPage || c is GroupBox)
            {
                return;
            }
            c.Enabled = false;
        }

        internal void SetReadOnly(TabPage tp)
        {
            foreach (Control c in tp.Controls)
            {
                SetReadOnlyRecursive(c);
            }
        }

        internal void SetReadOnly()
        {
            IsReadonly = true;
            btnTest.Visible = false;
            btnOk.Enabled = false;
            btnOk.Visible = false;
            btnCancel.Dock = DockStyle.Fill;
            cbxActionType.Enabled = false;
            foreach (TabPage tp in tbcActionSettings.TabPages)
            {
                SetReadOnly(tp);
            }
            SetReadOnly(tabActionCondition);
            SetReadOnly(tabScheduling);
            SetReadOnly(tabDebugging);
            SetReadOnly(tabDescription);
            panel6.Visible = false;
            panel8.Visible = true;
        }

        private void UpdateFontDescription()
        {
            FontInfoContainer fic = (FontInfoContainer)txtTextAuraFont.Tag;
            string desc = fic.Name + ", " + fic.Size + I18n.Translate("internal/ActionForm/textptunit", "pt");
            List<string> ex = new List<string>();
            if ((fic.Effect & ActionOld.TextAuraEffectEnum.Bold) != 0)
            {
                ex.Add(I18n.Translate("internal/ActionForm/textbold", "bold"));
            }
            if ((fic.Effect & ActionOld.TextAuraEffectEnum.Italic) != 0)
            {
                ex.Add(I18n.Translate("internal/ActionForm/textitalic", "italic"));
            }
            if ((fic.Effect & ActionOld.TextAuraEffectEnum.Underline) != 0)
            {
                ex.Add(I18n.Translate("internal/ActionForm/textunderline", "underline"));
            }
            if ((fic.Effect & ActionOld.TextAuraEffectEnum.Strikeout) != 0)
            {
                ex.Add(I18n.Translate("internal/ActionForm/textstrikeout", "strikeout"));
            }
            if (ex.Count > 0)
            {
                desc += " (" + string.Join(", ", ex) + ")";
            }
            txtTextAuraFont.Text = desc;
        }

        /// <summary>Loads the action and initializes the other action pages with their defaults.</summary>
        internal void SettingsFromAction(ActionBase action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            editedAction = action;
            cbxActionType.SelectedIndex = Array.IndexOf(actionTypes, action.GetType());
            expTag.Expression = action.Tag;
            cbxRefireOption1.SelectedIndex = action.RefireInterrupt ? 0 : 1;
            cbxRefireOption2.SelectedIndex = action.RefireRequeue ? 1 : 0;
            expExecutionDelay.Expression = action.ExecutionDelayExpression;
            chkExecuteAsync.Checked = action.Asynchronous;
            cbxLoggingLevel.SelectedIndex = (int)action.DebugLevel;
            txtDescription.Text = action.Description;
            chkOverrideDesc.Checked = action.DescriptionOverride;
            expDescBgColor.Expression = action.DescBgColor;
            expDescTextColor.Expression = action.DescTextColor;
            cndCondition.ConditionToEdit = (ConditionGroup)action.Condition?.Duplicate() ??
                new ConditionGroup { Grouping = ConditionGroup.CndGroupingEnum.Or, Enabled = false };

            // Switching types can expose any page; initialize inactive pages without storing their settings in the action.
            foreach (Type type in actionTypes)
                SettingsFromSpecificAction(type == action.GetType() ? action : CreateEditorAction(type));

            chkProcessLog_CheckedChanged(null, null);
        }

        /// <summary>Loads the settings belonging to one concrete action type.</summary>
        private void SettingsFromSpecificAction(ActionBase action)
        {
            switch (action)
            {
                case ActionBeep a:
                {
                    expBeepFrequency.Expression = a.Frequency;
                    expBeepLength.Expression = a.Duration;
                    break;
                }
                case ActionPlaySound a:
                {
                    expSoundFile.Expression = a.Filename;
                    expSoundVolume.Expression = a.Volume;
                    chkSoundExclusive.Checked = a.ExclusivePlayer;
                    cbxSoundMethod.SelectedIndex = (int)a.AudioTarget;
                    break;
                }
                case ActionPlaySpeech a:
                {
                    expTextToSay.Expression = a.Message;
                    expSpeechVolume.Expression = a.Volume;
                    expSpeechRate.Expression = a.Rate;
                    chkSpeechExclusive.Checked = a.ExclusivePlayer;
                    cbxTtsMethod.SelectedIndex = (int)a.AudioTarget;
                    break;
                }
                case ActionVariableScalar a:
                {
                    expVariableExpression.Expression = a.Value;
                    expVariableName.Expression = a.Name;
                    expVariableTarget.Expression = a.JsonTargetName;
                    cbxVariableOp.SelectedIndex = (int)a.Operation;
                    prsScalarName.IsPersistent = a.Persistent;
                    prsScalarTarget.IsPersistent = a.JsonTargetPersistent;
                    break;
                }
                case ActionVariableList a:
                {
                    expLvarIndex.Expression = a.Index;
                    expLvarName.Expression = a.Name;
                    expLvarTarget.Expression = a.Target;
                    expLvarValue.Expression = a.ValueExpression;
                    cbxLvarExpType.SelectedIndex = (int)a.ValueType;
                    cbxLvarOperation.SelectedIndex = (int)a.Operation;
                    prsListSource.IsPersistent = a.Persistent;
                    prsListTarget.IsPersistent = a.TargetPersistent;
                    break;
                }
                case ActionVariableTable a:
                {
                    cbxTvarOpType.SelectedIndex = (int)a.Operation;
                    cbxTvarExpType.SelectedIndex = (int)a.ValueType;
                    expTvarColumn.Expression = a.X;
                    expTvarName.Expression = a.Name;
                    expTvarRow.Expression = a.Y;
                    expTvarTarget.Expression = a.TargetName;
                    expTvarValue.Expression = a.Value;
                    prsTableSource.IsPersistent = a.Persistent;
                    prsTableTarget.IsPersistent = a.TargetPersistent;
                    break;
                }
                case ActionVariableDict a:
                {
                    cbxDictOpType.SelectedIndex = (int)a.Operation;
                    cbxDictKeyType.SelectedIndex = (int)a.KeyType;
                    cbxDictValueType.SelectedIndex = (int)a.ValueType;
                    expDictLength.Expression = a.VariableLength;
                    expDictKey.Expression = a.Key;
                    expDictValue.Expression = a.Value;
                    expDictName.Expression = a.Name;
                    expDictTarget.Expression = a.TargetVariable;
                    prsDictSource.IsPersistent = a.Persistent;
                    prsDictTarget.IsPersistent = a.TargetPersistent;
                    break;
                }
                case ActionMessageBox a:
                {
                    cbxMessageBoxIcon.SelectedIndex = ((int)a.Icon) / 16;
                    expMessageBoxText.Expression = a.Text;
                    break;
                }
                case ActionLogMessage a:
                {
                    chkProcessLog.Checked = a.ProcessAsLogline;
                    chkProcessLogACT.Checked = a.AddToACTEncounter;
                    cbxLogMessageTarget.SelectedIndex = (int)a.Target;
                    expLogMessageText.Expression = a.Message;
                    cbxLogMessageLevel.SelectedIndex = (int)a.Level;
                    break;
                }
                case ActionOverlayText a:
                {
                    cbxTextAuraOp.SelectedIndex = (int)a.Operation;
                    cbxTextAuraAlignment.SelectedIndex = (int)a.Alignment;
                    expTextAuraName.Expression = a.Name;
                    expTextAuraText.Expression = a.Text;
                    expTextAuraXIni.Expression = a.XIniExpression;
                    expTextAuraYIni.Expression = a.YIniExpression;
                    expTextAuraWIni.Expression = a.WIniExpression;
                    expTextAuraHIni.Expression = a.HIniExpression;
                    expTextAuraOIni.Expression = a.OIniExpression;
                    expTextAuraXTick.Expression = a.XTickExpression;
                    expTextAuraYTick.Expression = a.YTickExpression;
                    expTextAuraWTick.Expression = a.WTickExpression;
                    expTextAuraHTick.Expression = a.HTickExpression;
                    expTextAuraOTick.Expression = a.OTickExpression;
                    expTextAuraTTLTick.Expression = a.TTLTickExpression;
                    FontInfoContainer fic = new FontInfoContainer
                    {
                        Name = a.Font,
                        Size = a.FontSize,
                        Effect = (ActionOld.TextAuraEffectEnum)a.Effect
                    };
                    txtTextAuraFont.Tag = fic;
                    try
                    {
                        colorSelector1.TextColor = ExpressionTextBox.ParseColor(UiContext.EvaluateStringExpression(null, UiContext, a.ForeColor), Color.Black);
                    }
                    catch { colorSelector1.TextColor = Color.Black; }

                    try
                    {
                        colorSelector1.TextOutlineColor = ExpressionTextBox.ParseColor(UiContext.EvaluateStringExpression(null, UiContext, a.OutlineColor), Color.Empty);
                    }
                    catch { colorSelector1.TextOutlineColor = Color.Empty; }

                    try
                    {
                        colorSelector1.BackgroundColor = ExpressionTextBox.ParseColor(UiContext.EvaluateStringExpression(null, UiContext, a.BackColor), Color.Transparent);
                    }
                    catch { colorSelector1.BackgroundColor = Color.Transparent; }
                    expTextForeColor.Text = a.ForeColor;
                    expTextOutlineColor.Text = a.OutlineColor;
                    expTextBackColor.Text = a.BackColor;
                    UpdateFontDescription();
                    break;
                }
                case ActionOverlayImage a:
                {
                    cbxAuraOp.SelectedIndex = (int)a.Operation;
                    switch (a.SizeMode)
                    {
                        case PictureBoxSizeMode.Normal:
                            cbxAuraDisplay.SelectedIndex = 0;
                            break;
                        case PictureBoxSizeMode.StretchImage:
                            cbxAuraDisplay.SelectedIndex = 1;
                            break;
                        case PictureBoxSizeMode.CenterImage:
                            cbxAuraDisplay.SelectedIndex = 2;
                            break;
                        case PictureBoxSizeMode.Zoom:
                            cbxAuraDisplay.SelectedIndex = 3;
                            break;
                    }
                    expAuraName.Expression = a.Name;
                    expAuraImage.Expression = a.Filename;
                    expAuraXIni.Expression = a.XIniExpression;
                    expAuraYIni.Expression = a.YIniExpression;
                    expAuraWIni.Expression = a.WIniExpression;
                    expAuraHIni.Expression = a.HIniExpression;
                    expAuraOIni.Expression = a.OIniExpression;
                    expAuraXTick.Expression = a.XTickExpression;
                    expAuraYTick.Expression = a.YTickExpression;
                    expAuraWTick.Expression = a.WTickExpression;
                    expAuraHTick.Expression = a.HTickExpression;
                    expAuraOTick.Expression = a.OTickExpression;
                    expAuraTTLTick.Expression = a.TTLTickExpression;
                    break;
                }
                case ActionMouse a:
                {
                    cbxMouseOp.SelectedIndex = (int)a.Operation;
                    cbxMouseCoord.SelectedIndex = (int)a.Coordinate;
                    expMouseX.Expression = a.X;
                    expMouseY.Expression = a.Y;
                    break;
                }
                case ActionKeypress a:
                {
                    cbxKeypressMethod.SelectedIndex = (int)a.Operation;
                    expKeypresses.Expression = a.Keypress;
                    expKeypress.Expression = a.Keycode;
                    expWindowTitle.Expression = a.WindowTitle;
                    expKeypressProcId.Expression = a.ProcessId;
                    break;
                }
                case ActionNamedCallback a:
                {
                    expCallbackName.Expression = a.Name;
                    expCallbackParam.Expression = a.Parameter;
                    break;
                }
                case ActionWindowMessage a:
                {
                    expWmsgProcid.Expression = a.ProcessId;
                    expWmsgTitle.Expression = a.WindowTitle;
                    expWmsgCode.Expression = a.MessageId;
                    expWmsgWparam.Expression = a.Wparam;
                    expWmsgLparam.Expression = a.Lparam;
                    break;
                }
                case ActionDiskOperation a:
                {
                    expFileOpName.Expression = a.Filename;
                    expFileOpVariable.Expression = a.Variable;
                    cbxFileOpType.SelectedIndex = (int)a.Operation;
                    cbxFileOpCache.Checked = a.UseCache;
                    prsFileVariable.IsPersistent = a.Persistent;
                    break;
                }
                case ActionLaunchProcess a:
                {
                    expProcessName.Expression = a.Path;
                    expProcessParameters.Expression = a.Arguments;
                    expProcessWorkingDir.Expression = a.WorkingDirectory;
                    cbxProcessWindowStyle.SelectedIndex = (int)a.WindowStyle;
                    break;
                }
                case ActionExecuteScript a:
                {
                    expExecScriptCode.Expression = a.Script;
                    expExecScriptAssemblies.Expression = a.Assemblies;
                    break;
                }
                case ActionMutex a:
                {
                    cbxMutexOp.SelectedIndex = (int)a.Operation;
                    expMutexName.Expression = a.Name;
                    break;
                }
                case ActionLoop a:
                {
                    cndLoopCondition.ConditionToEdit = (ConditionGroup)a.LoopCondition?.Duplicate() ?? new ConditionGroup { Grouping = ConditionGroup.CndGroupingEnum.Or, Enabled = false };
                    loopActionViewer.Actions = a.LoopActions.OrderBy(child => child.OrderNumber).Select(child => child.Copy()).ToList();
                    loopActionViewer.RefreshDgv();
                    expLoopIterationDelay.Expression = a.DelayExpression;
                    expLoopIncr.Expression = a.IncrExpression;
                    expLoopInit.Expression = a.InitExpression;
                    break;
                }
                case ActionJsonRequest a:
                {
                    cbxJsonType.SelectedIndex = (int)a.Method;
                    expJsonEndpoint.Expression = a.Endpoint;
                    expJsonFiring.Expression = a.FiringExpression;
                    expJsonPayload.Expression = a.Payload;
                    expJsonHeaders.Expression = a.Headers;
                    cbxJsonCache.Checked = a.UseCache;
                    prsJsonVariable.IsPersistent = a.Persistent;
                    expJsonVariable.Expression = a.ResultVariable;
                    break;
                }
                case ActionDiscordWebhook a:
                {
                    expDiscordUrl.Expression = a.WebhookURL;
                    expDiscordMessage.Expression = a.Message;
                    cbxDiscordTts.Checked = a.UseTTS;
                    break;
                }
                case ActionLiveSplitControl a:
                {
                    cbxLsOpType.SelectedIndex = (int)a.Operation;
                    expLSCustPayload.Expression = a.CustomPayload;
                    break;
                }
                case ActionObsControl a:
                {
                    cbxObsOpType.SelectedIndex = (int)a.Operation;
                    expObsEndpoint.Expression = a.Endpoint;
                    expObsPassword.Expression = a.Password;
                    expObsSceneName.Expression = a.SceneName;
                    expObsSourceName.Expression = a.SourceName;
                    expObsJSONPayload.Expression = a.JSONPayload;
                    break;
                }
                case ActionActInteraction a:
                {
                    cbxActOpType.SelectedIndex = (int)a.Operation;
                    cbxActOpBoolParam.SelectedIndex = a.BoolParam ? 1 : 0;
                    expActOpStringParam.Expression = a.StringParam;
                    break;
                }
                case ActionTriggerOperation a:
                {
                    cbxTriggerOp.SelectedIndex = (int)a.Operation;
                    expTriggerZone.Expression = a.Zone;
                    expTriggerText.Expression = a.Text;
                    expTriggerTagRegex.Expression = a.TagRegex;
                    cbxTriggerZoneType.SelectedIndex = (int)a.ZoneType;
                    for (int i = 0; i < cbxFiringOptions.Items.Count; i++)
                        cbxFiringOptions.SetItemChecked(i, ((int)a.Force & (1 << i)) != 0);
                    TreeNode node = Plugin.LocateNodeHostingTriggerId(trvTrigger.Nodes[0], a.TriggerId, null);
                    trvTrigger.SelectedNode = node;
                    node?.EnsureVisible();
                    break;
                }
                case ActionFolderOperation a:
                {
                    cbxFolderOp.SelectedIndex = (int)a.Operation;
                    TreeNode node = Plugin.LocateNodeHostingFolderId(trvFolder.Nodes[0], a.FolderId, null);
                    trvFolder.SelectedNode = node;
                    node?.EnsureVisible();
                    break;
                }
                case ActionRepository a:
                {
                    cbxRepositoryOp.SelectedIndex = (int)a.Operation;
                    TreeNode node = Plugin.LocateNodeHostingRepositoryId(trvRepositoryLink.Nodes[0], a.RepositoryId);
                    trvRepositoryLink.SelectedNode = node;
                    node?.EnsureVisible();
                    break;
                }
                case ActionPlaceholder _:
                    break;
            }
        }

        /// <summary>Creates the selected typed action without modifying the edited object.</summary>
        internal ActionBase SettingsToAction()
        {
            Type type = actionTypes[cbxActionType.SelectedIndex];
            ActionBase action;
            if (type == editedAction.GetType())
            {
                action = editedAction.Copy();
            }
            else
            {
                action = CreateEditorAction(type);
                editedAction.CopyCommonPropertiesTo(action);
            }
            action.ParentTrigger = ParentTrigger;
            action.Tag = expTag.Expression;
            action.RefireInterrupt = cbxRefireOption1.SelectedIndex == 0;
            action.RefireRequeue = cbxRefireOption2.SelectedIndex == 1;
            action.ExecutionDelayExpression = expExecutionDelay.Expression;
            action.Asynchronous = chkExecuteAsync.Checked;
            action.DebugLevel = (RealPlugin.DebugLevelEnum)cbxLoggingLevel.SelectedIndex;
            action.Description = txtDescription.Text;
            action.DescriptionOverride = chkOverrideDesc.Checked;
            action.DescBgColor = expDescBgColor.Expression;
            action.DescTextColor = expDescTextColor.Expression;
            action.Condition = (ConditionGroup)cndCondition.ConditionToEdit.Duplicate();
            SettingsToSpecificAction(action);
            ActionBase.RebuildParentage(new[] { action }, ParentTrigger);
            return action;
        }

        /// <summary>Creates a draft using the existing action editor's defaults.</summary>
        private static ActionBase CreateEditorAction(Type type)
        {
            // The editor starts OBS credentials empty instead of using the action class's constant templates.
            if (type == typeof(ActionObsControl))
                return new ActionObsControl { Endpoint = "", Password = "" };
            return (ActionBase)Activator.CreateInstance(type);
        }

        /// <summary>Reads only the selected action page into its concrete action type.</summary>
        private void SettingsToSpecificAction(ActionBase action)
        {
            switch (action)
            {
                case ActionBeep a:
                {
                    a.Frequency = expBeepFrequency.Expression;
                    a.Duration = expBeepLength.Expression;
                    break;
                }
                case ActionPlaySound a:
                {
                    a.Filename = expSoundFile.Expression;
                    a.Volume = expSoundVolume.Expression;
                    a.ExclusivePlayer = chkSoundExclusive.Checked;
                    a.AudioTarget = (Configuration.AudioRoutingMethodEnum)cbxSoundMethod.SelectedIndex;
                    break;
                }
                case ActionPlaySpeech a:
                {
                    a.Message = expTextToSay.Expression;
                    a.Volume = expSpeechVolume.Expression;
                    a.Rate = expSpeechRate.Expression;
                    a.ExclusivePlayer = chkSpeechExclusive.Checked;
                    a.AudioTarget = (Configuration.AudioRoutingMethodEnum)cbxTtsMethod.SelectedIndex;
                    break;
                }
                case ActionVariableScalar a:
                {
                    a.Value = expVariableExpression.Expression;
                    a.Name = expVariableName.Expression;
                    a.JsonTargetName = expVariableTarget.Expression;
                    a.Operation = (ActionVariableScalar.OperationEnum)cbxVariableOp.SelectedIndex;
                    a.Persistent = prsScalarName.IsPersistent;
                    a.JsonTargetPersistent = prsScalarTarget.IsPersistent;
                    break;
                }
                case ActionVariableList a:
                {
                    a.ValueExpression = expLvarValue.Expression;
                    a.ValueType = (ActionVariableList.ExpressionTypeEnum)cbxLvarExpType.SelectedIndex;
                    a.Index = expLvarIndex.Expression;
                    a.Name = expLvarName.Expression;
                    a.Operation = (ActionVariableList.OperationEnum)cbxLvarOperation.SelectedIndex;
                    a.Target = expLvarTarget.Expression;
                    a.Persistent = prsListSource.IsPersistent;
                    a.TargetPersistent = prsListTarget.IsPersistent;
                    break;
                }
                case ActionVariableTable a:
                {
                    a.Operation = (ActionVariableTable.OperationEnum)cbxTvarOpType.SelectedIndex;
                    a.ValueType = (ActionVariableTable.ExpressionTypeEnum)cbxTvarExpType.SelectedIndex;
                    a.X = expTvarColumn.Expression;
                    a.Name = expTvarName.Expression;
                    a.Y = expTvarRow.Expression;
                    a.TargetName = expTvarTarget.Expression;
                    a.Value = expTvarValue.Expression;
                    a.Persistent = prsTableSource.IsPersistent;
                    a.TargetPersistent = prsTableTarget.IsPersistent;
                    break;
                }
                case ActionVariableDict a:
                {
                    a.Operation = (ActionVariableDict.OperationEnum)cbxDictOpType.SelectedIndex;
                    a.KeyType = (ActionVariableDict.ExpressionTypeEnum)cbxDictKeyType.SelectedIndex;
                    a.ValueType = (ActionVariableDict.ExpressionTypeEnum)cbxDictValueType.SelectedIndex;
                    a.VariableLength = expDictLength.Expression;
                    a.Key = expDictKey.Expression;
                    a.Value = expDictValue.Expression;
                    a.Name = expDictName.Expression;
                    a.TargetVariable = expDictTarget.Expression;
                    a.Persistent = prsDictSource.IsPersistent;
                    a.TargetPersistent = prsDictTarget.IsPersistent;
                    break;
                }
                case ActionMessageBox a:
                {
                    a.Icon = (ActionMessageBox.MessageBoxIconEnum)(cbxMessageBoxIcon.SelectedIndex * 16);
                    a.Text = expMessageBoxText.Expression;
                    break;
                }
                case ActionLogMessage a:
                {
                    a.ProcessAsLogline = chkProcessLog.Checked;
                    a.AddToACTEncounter = chkProcessLogACT.Checked;
                    a.Target = (LogEvent.SourceEnum)cbxLogMessageTarget.SelectedIndex;
                    a.Message = expLogMessageText.Expression;
                    a.Level = (ActionLogMessage.LogLevelEnum)cbxLogMessageLevel.SelectedIndex;
                    break;
                }
                case ActionOverlayText a:
                {
                    FontInfoContainer fic = (FontInfoContainer)txtTextAuraFont.Tag;
                    a.Alignment = (ActionOverlayText.TextAlignmentEnum)cbxTextAuraAlignment.SelectedIndex;
                    a.Operation = (ActionOverlayText.OperationEnum)cbxTextAuraOp.SelectedIndex;
                    a.Name = expTextAuraName.Expression;
                    a.Text = expTextAuraText.Expression;
                    a.XIniExpression = expTextAuraXIni.Expression;
                    a.YIniExpression = expTextAuraYIni.Expression;
                    a.WIniExpression = expTextAuraWIni.Expression;
                    a.HIniExpression = expTextAuraHIni.Expression;
                    a.OIniExpression = expTextAuraOIni.Expression;
                    a.XTickExpression = expTextAuraXTick.Expression;
                    a.YTickExpression = expTextAuraYTick.Expression;
                    a.WTickExpression = expTextAuraWTick.Expression;
                    a.HTickExpression = expTextAuraHTick.Expression;
                    a.OTickExpression = expTextAuraOTick.Expression;
                    a.TTLTickExpression = expTextAuraTTLTick.Expression;
                    a.Font = fic.Name;
                    a.FontSize = fic.Size;
                    a.Effect = (ActionOverlayText.EffectEnum)fic.Effect;
                    a.ForeColor = expTextForeColor.Text;
                    a.BackColor = expTextBackColor.Text;
                    a.OutlineColor = expTextOutlineColor.Text;
                    break;
                }
                case ActionOverlayImage a:
                {
                    a.Operation = (ActionOverlayImage.OperationEnum)cbxAuraOp.SelectedIndex;
                    switch (cbxAuraDisplay.SelectedIndex)
                    {
                        case 0:
                            a.SizeMode = PictureBoxSizeMode.Normal;
                            break;
                        case 1:
                            a.SizeMode = PictureBoxSizeMode.StretchImage;
                            break;
                        case 2:
                            a.SizeMode = PictureBoxSizeMode.CenterImage;
                            break;
                        case 3:
                            a.SizeMode = PictureBoxSizeMode.Zoom;
                            break;
                    }
                    a.Name = expAuraName.Expression;
                    a.Filename = expAuraImage.Expression;
                    a.XIniExpression = expAuraXIni.Expression;
                    a.YIniExpression = expAuraYIni.Expression;
                    a.WIniExpression = expAuraWIni.Expression;
                    a.HIniExpression = expAuraHIni.Expression;
                    a.OIniExpression = expAuraOIni.Expression;
                    a.XTickExpression = expAuraXTick.Expression;
                    a.YTickExpression = expAuraYTick.Expression;
                    a.WTickExpression = expAuraWTick.Expression;
                    a.HTickExpression = expAuraHTick.Expression;
                    a.OTickExpression = expAuraOTick.Expression;
                    a.TTLTickExpression = expAuraTTLTick.Expression;
                    break;
                }
                case ActionMouse a:
                {
                    a.Operation = (ActionMouse.OperationEnum)cbxMouseOp.SelectedIndex;
                    a.Coordinate = (ActionMouse.CoordinateEnum)cbxMouseCoord.SelectedIndex;
                    a.X = expMouseX.Expression;
                    a.Y = expMouseY.Expression;
                    break;
                }
                case ActionKeypress a:
                {
                    a.Keypress = expKeypresses.Expression;
                    a.Operation = (ActionKeypress.OperationEnum)cbxKeypressMethod.SelectedIndex;
                    a.Keycode = expKeypress.Expression;
                    a.ProcessId = expKeypressProcId.Expression;
                    a.WindowTitle = expWindowTitle.Expression;
                    break;
                }
                case ActionNamedCallback a:
                {
                    a.Name = expCallbackName.Expression;
                    a.Parameter = expCallbackParam.Expression;
                    break;
                }
                case ActionWindowMessage a:
                {
                    a.ProcessId = expWmsgProcid.Expression;
                    a.WindowTitle = expWmsgTitle.Expression;
                    a.MessageId = expWmsgCode.Expression;
                    a.Wparam = expWmsgWparam.Expression;
                    a.Lparam = expWmsgLparam.Expression;
                    break;
                }
                case ActionDiskOperation a:
                {
                    a.Operation = (ActionDiskOperation.OperationEnum)cbxFileOpType.SelectedIndex;
                    a.Filename = expFileOpName.Expression;
                    a.Variable = expFileOpVariable.Expression;
                    a.UseCache = cbxFileOpCache.Checked;
                    a.Persistent = prsFileVariable.IsPersistent;
                    break;
                }
                case ActionLaunchProcess a:
                {
                    a.Path = expProcessName.Expression;
                    a.Arguments = expProcessParameters.Expression;
                    a.WorkingDirectory = expProcessWorkingDir.Expression;
                    a.WindowStyle = (System.Diagnostics.ProcessWindowStyle)cbxProcessWindowStyle.SelectedIndex;
                    break;
                }
                case ActionExecuteScript a:
                {
                    a.Script = expExecScriptCode.Expression;
                    a.Assemblies = expExecScriptAssemblies.Expression;
                    break;
                }
                case ActionMutex a:
                {
                    a.Operation = (ActionMutex.OperationEnum)cbxMutexOp.SelectedIndex;
                    a.Name = expMutexName.Expression;
                    break;
                }
                case ActionLoop a:
                {
                    a.LoopCondition = (ConditionGroup)cndLoopCondition.ConditionToEdit.Duplicate();
                    a.LoopActions = loopActionViewer.Actions.OrderBy(child => child.OrderNumber).Select(child => child.Copy()).ToList();
                    a.DelayExpression = expLoopIterationDelay.Expression;
                    a.IncrExpression = expLoopIncr.Expression;
                    a.InitExpression = expLoopInit.Expression;
                    break;
                }
                case ActionJsonRequest a:
                {
                    a.Method = (ActionJsonRequest.MethodEnum)cbxJsonType.SelectedIndex;
                    a.Endpoint = expJsonEndpoint.Expression;
                    a.FiringExpression = expJsonFiring.Expression;
                    a.Headers = expJsonHeaders.Expression;
                    a.Payload = expJsonPayload.Expression;
                    a.UseCache = cbxJsonCache.Checked;
                    a.Persistent = prsJsonVariable.IsPersistent;
                    a.ResultVariable = expJsonVariable.Expression;
                    break;
                }
                case ActionDiscordWebhook a:
                {
                    a.Message = expDiscordMessage.Expression;
                    a.WebhookURL = expDiscordUrl.Expression;
                    a.UseTTS = cbxDiscordTts.Checked;
                    break;
                }
                case ActionLiveSplitControl a:
                {
                    a.Operation = (ActionLiveSplitControl.OperationEnum)cbxLsOpType.SelectedIndex;
                    a.CustomPayload = expLSCustPayload.Expression;
                    break;
                }
                case ActionObsControl a:
                {
                    a.Operation = (ActionObsControl.OperationEnum)cbxObsOpType.SelectedIndex;
                    a.Endpoint = expObsEndpoint.Expression;
                    a.Password = expObsPassword.Expression;
                    a.SceneName = expObsSceneName.Expression;
                    a.SourceName = expObsSourceName.Expression;
                    a.JSONPayload = expObsJSONPayload.Expression;
                    break;
                }
                case ActionActInteraction a:
                {
                    a.Operation = (ActionActInteraction.OperationEnum)cbxActOpType.SelectedIndex;
                    a.BoolParam = cbxActOpBoolParam.SelectedIndex == 1;
                    a.StringParam = expActOpStringParam.Expression;
                    break;
                }
                case ActionTriggerOperation a:
                {
                    a.Operation = (ActionTriggerOperation.OperationEnum)cbxTriggerOp.SelectedIndex;
                    a.Text = expTriggerText.Expression;
                    a.TagRegex = expTriggerTagRegex.Expression;
                    a.Zone = expTriggerZone.Expression;
                    a.ZoneType = (ActionTriggerOperation.ZoneTypeEnum)cbxTriggerZoneType.SelectedIndex;
                    a.TriggerId = (trvTrigger.SelectedNode?.Tag as Trigger)?.Id ?? Guid.Empty;
                    a.Force = ActionTriggerOperation.ForceEnum.NoSkip;
                    foreach (int index in cbxFiringOptions.CheckedIndices)
                        a.Force |= (ActionTriggerOperation.ForceEnum)(1 << index);
                    break;
                }
                case ActionFolderOperation a:
                {
                    a.Operation = (ActionFolderOperation.OperationEnum)cbxFolderOp.SelectedIndex;
                    a.FolderId = (trvFolder.SelectedNode?.Tag as Folder)?.Id ?? Guid.Empty;
                    break;
                }
                case ActionRepository a:
                {
                    a.Operation = (ActionRepository.OperationEnum)cbxRepositoryOp.SelectedIndex;
                    a.RepositoryId = (trvRepositoryLink.SelectedNode?.Tag as Repository)?.Id ?? Guid.Empty;
                    break;
                }
                case ActionPlaceholder _:
                    break;
            }
        }

        private void TestAction(bool liveValues, bool ignoreConditions = false)
        {
            ActionOld a = SettingsToAction().ConvertToOldAction();
            if (ignoreConditions) a.Condition = new ConditionGroup();

            Context ctx = new Context(a.ParentTrigger);
            ctx.testByPlaceholder = liveValues == false;
            ctx.soundhook = Plugin.SoundPlaybackSmart;
            ctx.ttshook = Plugin.TtsPlaybackSmart;
            a.Execute(null, ctx);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                expProcessName.Expression = openFileDialog2.FileName;
            }
        }

        private void cbxVariableOp_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxVariableOp.SelectedIndex)
            {
                case (int)ActionVariableScalar.OperationEnum.Unset:
                    expVariableExpression.Enabled = false;
                    expVariableName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case (int)ActionVariableScalar.OperationEnum.SetString:
                    expVariableExpression.Enabled = true;
                    expVariableName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case (int)ActionVariableScalar.OperationEnum.SetNumeric:
                    expVariableExpression.Enabled = true;
                    expVariableName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case (int)ActionVariableScalar.OperationEnum.Increment:
                    expVariableExpression.Enabled = true;
                    expVariableName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case (int)ActionVariableScalar.OperationEnum.Clipboard:
                    expVariableExpression.Enabled = true;
                    expVariableName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case (int)ActionVariableScalar.OperationEnum.UnsetAll:
                    expVariableExpression.Enabled = false;
                    expVariableName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case (int)ActionVariableScalar.OperationEnum.UnsetRegex:
                    expVariableExpression.Enabled = false;
                    expVariableName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
                    break;
                case (int)ActionVariableScalar.OperationEnum.UnsetRegexUniversal:
                    expVariableExpression.Enabled = false;
                    expVariableName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
                    break;
                case (int)ActionVariableScalar.OperationEnum.QueryJsonPath:
                    expVariableExpression.Enabled = true;
                    expVariableName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case (int)ActionVariableScalar.OperationEnum.QueryJsonPathList:
                    expVariableExpression.Enabled = true;
                    expVariableName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
            }
            expVariableName.Enabled = (cbxVariableOp.SelectedIndex != (int)ActionVariableScalar.OperationEnum.UnsetAll);
            prsScalarName.Enabled = (cbxVariableOp.SelectedIndex == (int)ActionVariableScalar.OperationEnum.UnsetAll)
                                  ? true : expVariableName.Enabled;
            expVariableTarget.Enabled = (cbxVariableOp.SelectedIndex == (int)ActionVariableScalar.OperationEnum.QueryJsonPath
                                      || cbxVariableOp.SelectedIndex == (int)ActionVariableScalar.OperationEnum.QueryJsonPathList);
            prsScalarTarget.Enabled = expVariableTarget.Enabled;

            expVariableExpression.ExpressionType = (cbxVariableOp.SelectedIndex == (int)ActionVariableScalar.OperationEnum.SetNumeric
                                                 || cbxVariableOp.SelectedIndex == (int)ActionVariableScalar.OperationEnum.Increment)
                                     ? ExpressionTextBox.SupportedExpressionTypeEnum.Numeric
                                     : ExpressionTextBox.SupportedExpressionTypeEnum.String;
            expVariableName.AutofillType = expVariableName.ExpressionType != ExpressionTextBox.SupportedExpressionTypeEnum.Regex
                ? ExpressionTextBox.AutofillTypeEnum.Scalar
                : ExpressionTextBox.AutofillTypeEnum.None;
            expVariableTarget.AutofillType = cbxVariableOp.SelectedIndex != (int)ActionVariableScalar.OperationEnum.QueryJsonPathList
                ? ExpressionTextBox.AutofillTypeEnum.Scalar
                : ExpressionTextBox.AutofillTypeEnum.List;
            rtbVariableHelper.UpdateText();
        }

        private void cbxTriggerOp_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isFire = cbxTriggerOp.SelectedIndex == (int)ActionTriggerOperation.OperationEnum.FireTrigger;
            cbxFiringOptions.Enabled = isFire;

            var isRegexIgnored = cbxFiringOptions.CheckedIndices.Contains(0);
            var isEventTextEnabled = isFire && !isRegexIgnored;
            expTriggerText.Enabled = isEventTextEnabled;
            expTriggerZone.Enabled = isEventTextEnabled;
            cbxTriggerZoneType.Enabled = isEventTextEnabled;

            var isCancel = cbxTriggerOp.SelectedIndex == (int)ActionTriggerOperation.OperationEnum.CancelTrigger;
            var isCancelAll = cbxTriggerOp.SelectedIndex == (int)ActionTriggerOperation.OperationEnum.CancelAllTrigger;
            expTriggerTagRegex.Enabled = isCancel || isCancelAll;
            trvTrigger.Enabled = !isCancelAll;
        }

        private void cbxFileOpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxFileOpType.SelectedIndex)
            {
                case (int)ActionDiskOperation.OperationEnum.ReadIntoVariable:
                    expFileOpVariable.AutofillType = ExpressionTextBox.AutofillTypeEnum.Scalar; 
                    break;
                case (int)ActionDiskOperation.OperationEnum.ReadIntoListVariable:
                    expFileOpVariable.AutofillType = ExpressionTextBox.AutofillTypeEnum.List; 
                    break;
                case (int)ActionDiskOperation.OperationEnum.ReadCSVIntoTableVariable:
                    expFileOpVariable.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table; 
                    break;
            }
        }

        private void cbxFiringOptions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool nv = cbxFiringOptions.CheckedIndices.Contains(0);
            if (e.Index == 0)
            {
                nv = e.NewValue == CheckState.Checked ? true : false;
            }
            expTriggerText.Enabled = (cbxTriggerOp.SelectedIndex == 0 && nv == false);
            expTriggerZone.Enabled = (cbxTriggerOp.SelectedIndex == 0 && nv == false);
        }

        private void trvTrigger_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is Folder)
            {
                e.Cancel = true;
            }
        }

        private void trvTrigger_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is Folder)
            {
                e.Node.ImageIndex = (int)CustomControls.UserInterface.GetImageIndexForClosedFolder((Folder)e.Node.Tag);
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void trvTrigger_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is Folder)
            {
                e.Node.ImageIndex = (int)CustomControls.UserInterface.GetImageIndexForOpenFolder((Folder)e.Node.Tag);
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            TestActionPrepare(Plugin.cfg.TestLiveByDefault, Plugin.cfg.TestIgnoreConditionsByDefault);
        }

        private void TestActionPrepare(bool liveValues, bool ignoreConditions)
        {
            if (cbxActionType.SelectedIndex == (int)ActionTypeEnum.KeyPress && cbxKeypressMethod.SelectedIndex == (int)ActionKeypress.OperationEnum.SendKeys)
            {
                if (DialogResult.OK == MessageBox.Show(this, I18n.Translate("internal/ActionForm/confirmkeypress", "The keypresses you defined will be sent in two seconds after you hit OK, to allow you to change to the window you want to send the keypresses to."), I18n.Translate("internal/ActionForm/confirmkeypressdelay", "Confirm"), MessageBoxButtons.OK, MessageBoxIcon.Information))
                {
                    Timer delayTimer = new Timer() { Interval = 2000 };
                    delayTimer.Tick += (s, e) =>
                    {
                        delayTimer.Stop();
                        TestAction(liveValues, ignoreConditions);
                    };
                    delayTimer.Start();
                }
                return;
            }
            if (cbxActionType.SelectedIndex == (int)ActionTypeEnum.Aura)
            {
                if (cbxAuraOp.SelectedIndex == (int)ActionOverlayImage.OperationEnum.Activate)
                {
                    timer1.Stop();
                    timer1.Tag = 0;
                    timer1.Start();
                }
            }
            TestAction(liveValues, ignoreConditions);
        }

        private void cbxAuraOp_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxAuraOp.SelectedIndex)
            {
                case (int)ActionOverlayImage.OperationEnum.Activate:
                case (int)ActionOverlayImage.OperationEnum.Deactivate:
                case (int)ActionOverlayImage.OperationEnum.DeactivateAll:
                    expAuraName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expAuraName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Image;
                    break;
                case (int)ActionOverlayImage.OperationEnum.DeactivateRegex:
                    expAuraName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
                    expAuraName.AutofillType = ExpressionTextBox.AutofillTypeEnum.None;
                    break;
            }
            bool enabled = (cbxAuraOp.SelectedIndex == 0);
            btnBrowseAura.Enabled = enabled;
            cbxAuraDisplay.Enabled = enabled;
            expAuraImage.Enabled = enabled;
            expAuraXIni.Enabled = enabled;
            expAuraYIni.Enabled = enabled;
            expAuraWIni.Enabled = enabled;
            expAuraHIni.Enabled = enabled;
            expAuraOIni.Enabled = enabled;
            expAuraXTick.Enabled = enabled;
            expAuraYTick.Enabled = enabled;
            expAuraWTick.Enabled = enabled;
            expAuraHTick.Enabled = enabled;
            expAuraOTick.Enabled = enabled;
            expAuraTTLTick.Enabled = enabled;
            btnAuraGuide.Enabled = enabled;
            expAuraName.Enabled = (cbxAuraOp.SelectedIndex != 2);
            btnHide.Enabled = (cbxAuraOp.SelectedIndex != 2);
        }

        private void expExecutionDelay_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                expAuraImage.Expression = openFileDialog3.FileName;
            }
        }

        private System.Drawing.Bitmap LoadImage(string fn)
        {
            byte[] buf = File.ReadAllBytes(fn);
            Bitmap bm = (Bitmap)imageConverter.ConvertFrom(buf);
            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution || bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                // Correct a strange glitch that has been observed in the test program when converting 
                //  from a PNG file image created by CopyImageToByteArray() - the dpi value "drifts" 
                //  slightly away from the nominal integer value
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f), (int)(bm.VerticalResolution + 0.5f));
            }
            return bm;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Context ctx = new Context(ParentTrigger);
            string fn = ctx.EvaluateStringExpression(null, null, expAuraImage.Expression);
            Bitmap ix;
            try
            {
                Image iix = AuraContainerForm.LoadImageData(Plugin, fn);
                ix = (Bitmap)iix;
            }
            catch (Exception)
            {
                MessageBox.Show(this, I18n.Translate("internal/ActionForm/imageloaderror", "The specific image file could not be loaded, please verify the path and filename."), I18n.Translate("internal/ActionForm/error", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (AuraDesignForm adf = new AuraDesignForm(AuraContainerForm.AuraTypeEnum.Image))
            {
                I18n.TranslateForm(adf);
                adf.SetImage(ix);
                switch (cbxAuraDisplay.SelectedIndex)
                {
                    case 0:
                        adf.SetImageMode(PictureBoxSizeMode.Normal);
                        break;
                    case 1:
                        adf.SetImageMode(PictureBoxSizeMode.StretchImage);
                        break;
                    case 2:
                        adf.SetImageMode(PictureBoxSizeMode.CenterImage);
                        break;
                    case 3:
                        adf.SetImageMode(PictureBoxSizeMode.Zoom);
                        break;
                }
                if (
                    (expAuraXIni.Expression.Length == 0)
                    ||
                    (expAuraYIni.Expression.Length == 0)
                    ||
                    (expAuraWIni.Expression.Length == 0)
                    ||
                    (expAuraHIni.Expression.Length == 0)
                )
                {
                    adf.StartPosition = FormStartPosition.CenterParent;
                }
                else
                {
                    adf.StartPosition = FormStartPosition.Manual;
                }
                int i = (int)Math.Round(ctx.EvaluateNumericExpression(null, null, expAuraXIni.Expression));
                adf.Left = i;
                i = (int)Math.Round(ctx.EvaluateNumericExpression(null, null, expAuraYIni.Expression));
                adf.Top = i;
                i = (int)Math.Round(ctx.EvaluateNumericExpression(null, null, expAuraWIni.Expression));
                adf.Width = i;
                i = (int)Math.Round(ctx.EvaluateNumericExpression(null, null, expAuraHIni.Expression));
                adf.Height = i;
                switch (adf.ShowDialog())
                {
                    case DialogResult.OK:
                        expAuraXIni.Expression = adf.Left.ToString();
                        expAuraYIni.Expression = adf.Top.ToString();
                        expAuraWIni.Expression = adf.Width.ToString();
                        expAuraHIni.Expression = adf.Height.ToString();
                        if (expAuraOIni.Expression.Length == 0)
                        {
                            expAuraOIni.Expression = "100";
                        }
                        switch (adf.GetImageDisplay())
                        {
                            case PictureBoxSizeMode.Normal:
                                cbxAuraDisplay.SelectedIndex = 0;
                                break;
                            case PictureBoxSizeMode.StretchImage:
                                cbxAuraDisplay.SelectedIndex = 1;
                                break;
                            case PictureBoxSizeMode.CenterImage:
                                cbxAuraDisplay.SelectedIndex = 2;
                                break;
                            case PictureBoxSizeMode.Zoom:
                                cbxAuraDisplay.SelectedIndex = 3;
                                break;
                        }
                        break;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Context ctx = new Context(ParentTrigger) { testByPlaceholder = true };
            using (AuraDesignForm adf = new AuraDesignForm(AuraContainerForm.AuraTypeEnum.Text))
            {
                I18n.TranslateForm(adf);
                adf.TextColor = ExpressionTextBox.ParseColor(expTextForeColor.Text, Color.Black);
                adf.OutlineColor = ExpressionTextBox.ParseColor(expTextOutlineColor.Text, Color.Empty);
                adf.UseOutline = adf.OutlineColor != Color.Empty;
                FontInfoContainer fic = (FontInfoContainer)txtTextAuraFont.Tag;
                FontStyle fs = FontStyle.Regular;
                if ((fic.Effect & ActionOld.TextAuraEffectEnum.Bold) != 0)
                {
                    fs |= FontStyle.Bold;
                }
                if ((fic.Effect & ActionOld.TextAuraEffectEnum.Italic) != 0)
                {
                    fs |= FontStyle.Italic;
                }
                if ((fic.Effect & ActionOld.TextAuraEffectEnum.Underline) != 0)
                {
                    fs |= FontStyle.Underline;
                }
                if ((fic.Effect & ActionOld.TextAuraEffectEnum.Strikeout) != 0)
                {
                    fs |= FontStyle.Strikeout;
                }
                using (Font fx = new Font(fic.Name, fic.Size, fs, GraphicsUnit.Point))
                {
                    adf.AuraFont = fx;
                    switch (cbxTextAuraAlignment.SelectedIndex)
                    {
                        case 0:
                            adf.TextAlignment = ActionOverlayText.TextAlignmentEnum.TopLeft;
                            break;
                        case 1:
                            adf.TextAlignment = ActionOverlayText.TextAlignmentEnum.TopCenter;
                            break;
                        case 2:
                            adf.TextAlignment = ActionOverlayText.TextAlignmentEnum.TopRight;
                            break;
                        case 3:
                            adf.TextAlignment = ActionOverlayText.TextAlignmentEnum.MiddleLeft;
                            break;
                        case 4:
                            adf.TextAlignment = ActionOverlayText.TextAlignmentEnum.MiddleCenter;
                            break;
                        case 5:
                            adf.TextAlignment = ActionOverlayText.TextAlignmentEnum.MiddleRight;
                            break;
                        case 6:
                            adf.TextAlignment = ActionOverlayText.TextAlignmentEnum.BottomLeft;
                            break;
                        case 7:
                            adf.TextAlignment = ActionOverlayText.TextAlignmentEnum.BottomCenter;
                            break;
                        case 8:
                            adf.TextAlignment = ActionOverlayText.TextAlignmentEnum.BottomRight;
                            break;
                    }
                    if (string.IsNullOrWhiteSpace(expTextAuraXIni.Expression + expTextAuraYIni.Expression + expTextAuraWIni.Expression + expTextAuraHIni.Expression))
                    {
                        adf.StartPosition = FormStartPosition.CenterParent;
                    }
                    else
                    {
                        adf.StartPosition = FormStartPosition.Manual;
                    }
                    int i = (int)Math.Round(ctx.EvaluateNumericExpression(null, null, expTextAuraXIni.Expression));
                    adf.Left = i;
                    i = (int)Math.Round(ctx.EvaluateNumericExpression(null, null, expTextAuraYIni.Expression));
                    adf.Top = i;
                    i = (int)Math.Round(ctx.EvaluateNumericExpression(null, null, expTextAuraWIni.Expression));
                    adf.Width = i;
                    i = (int)Math.Round(ctx.EvaluateNumericExpression(null, null, expTextAuraHIni.Expression));
                    adf.Height = i;
                    switch (adf.ShowDialog())
                    {
                        case DialogResult.OK:
                            expTextAuraXIni.Expression = adf.Left.ToString();
                            expTextAuraYIni.Expression = adf.Top.ToString();
                            expTextAuraWIni.Expression = adf.Width.ToString();
                            expTextAuraHIni.Expression = adf.Height.ToString();
                            if (expTextAuraOIni.Expression.Length == 0)
                            {
                                expTextAuraOIni.Expression = "100";
                            }
                            switch (adf.TextAlignment)
                            {
                                case ActionOverlayText.TextAlignmentEnum.TopLeft:
                                    cbxTextAuraAlignment.SelectedIndex = 0;
                                    break;
                                case ActionOverlayText.TextAlignmentEnum.TopCenter:
                                    cbxTextAuraAlignment.SelectedIndex = 1;
                                    break;
                                case ActionOverlayText.TextAlignmentEnum.TopRight:
                                    cbxTextAuraAlignment.SelectedIndex = 2;
                                    break;
                                case ActionOverlayText.TextAlignmentEnum.MiddleLeft:
                                    cbxTextAuraAlignment.SelectedIndex = 3;
                                    break;
                                case ActionOverlayText.TextAlignmentEnum.MiddleCenter:
                                    cbxTextAuraAlignment.SelectedIndex = 4;
                                    break;
                                case ActionOverlayText.TextAlignmentEnum.MiddleRight:
                                    cbxTextAuraAlignment.SelectedIndex = 5;
                                    break;
                                case ActionOverlayText.TextAlignmentEnum.BottomLeft:
                                    cbxTextAuraAlignment.SelectedIndex = 6;
                                    break;
                                case ActionOverlayText.TextAlignmentEnum.BottomCenter:
                                    cbxTextAuraAlignment.SelectedIndex = 7;
                                    break;
                                case ActionOverlayText.TextAlignmentEnum.BottomRight:
                                    cbxTextAuraAlignment.SelectedIndex = 8;
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            var a = SettingsToAction().ConvertToOldAction();
            Context ctx = new Context(ParentTrigger) { testByPlaceholder = true };
            a.ActionType = ActionTypeEnum.Aura;
            a._AuraOp = ActionOld.AuraOpEnum.DeactivateAura;
            a.Execute(null, ctx);
        }

        private void btnTextAuraHide_Click(object sender, EventArgs e)
        {
            var a = SettingsToAction().ConvertToOldAction();
            Context ctx = new Context(ParentTrigger) { testByPlaceholder = true };
            a.ActionType = ActionTypeEnum.TextAura;
            a._AuraOp = ActionOld.AuraOpEnum.DeactivateAura;
            a.Execute(null, ctx);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int ex = (int)timer1.Tag;
            ex++;
            if ((ex >= 0 && ex < 10) || (ex >= 15 && ex < 25))
            {
                if ((ex % 2) == 0)
                {
                    btnHide.BackColor = SystemColors.Info;
                }
                else
                {
                    btnHide.BackColor = SystemColors.Control;
                }
            }
            if (ex >= 25)
            {
                btnHide.BackColor = SystemColors.Control;
                timer1.Stop();
            }
            timer1.Tag = ex;
        }

        private void trvFolder_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is Folder)
            {
                e.Node.ImageIndex = (int)CustomControls.UserInterface.GetImageIndexForClosedFolder((Folder)e.Node.Tag);
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void trvFolder_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is Folder)
            {
                e.Node.ImageIndex = (int)CustomControls.UserInterface.GetImageIndexForOpenFolder((Folder)e.Node.Tag);
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void trvFolder_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if ((e.Node.Tag is Folder) == false)
            {
                e.Cancel = true;
                return;
            }
            Folder f = (Folder)e.Node.Tag;
            if (f.Parent == null)
            {
                e.Cancel = true;
                return;
            }
            //plug.AddToLog(true, "Selected folder (" + f.Name + ") with id (" + f.Id + ")");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void cbxTextAuraOp_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxTextAuraOp.SelectedIndex)
            {
                case (int)ActionOverlayText.OperationEnum.Activate:
                case (int)ActionOverlayText.OperationEnum.Deactivate:
                case (int)ActionOverlayText.OperationEnum.DeactivateAll:
                    expTextAuraName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTextAuraName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Text;
                    break;
                case (int)ActionOverlayText.OperationEnum.DeactivateRegex:
                    expTextAuraName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
                    expTextAuraName.AutofillType = ExpressionTextBox.AutofillTypeEnum.None;
                    break;
            }
            bool enable = cbxTextAuraOp.SelectedIndex == 0;
            cbxTextAuraAlignment.Enabled = enable;
            expTextAuraText.Enabled = enable;
            expTextAuraXIni.Enabled = enable;
            expTextAuraYIni.Enabled = enable;
            expTextAuraWIni.Enabled = enable;
            expTextAuraHIni.Enabled = enable;
            expTextAuraOIni.Enabled = enable;
            btnTextAuraFont.Enabled = enable;
            expTextAuraXTick.Enabled = enable;
            expTextAuraYTick.Enabled = enable;
            expTextAuraWTick.Enabled = enable;
            expTextAuraHTick.Enabled = enable;
            expTextAuraOTick.Enabled = enable;
            expTextAuraTTLTick.Enabled = enable;
            colorSelector1.Enabled = enable;
            expTextForeColor.Enabled = enable;
            expTextBackColor.Enabled = enable;
            expTextOutlineColor.Enabled = enable;
            txtTextAuraFont.Enabled = enable;
            btnTextAuraGuide.Enabled = enable;
            expTextAuraName.Enabled = (cbxTextAuraOp.SelectedIndex != 2);
            btnTextAuraHide.Enabled = (cbxTextAuraOp.SelectedIndex != 2);
        }

        private void btnTextAuraFont_Click(object sender, EventArgs e)
        {
            FontInfoContainer fic = (FontInfoContainer)txtTextAuraFont.Tag;
            FontStyle fs = FontStyle.Regular;
            if ((fic.Effect & ActionOld.TextAuraEffectEnum.Bold) != 0)
            {
                fs |= FontStyle.Bold;
            }
            if ((fic.Effect & ActionOld.TextAuraEffectEnum.Italic) != 0)
            {
                fs |= FontStyle.Italic;
            }
            if ((fic.Effect & ActionOld.TextAuraEffectEnum.Underline) != 0)
            {
                fs |= FontStyle.Underline;
            }
            if ((fic.Effect & ActionOld.TextAuraEffectEnum.Strikeout) != 0)
            {
                fs |= FontStyle.Strikeout;
            }
            if (fic.Size < 1)
            {
                fic.Size = 1;
            }
            using (Font fx = new Font(fic.Name, fic.Size, fs, GraphicsUnit.Point))
            {
                fontDialog1.Font = fx;
                switch (fontDialog1.ShowDialog())
                {
                    case DialogResult.OK:
                        {
                            fic.Name = fontDialog1.Font.Name;
                            fic.Size = fontDialog1.Font.SizeInPoints;
                            ActionOld.TextAuraEffectEnum ef = ActionOld.TextAuraEffectEnum.None;
                            if (fontDialog1.Font.Bold == true)
                            {
                                ef |= ActionOld.TextAuraEffectEnum.Bold;
                            }
                            if (fontDialog1.Font.Italic == true)
                            {
                                ef |= ActionOld.TextAuraEffectEnum.Italic;
                            }
                            if (fontDialog1.Font.Strikeout == true)
                            {
                                ef |= ActionOld.TextAuraEffectEnum.Strikeout;
                            }
                            if (fontDialog1.Font.Underline == true)
                            {
                                ef |= ActionOld.TextAuraEffectEnum.Underline;
                            }
                            fic.Effect = ef;
                            UpdateFontDescription();
                        }
                        break;
                }
            }
        }

        private void expTextForeColor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                colorSelector1._textColor = ExpressionTextBox.ParseColor(
                    UiContext.EvaluateStringExpression(null, UiContext, expTextForeColor.Text), Color.Black);
            }
            catch 
            {
                colorSelector1._textColor = Color.Black;
            }
            colorSelector1.Invalidate();
        }

        private void expTextBackColor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                colorSelector1._backgroundColor = ExpressionTextBox.ParseColor(
                    UiContext.EvaluateStringExpression(null, UiContext, expTextBackColor.Text), Color.Transparent);
            }
            catch
            {
                colorSelector1._backgroundColor = Color.Transparent;
            }
            colorSelector1.Invalidate();
        }

        private void expTextOutlineColor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                colorSelector1._textOutlineColor = ExpressionTextBox.ParseColor(
                    UiContext.EvaluateStringExpression(null, UiContext, expTextOutlineColor.Text), Color.Empty);
            }
            catch
            {
                colorSelector1._textOutlineColor = Color.Empty;
            }
            colorSelector1.Invalidate();
        }


        private void colorSelector1_ColorChanged(object sender, ColorChangedEventArgs e)
        {
            switch (e.ChangedType)
            {
                case ColorChangedType.TextColor:
                    expTextForeColor.Text = ExpressionTextBox.ColorToString(e.NewColor, Color.Black);
                    break;
                case ColorChangedType.TextOutlineColor:
                    expTextOutlineColor.Text = ExpressionTextBox.ColorToString(e.NewColor, Color.Empty);
                    break;
                case ColorChangedType.BackgroundColor:
                    expTextBackColor.Text = ExpressionTextBox.ColorToString(e.NewColor, Color.Transparent);
                    break;
            }
        }

        private void cbxLvarExpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxLvarExpType.SelectedIndex)
            {
                case 0:
                    expLvarValue.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case 1:
                    expLvarValue.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Numeric;
                    break;
            }
        }

        private void cbxLvarOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxLvarOperation.SelectedIndex)
            {
                case (int)ActionVariableList.OperationEnum.Unset:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.Push:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.Enabled = true;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.Insert:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.Enabled = true;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = true;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.Set:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.Enabled = true;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = true;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.SetAll:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.Enabled = true;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = true;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.Remove:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = true;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.PopFirst:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = true;
                    expLvarIndex.Enabled = true;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    expLvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.Scalar;
                    break;
                case (int)ActionVariableList.OperationEnum.PopToListInsert:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.SelectedIndex = 1; // numeric
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = true;
                    expLvarIndex.Enabled = true;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    expLvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.PopToListSet:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.SelectedIndex = 1; // numeric
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = true;
                    expLvarIndex.Enabled = true;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    expLvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.Build:
                    expLvarName.Enabled = false;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.Enabled = true;
                    expLvarTarget.Enabled = true;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    expLvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.Filter:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.Enabled = false;
                    cbxLvarExpType.SelectedIndex = 1; // numeric
                    expLvarTarget.Enabled = true;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    expLvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.Join:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.Enabled = true;
                    expLvarTarget.Enabled = true;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    expLvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.Scalar;
                    break;
                case (int)ActionVariableList.OperationEnum.Split:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.Enabled = true;
                    expLvarTarget.Enabled = true;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Scalar;
                    expLvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.Copy:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = true;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    expLvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.InsertList:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = true;
                    expLvarIndex.Enabled = true;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    expLvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.SortNumericAsc:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.SortNumericDesc:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.SortAlphaAsc:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.SortAlphaDesc:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.SortFfxivPartyAsc:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.SortFfxivPartyDesc:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.SortByKeys:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = true;
                    cbxLvarExpType.Enabled = false;
                    cbxLvarExpType.SelectedIndex = 0; // string
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableList.OperationEnum.UnsetAll:
                    expLvarName.Enabled = false;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    break;
                case (int)ActionVariableList.OperationEnum.UnsetRegex:
                    expLvarName.Enabled = true;
                    expLvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
                    expLvarValue.Enabled = false;
                    cbxLvarExpType.Enabled = false;
                    expLvarTarget.Enabled = false;
                    expLvarIndex.Enabled = false;
                    expLvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.None;
                    break;
            }
            prsListSource.Enabled = (cbxLvarOperation.SelectedIndex == (int)ActionVariableList.OperationEnum.UnsetAll)
                                  ? true : expLvarName.Enabled;
            prsListTarget.Enabled = expLvarTarget.Enabled;
            lblLvarValue.Text = (cbxLvarOperation.SelectedIndex != (int)ActionVariableList.OperationEnum.PopToListInsert
                              && cbxLvarOperation.SelectedIndex != (int)ActionVariableList.OperationEnum.PopToListSet)
                              ? I18n.Translate("ActionForm/lblLvarValue", "Expression")
                              : I18n.Translate("ActionForm/lblLvarValueTgtIndex", "Target Index");
            rtbLvarHelper.UpdateText();
        }

        private void expVariableName_EnabledChanged(object sender, EventArgs e)
        {
            lblVariableName.Enabled = expVariableName.Enabled;
        }

        private void expVariableExpression_EnabledChanged(object sender, EventArgs e)
        {
            lblVariableExpression.Enabled = expVariableExpression.Enabled;
        }

        private void expTriggerText_EnabledChanged(object sender, EventArgs e)
        {
            lblTriggerText.Enabled = expTriggerText.Enabled;
        }
        private void cbxZoneType_EnabledChanged(object sender, EventArgs e)
        {
            lblTriggerZoneType.Enabled = cbxTriggerZoneType.Enabled;
        }

        private void expTriggerActionTag_EnabledChanged(object sender, EventArgs e)
        {
            lblTriggerTagRegex.Enabled = expTriggerTagRegex.Enabled;
        }

        private void expTriggerZone_EnabledChanged(object sender, EventArgs e)
        {
            lblTriggerZone.Enabled = expTriggerZone.Enabled;
        }

        private void cbxFiringOptions_EnabledChanged(object sender, EventArgs e)
        {
            lblFiringOptions.Enabled = cbxFiringOptions.Enabled;
        }

        private void trvTrigger_EnabledChanged(object sender, EventArgs e)
        {
            lblTrigger.Enabled = trvTrigger.Enabled;
        }

        private void cbxFolderOp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void expLvarName_EnabledChanged(object sender, EventArgs e)
        {
            lblLvarName.Enabled = expLvarName.Enabled;
        }

        private void cbxLvarExpType_EnabledChanged(object sender, EventArgs e)
        {
            lblLvarExpType.Enabled = cbxLvarExpType.Enabled;
        }

        private void expLvarValue_EnabledChanged(object sender, EventArgs e)
        {
            lblLvarValue.Enabled = expLvarValue.Enabled;
        }

        private void expLvarIndex_EnabledChanged(object sender, EventArgs e)
        {
            lblLvarIndex.Enabled = expLvarIndex.Enabled;
        }

        private void expLvarTarget_EnabledChanged(object sender, EventArgs e)
        {
            lblLvarTarget.Enabled = expLvarTarget.Enabled;
        }

        private void expTextAuraName_EnabledChanged(object sender, EventArgs e)
        {
            lblTextAuraName.Enabled = expTextAuraName.Enabled;
        }

        private void expTextAuraText_EnabledChanged(object sender, EventArgs e)
        {
            lblTextAuraText.Enabled = expTextAuraText.Enabled;
        }

        private void colorSelector1_EnabledChanged(object sender, EventArgs e)
        {
            lblTextAuraFont.Enabled = colorSelector1.Enabled;
        }

        private void cbxTextAuraAlignment_EnabledChanged(object sender, EventArgs e)
        {
            lblTextAuraAlignment.Enabled = cbxTextAuraAlignment.Enabled;
        }

        private void expTextAuraXTick_EnabledChanged(object sender, EventArgs e)
        {
            lblTextAuraX.Enabled = expTextAuraXTick.Enabled;
        }

        private void expTextAuraYTick_EnabledChanged(object sender, EventArgs e)
        {
            lblTextAuraY.Enabled = expTextAuraYTick.Enabled;
        }

        private void expTextAuraWTick_EnabledChanged(object sender, EventArgs e)
        {
            lblTextAuraWidth.Enabled = expTextAuraWTick.Enabled;
        }

        private void expTextAuraHTick_EnabledChanged(object sender, EventArgs e)
        {
            lblTextAuraHeight.Enabled = expTextAuraHTick.Enabled;
        }

        private void expTextAuraOTick_EnabledChanged(object sender, EventArgs e)
        {
            lblTextAuraOpacity.Enabled = expTextAuraOTick.Enabled;
        }

        private void expTextAuraTTLTick_EnabledChanged(object sender, EventArgs e)
        {
            lblTextAuraTtlExp.Enabled = expTextAuraTTLTick.Enabled;
        }

        private void expAuraName_EnabledChanged(object sender, EventArgs e)
        {
            lblAuraName.Enabled = expAuraName.Enabled;
        }

        private void expAuraImage_EnabledChanged(object sender, EventArgs e)
        {
            lblAuraImage.Enabled = expAuraImage.Enabled;
        }

        private void cbxAuraDisplay_EnabledChanged(object sender, EventArgs e)
        {
            lblAuraDisplay.Enabled = cbxAuraDisplay.Enabled;
        }

        private void expAuraXTick_EnabledChanged(object sender, EventArgs e)
        {
            lblAuraX.Enabled = expAuraXTick.Enabled;
        }

        private void expAuraYTick_EnabledChanged(object sender, EventArgs e)
        {
            lblAuraY.Enabled = expAuraYTick.Enabled;
        }

        private void expAuraWTick_EnabledChanged(object sender, EventArgs e)
        {
            lblAuraWidth.Enabled = expAuraWTick.Enabled;
        }

        private void expAuraHTick_EnabledChanged(object sender, EventArgs e)
        {
            lblAuraHeight.Enabled = expAuraHTick.Enabled;
        }

        private void expAuraOTick_EnabledChanged(object sender, EventArgs e)
        {
            lblAuraOpacity.Enabled = expAuraOTick.Enabled;
        }

        private void expAuraTTLTick_EnabledChanged(object sender, EventArgs e)
        {
            lblAuraTtl.Enabled = expAuraTTLTick.Enabled;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(txtObsWebsocketLink.Text.ToString());
        }

        private void cbxObsOpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            expObsSceneName.Enabled = (cbxObsOpType.SelectedIndex >= 15 && cbxObsOpType.SelectedIndex <= 17);
            expObsSourceName.Enabled = (cbxObsOpType.SelectedIndex >= 16 && cbxObsOpType.SelectedIndex <= 17);
            expObsJSONPayload.Enabled = (cbxObsOpType.SelectedIndex >= 18);
        }

        private void cbxLsOpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            expLSCustPayload.Enabled = (cbxLsOpType.SelectedIndex == cbxLsOpType.Items.Count - 1);
            lblLsCustPayload.Enabled = expLSCustPayload.Enabled;
        }

        private void cbxKeypressMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblKeypresses.Enabled = (cbxKeypressMethod.SelectedIndex == 0);
            expKeypresses.Enabled = (cbxKeypressMethod.SelectedIndex == 0);
            btnSendKeysListen.Enabled = (cbxKeypressMethod.SelectedIndex == 0);
            lblKeypressWindow.Enabled = (cbxKeypressMethod.SelectedIndex >= 1);
            expWindowTitle.Enabled = (cbxKeypressMethod.SelectedIndex >= 1);
            lblKeypress.Enabled = (cbxKeypressMethod.SelectedIndex >= 1);
            expKeypress.Enabled = (cbxKeypressMethod.SelectedIndex >= 1);
            btnKeycodesListen.Enabled = (cbxKeypressMethod.SelectedIndex >= 1);
            lblKeypressProcId.Enabled = (cbxKeypressMethod.SelectedIndex >= 1);
            expKeypressProcId.Enabled = (cbxKeypressMethod.SelectedIndex >= 1);
            txtSendKeysLink.Text = (cbxKeypressMethod.SelectedIndex == 0)
                ? I18n.Translate("internal/ActionForm/urlSendKey", @"https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys.send")
                : I18n.Translate("internal/ActionForm/urlWinMessage", @"https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.keys");
            rtbSendKeysHelper.UpdateText();

            switch (cbxKeypressMethod.SelectedIndex)
            {
                case 0:
                    expKeypress.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Numeric;
                    break;
                case 1:
                    expKeypress.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Numeric;
                    break;
                case 2:
                    expKeypress.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
            }
        }

        private void btnSendKeysLink_Click(object sender, EventArgs e)
        {
            Process.Start(txtSendKeysLink.Text.ToString());
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            tlsMouseLocation.Text = string.Format("X: {0}, Y: {1}",
                Cursor.Position.X.ToString(CultureInfo.InvariantCulture),
                Cursor.Position.Y.ToString(CultureInfo.InvariantCulture)
            );
        }

        private void cbxTvarExpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxTvarExpType.SelectedIndex)
            {
                case 0:
                    expTvarValue.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case 1:
                    expTvarValue.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Numeric;
                    break;
            }
        }

        private void expObsSceneName_EnabledChanged(object sender, EventArgs e)
        {
            lblObsSceneName.Enabled = expObsSceneName.Enabled;
        }

        private void expObsSourceName_EnabledChanged(object sender, EventArgs e)
        {
            lblObsSourceName.Enabled = expObsSourceName.Enabled;
        }

        private void expObsJSONPayload_EnabledChanged(object sender, EventArgs e)
        {
            lblObsJSONPayload.Enabled = expObsJSONPayload.Enabled;
        }

        private void cbxTvarOpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxTvarOpType.SelectedIndex)
            {
                case (int)ActionVariableTable.OperationEnum.Unset:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = false;
                    expTvarValue.Enabled = false;
                    expTvarColumn.Enabled = false;
                    expTvarRow.Enabled = false;
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.Set:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = true;
                    expTvarValue.Enabled = true;
                    expTvarColumn.Enabled = true;
                    expTvarRow.Enabled = true;
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.SetAll:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = true;
                    expTvarValue.Enabled = true;
                    expTvarColumn.Enabled = true;
                    expTvarRow.Enabled = true;
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.SlicesSetAll:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = true;
                    expTvarValue.Enabled = true;
                    expTvarColumn.Enabled = true;
                    expTvarRow.Enabled = true;
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.Resize:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = false;
                    expTvarValue.Enabled = false;
                    expTvarColumn.Enabled = true;
                    expTvarRow.Enabled = true;
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.Build:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = false;
                    cbxTvarExpType.Enabled = true;
                    expTvarValue.Enabled = true;
                    expTvarColumn.Enabled = false;
                    expTvarRow.Enabled = false;
                    expTvarTarget.Enabled = true;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    expTvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.SetLine:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = true;
                    expTvarValue.Enabled = true;
                    expTvarColumn.Enabled = true;
                    expTvarRow.Enabled = true;
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.InsertLine:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = true;
                    expTvarValue.Enabled = true;
                    expTvarColumn.Enabled = true;
                    expTvarRow.Enabled = true;
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.RemoveLine:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = false;
                    expTvarValue.Enabled = false;
                    expTvarColumn.Enabled = true;
                    expTvarRow.Enabled = true;
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.Filter:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = false;
                    cbxTvarExpType.SelectedIndex = 1; // numeric
                    expTvarValue.Enabled = true;
                    expTvarColumn.Enabled = false;
                    expTvarRow.Enabled = false;
                    expTvarTarget.Enabled = true;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    expTvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.List;
                    break;
                case (int)ActionVariableTable.OperationEnum.FilterLine:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = false;
                    expTvarValue.Enabled = false;
                    expTvarColumn.Enabled = true;
                    expTvarRow.Enabled = true;
                    expTvarTarget.Enabled = true;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    expTvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.Copy:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = false;
                    expTvarValue.Enabled = false;
                    expTvarColumn.Enabled = false;
                    expTvarRow.Enabled = false;
                    expTvarTarget.Enabled = true;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    expTvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.Append:
                case (int)ActionVariableTable.OperationEnum.AppendH:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = false;
                    expTvarValue.Enabled = false;
                    expTvarColumn.Enabled = false;
                    expTvarRow.Enabled = false;
                    expTvarTarget.Enabled = true;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    expTvarTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.SortLine:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = false;
                    expTvarValue.Enabled = false;
                    expTvarColumn.Enabled = true;   // set to numeric after switch-case
                    expTvarRow.Enabled = true;      // set to numeric after switch-case
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.GetAllEntities:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = false;
                    expTvarValue.Enabled = false;
                    expTvarColumn.Enabled = true;
                    expTvarRow.Enabled = true;
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Table;
                    break;
                case (int)ActionVariableTable.OperationEnum.UnsetAll:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expTvarName.Enabled = false;
                    cbxTvarExpType.Enabled = false;
                    expTvarValue.Enabled = false;
                    expTvarColumn.Enabled = false;
                    expTvarRow.Enabled = false;
                    expTvarTarget.Enabled = false;
                    break;
                case (int)ActionVariableTable.OperationEnum.UnsetRegex:
                    expTvarName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
                    expTvarName.Enabled = true;
                    cbxTvarExpType.Enabled = false;
                    expTvarValue.Enabled = false;
                    expTvarColumn.Enabled = false;
                    expTvarRow.Enabled = false;
                    expTvarTarget.Enabled = false;
                    expTvarName.AutofillType = ExpressionTextBox.AutofillTypeEnum.None;
                    break;
            }
            expTvarColumn.ExpressionType = (cbxTvarOpType.SelectedIndex == (int)ActionVariableTable.OperationEnum.SortLine
                                         || cbxTvarOpType.SelectedIndex == (int)ActionVariableTable.OperationEnum.SlicesSetAll
                                         || cbxTvarOpType.SelectedIndex == (int)ActionVariableTable.OperationEnum.GetAllEntities)
                                         ? ExpressionTextBox.SupportedExpressionTypeEnum.String
                                         : ExpressionTextBox.SupportedExpressionTypeEnum.Numeric;
            expTvarRow.ExpressionType = expTvarColumn.ExpressionType;
            prsTableSource.Enabled = (cbxTvarOpType.SelectedIndex == (int)ActionVariableTable.OperationEnum.UnsetAll)
                                   ? true : expTvarName.Enabled;
            prsTableTarget.Enabled = expTvarTarget.Enabled;
            rtbTvarHelper.UpdateText();
        }

        private void expTvarName_EnabledChanged(object sender, EventArgs e)
        {
            lblTvarName.Enabled = expTvarName.Enabled;
        }

        private void cbxTvarExpType_EnabledChanged(object sender, EventArgs e)
        {
            lblTvarExpType.Enabled = cbxTvarExpType.Enabled;
        }

        private void expTvarValue_EnabledChanged(object sender, EventArgs e)
        {
            lblTvarValue.Enabled = expTvarValue.Enabled;
        }

        private void expTvarColumn_EnabledChanged(object sender, EventArgs e)
        {
            lblTvarColumn.Enabled = expTvarColumn.Enabled;
        }

        private void expTvarRow_EnabledChanged(object sender, EventArgs e)
        {
            lblTvarRow.Enabled = expTvarRow.Enabled;
        }

        private void expTvarTarget_EnabledChanged(object sender, EventArgs e)
        {
            lblTvarTarget.Enabled = expTvarTarget.Enabled;
        }

        private void cbxDictKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxDictKeyType.SelectedIndex)
            {
                case 0:
                    expDictKey.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case 1:
                    expDictKey.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Numeric;
                    break;
            }
        }

        private void cbxDictValueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxDictValueType.SelectedIndex)
            {
                case 0:
                    expDictValue.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    break;
                case 1:
                    expDictValue.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Numeric;
                    break;
            }
        }

        private void cbxDictOpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxDictOpType.SelectedIndex)
            {
                case (int)ActionVariableDict.OperationEnum.Unset:
                    expDictName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expDictName.Enabled = true;
                    prsDictSource.Enabled = true;
                    expDictLength.Enabled = false;
                    cbxDictKeyType.Enabled = false;
                    expDictKey.Enabled = false;
                    cbxDictValueType.Enabled = false;
                    expDictValue.Enabled = false;
                    expDictTarget.Enabled = false;
                    expDictName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    break;
                case (int)ActionVariableDict.OperationEnum.Set:
                    expDictName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expDictName.Enabled = true;
                    prsDictSource.Enabled = true;
                    expDictLength.Enabled = false;
                    cbxDictKeyType.Enabled = true;
                    expDictKey.Enabled = true;
                    cbxDictValueType.Enabled = true;
                    expDictValue.Enabled = true;
                    expDictTarget.Enabled = false;
                    expDictName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    break;
                case (int)ActionVariableDict.OperationEnum.Remove:
                    expDictName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expDictName.Enabled = true;
                    prsDictSource.Enabled = true;
                    expDictLength.Enabled = false;
                    cbxDictKeyType.Enabled = true;
                    expDictKey.Enabled = true;
                    cbxDictValueType.Enabled = false;
                    expDictValue.Enabled = false;
                    expDictTarget.Enabled = false;
                    expDictName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    break;
                case (int)ActionVariableDict.OperationEnum.SetAll:
                    expDictName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expDictName.Enabled = true;
                    prsDictSource.Enabled = true;
                    expDictLength.Enabled = true;
                    cbxDictKeyType.Enabled = true;
                    expDictKey.Enabled = true;
                    cbxDictValueType.Enabled = true;
                    expDictValue.Enabled = true;
                    expDictTarget.Enabled = false;
                    expDictName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    break;
                case (int)ActionVariableDict.OperationEnum.Build:
                    expDictName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expDictName.Enabled = false;
                    prsDictSource.Enabled = false;
                    expDictLength.Enabled = false;
                    cbxDictKeyType.Enabled = false;
                    expDictKey.Enabled = false;
                    cbxDictValueType.Enabled = true;
                    expDictValue.Enabled = true;
                    expDictTarget.Enabled = true;
                    expDictName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    expDictTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    break;
                case (int)ActionVariableDict.OperationEnum.Filter:
                    expDictName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expDictName.Enabled = true;
                    prsDictSource.Enabled = true;
                    expDictLength.Enabled = false;
                    cbxDictKeyType.Enabled = false;
                    expDictKey.Enabled = false;
                    cbxDictValueType.Enabled = false;
                    cbxDictValueType.SelectedIndex = 1; // numeric
                    expDictValue.Enabled = true;
                    expDictTarget.Enabled = true;
                    expDictName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    expDictTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    break;
                case (int)ActionVariableDict.OperationEnum.Merge:
                case (int)ActionVariableDict.OperationEnum.MergeHard:
                    expDictName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expDictName.Enabled = true;
                    prsDictSource.Enabled = true;
                    expDictLength.Enabled = false;
                    cbxDictKeyType.Enabled = false;
                    expDictKey.Enabled = false;
                    cbxDictValueType.Enabled = false;
                    expDictValue.Enabled = false;
                    expDictTarget.Enabled = true;
                    expDictName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    expDictTarget.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    break;
                case (int)ActionVariableDict.OperationEnum.GetEntity:
                    expDictName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expDictName.Enabled = true;
                    prsDictSource.Enabled = true;
                    expDictLength.Enabled = false;
                    cbxDictKeyType.Enabled = false;
                    expDictKey.Enabled = true;
                    cbxDictKeyType.SelectedIndex = 0; // string
                    cbxDictValueType.Enabled = false;
                    cbxDictValueType.SelectedIndex = 0; // string
                    expDictValue.Enabled = true;
                    expDictTarget.Enabled = false;
                    expDictName.AutofillType = ExpressionTextBox.AutofillTypeEnum.Dict;
                    break;
                case (int)ActionVariableDict.OperationEnum.UnsetAll:
                    expDictName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    expDictName.Enabled = false;
                    prsDictSource.Enabled = true;
                    expDictLength.Enabled = false;
                    cbxDictKeyType.Enabled = false;
                    expDictKey.Enabled = false;
                    cbxDictValueType.Enabled = false;
                    expDictValue.Enabled = false;
                    expDictTarget.Enabled = false;
                    break;
                case (int)ActionVariableDict.OperationEnum.UnsetRegex:
                    expDictName.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
                    expDictName.Enabled = true;
                    prsDictSource.Enabled = true;
                    expDictLength.Enabled = false;
                    cbxDictKeyType.Enabled = false;
                    expDictKey.Enabled = false;
                    cbxDictValueType.Enabled = false;
                    expDictValue.Enabled = false;
                    expDictTarget.Enabled = false;
                    expDictName.AutofillType = ExpressionTextBox.AutofillTypeEnum.None;
                    break;
            }
            prsDictTarget.Enabled = expDictTarget.Enabled;
            rtbDictHelper.UpdateText();
        }

        private void expDictName_EnabledChanged(object sender, EventArgs e)
        {
            lblDictName.Enabled = expDictName.Enabled;
        }

        private void expDictLength_EnabledChanged(object sender, EventArgs e)
        {
            lblDictLength.Enabled = expDictLength.Enabled;
        }

        private void cbxDictKeyType_EnabledChanged(object sender, EventArgs e)
        {
            lblDictKeyType.Enabled = cbxDictKeyType.Enabled;
        }

        private void expDictKey_EnabledChanged(object sender, EventArgs e)
        {
            lblDictKey.Enabled = expDictKey.Enabled;
        }

        private void cbxDictValueType_EnabledChanged(object sender, EventArgs e)
        {
            lblDictValueType.Enabled = cbxDictValueType.Enabled;
        }

        private void expDictValue_EnabledChanged(object sender, EventArgs e)
        {
            lblDictValue.Enabled = expDictValue.Enabled;
        }

        private void expDictTarget_EnabledChanged(object sender, EventArgs e)
        {
            lblDictTarget.Enabled = expDictTarget.Enabled;
        }


        private void cbxJsonType_SelectedIndexChanged(object sender, EventArgs e)
        {
            expJsonPayload.Enabled = (cbxJsonType.SelectedIndex == 0);
        }

        private void chkProcessLog_CheckedChanged(object sender, EventArgs e)
        {
            chkProcessLogACT.Enabled = chkProcessLog.Checked;
            cbxLogMessageTarget.Enabled = chkProcessLog.Checked;
            lblLogMessageTarget.Enabled = cbxLogMessageTarget.Enabled;
            cbxLogMessageLevel.Enabled = (cbxLogMessageTarget.Enabled == false);
            lblLogMessageLevel.Enabled = cbxLogMessageLevel.Enabled;
        }

        private void btnScriptExternalEditor_Click(object sender, EventArgs e)
        {
            try
            {
                string code = expExecScriptCode.Expression;
                EditorTempFn = Path.GetTempFileName() + ".cs";
                File.WriteAllText(EditorTempFn, code);
                ExtEditor = new Process();
                ExtEditor.StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = EditorTempFn
                };
                ExtEditor.EnableRaisingEvents = true;
                ExtEditor.Exited += P_Exited;
                ExtEditor.Start();
                btnScriptExternalEditor.Enabled = false;
                expExecScriptCode.Enabled = false;
                lblScriptExtEditor.Visible = true;
            }
            catch (Exception ex)
            {
                Plugin.FilteredAddToLog(
                    RealPlugin.DebugLevelEnum.Error, 
                    I18n.Translate("internal/ActionForm/exteditorfailed", "Couldn't launch external editor due to exception: {0}", ex.Message),
                    this.ParentTrigger);
            }
        }

        private void P_Exited(object sender, EventArgs e)
        {
            try
            {
                expExecScriptCode.Expression = File.ReadAllText(EditorTempFn);
                File.Delete(EditorTempFn);
            }
            catch (Exception ex)
            {
                Plugin.FilteredAddToLog(
                    RealPlugin.DebugLevelEnum.Error, 
                    I18n.Translate("internal/ActionForm/tempfiledeletefailed", "Couldn't process temporary file {0} due to exception: {1}", EditorTempFn, ex.Message),
                    this.ParentTrigger);
            }
            btnScriptExternalEditor.Enabled = true;
            expExecScriptCode.Enabled = true;
            lblScriptExtEditor.Visible = false;
            ExtEditor.Dispose();
            ExtEditor = null;
        }

        private void cbxActOpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbxActionType.SelectedIndex)
            {
                case (int)ActionActInteraction.OperationEnum.SetCombatState:
                    expActOpStringParam.Enabled = false;
                    break;
                case (int)ActionActInteraction.OperationEnum.LogAllNetwork:
                    expActOpStringParam.Enabled = false;
                    break;
                case (int)ActionActInteraction.OperationEnum.UseDeucalion:
                    expActOpStringParam.Enabled = false;
                    break;
            }
        }

        private void cbxRepositoryOp_SelectedIndexChanged(object sender, EventArgs e)
        {
            trvRepositoryLink.Enabled = (cbxRepositoryOp.SelectedIndex == 1);
        }

        private void trvRepositoryLink_EnabledChanged(object sender, EventArgs e)
        {
            lblRepositoryLink.Enabled = trvRepositoryLink.Enabled;
        }

        private void trvRepositoryLink_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if ((e.Node.Tag is Repository) == false)
            {
                e.Cancel = true;
            }
        }

        private void trvRepositoryLink_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is Folder)
            {
                e.Node.ImageIndex = (int)CustomControls.UserInterface.GetImageIndexForClosedFolder((Folder)e.Node.Tag);
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void trvRepositoryLink_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is Folder)
            {
                e.Node.ImageIndex = (int)CustomControls.UserInterface.GetImageIndexForOpenFolder((Folder)e.Node.Tag);
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void cbxZoneType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTriggerZoneType.SelectedIndex == 0)
            {
                lblTriggerZone.Text = I18n.Translate("TestInputForm/lblZoneName", "Zone name");
            }
            else
            {
                lblTriggerZone.Text = I18n.Translate("TestInputForm/ffxivzoneid", "Zone ID");
            }
        }

        private void btnSendKeysListen_Click(object sender, EventArgs e)
        {
            ListenToKeypresses();
        }

        private void btnKeycodesListen_Click(object sender, EventArgs e)
        {
            ListenToKeypresses();
        }

        private static Dictionary<string, string> SendKeysMap = new Dictionary<string, string>
        {
            {"Back", "{BS}"}, {"Escape", "{ESC}"}, {"Enter", "{ENTER}"}, {"Tab", "{TAB}"}, {"Space", " "},
            {"Scroll", "{SCROLLLOCK}"}, {"CapsLock", "{CAPSLOCK}"}, {"NumLock", "{NUMLOCK}"}, 
            {"Insert", "{INS}"}, {"Delete", "{DEL}"}, {"Home", "{HOME}"}, {"End", "{END}"}, {"PageUp", "{PGUP}"}, {"PageDown", "{PGDN}"},
            {"Up", "{UP}"}, {"Down", "{DOWN}"}, {"Left", "{LEFT}"}, {"Right", "{RIGHT}"},
            {"Add", "{ADD}"}, {"Subtract", "{SUBTRACT}"}, {"Multiply", "{MULTIPLY}"}, {"Divide", "{DIVIDE}"},
            {"PrintScreen", "{PRTSC}"}, {"Help", "{HELP}"},
            {"Oem1", ":"}, {"OemSemicolon", ":"}, {"Oem2", "/"}, {"OemQuestion", "/"}, {"Oem3", "`"}, {"Oemtilde", "`"}, 
            {"Oem4", "["}, {"OemOpenBrackets", "["}, {"Oem5", "\\"}, {"OemPipe", "\\"}, {"Oem6", "]"}, {"OemCloseBrackets", "]"},
            {"Oem7", "'"}, {"OemQuotes", "'"}, {"Oemcomma", ","}, {"OemPeriod", "."}, {"OemMinus", "-"}, {"Oemplus", "="},
            {"A", "a"}, {"B", "b"}, {"C", "c"}, {"D", "d"}, {"E", "e"}, {"F", "f"}, {"G", "g"}, {"H", "h"}, {"I", "i"}, {"J", "j"}, {"K", "k"}, {"L", "l"}, {"M", "m"}, {"N", "n"}, {"O", "o"}, {"P", "p"}, {"Q", "q"}, {"R", "r"}, {"S", "s"}, {"T", "t"}, {"U", "u"}, {"V", "v"}, {"W", "w"}, {"X", "x"}, {"Y", "y"}, {"Z", "z"},
            {"F1", "{F1}"}, {"F2", "{F2}"}, {"F3", "{F3}"}, {"F4", "{F4}"}, {"F5", "{F5}"}, {"F6", "{F6}"}, {"F7", "{F7}"}, {"F8", "{F8}"}, {"F9", "{F9}"}, {"F10", "{F10}"}, {"F11", "{F11}"}, {"F12", "{F12}"}, {"F13", "{F13}"}, {"F14", "{F14}"}, {"F15", "{F15}"}, {"F16", "{F16}"},
            {"D0", "0"}, {"D1", "1"}, {"D2", "2"}, {"D3", "3"}, {"D4", "4"}, {"D5", "5"}, {"D6", "6"}, {"D7", "7"}, {"D8", "8"}, {"D9", "9"},
            {"None", ""}
        };

        private void ListenToKeypresses()
        {
            Keys keyData = Keys.None;
            string keyRep = "";
            using (KeyListenForm klf = new KeyListenForm())
            {
                if (klf.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
                keyData = klf.CurrentKey;
            }
            if (keyData != Keys.None)
            {
                if (cbxKeypressMethod.SelectedIndex != (int)ActionKeypress.OperationEnum.SendKeys)
                {
                    keyRep = ((int)keyData).ToString();
                }
                else
                {
                    if ((keyData & Keys.Control) != Keys.None)
                    {
                        keyRep += "^";
                    }
                    if ((keyData & Keys.Shift) != Keys.None)
                    {
                        keyRep += "+";
                    }
                    if ((keyData & Keys.Alt) != Keys.None)
                    {
                        keyRep += "%";
                    }
                    Keys keyCode = keyData & Keys.KeyCode;
                    string keyName = keyCode.ToString();
                    if (SendKeysMap.ContainsKey(keyName))
                    {
                        keyRep += SendKeysMap[keyName];
                    }
                    else 
                    {
                        MessageBox.Show(I18n.Translate("internal/ActionForm/sendkeyinvalid", "This key is not supported in the SendKeys method."), 
                            I18n.Translate("internal/ConfigurationForm/warning", "Warning"));
                    }
                }
            }
            switch (cbxKeypressMethod.SelectedIndex)
            {
                case (int)ActionKeypress.OperationEnum.SendKeys:
                    expKeypresses.Expression += keyRep; break;
                case (int)ActionKeypress.OperationEnum.WindowMessage:
                    expKeypress.Expression = keyRep; break;
                case (int)ActionKeypress.OperationEnum.WindowMessageCombo:
                    expKeypress.Expression = (expKeypress.Expression == "") ? keyRep : expKeypress.Expression + "," + keyRep; break;
            }
        }

        // Applying a different font for Chinese characters to fix the issue of no line spacings in the cbx.
        // Will try to apply the font globally depending on the selected languages later, but for now, this fix should suffice.
        public void SetComboBoxFontCN(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is ComboBox cbx)
                {
                    cbx.Font = new Font("Microsoft YaHei", cbx.Font.Size);
                }
                else
                {
                    SetComboBoxFontCN(control);
                }
            }
        }

    }

}

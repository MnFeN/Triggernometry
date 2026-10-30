using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Core.Actions;
using Triggernometry.Core.Conditions;
using Triggernometry.Localization;
using Triggernometry.UI.Forms;
using Triggernometry.Utilities;

namespace Triggernometry.UI.CustomControls
{
    /// <summary>为通用配置表提供动作专用的字段编辑控件。</summary>
    public partial class ActionEditor : UserControl
    {
        internal Context UiContext { get; set; }
        internal TreeView TreeView { get; set; }
        internal ImageList Images { get; set; }
        private ActionBase action;
        private bool isBinding;
        private Action updateGeneralRules;
        private readonly Dictionary<string, ExpressionTextBox> expressions = new Dictionary<string, ExpressionTextBox>();
        private RichTextBoxHelper help;
        private TextBox documentationLink;
        private static readonly System.ComponentModel.ComponentResourceManager resources =
            new System.ComponentModel.ComponentResourceManager(typeof(ActionForm));
        internal bool ShowActionTools { get; set; }

        /// <summary>初始化动作参数编辑器并绑定界面更新事件。</summary>
        public ActionEditor()
        {
            InitializeComponent();
            DoubleBuffered = true;
            configurationEditor.CreateEditor += CreateFieldEditor;
            configurationEditor.Layout += (s, e) => help?.UpdateHeight();
            configurationEditor.ValueChanged += (s, e) =>
            {
                if (isBinding)
                    return;
                updateGeneralRules?.Invoke();
                UpdateOverlayTools();
                help?.UpdateText();
                UpdateDocumentationLink();
                UpdateOverlayFontPreview();
            };
        }

        /// <summary>载入动作草稿，并生成参数、辅助工具和帮助内容。</summary>
        internal void Bind(ActionBase value, Predicate<EditorField> include)
        {
            SuspendLayout();
            try
            {
                action = value;
                foreach (Control control in helpLayout.Controls.Cast<Control>().ToArray()) control.Dispose();
                help = null;
                documentationLink = null;
                helpLayout.Controls.Clear();
                helpLayout.RowCount = 0;
                helpLayout.RowStyles.Clear();
                expressions.Clear();
                fontPreview = null;
                fontSummary = null;
                isBinding = true;
                try
                {
                    configurationEditor.Bind(value, include, applyRules: ShowActionTools);
                    updateGeneralRules = ShowActionTools ? null : value.CreateGeneralEditorRules(configurationEditor);
                    updateGeneralRules?.Invoke();
                }
                finally { isBinding = false; }
                foreach (Control control in tools.Controls.Cast<Control>().ToArray()) control.Dispose();
                tools.Controls.Clear();
                if (ShowActionTools && (value is ActionOverlayImage || value is ActionOverlayText))
                {
                    AddTool("internal/ActionForm/visualdesign", "Visual design", DesignOverlay);
                    AddTool("ActionForm/btnHide", "Hide", HideOverlay);
                }
                tools.Visible = tools.Controls.Count > 0;
                UpdateOverlayTools();
                UpdateOverlayFontPreview(true);
                bool showHelp = ShowActionTools && GetHelpKey() != null;
                if (ShowActionTools)
                    AddActionInformation();
                if (showHelp)
                    help = new RichTextBoxHelper("help", FindForm(), helpLayout, 0, GetHelpKey) { Margin = new Padding(4) };
                helpLayout.Visible = helpLayout.Controls.Count > 0;
            }
            finally { ResumeLayout(true); }
        }

        /// <summary>换绑通用设置，保留已生成的控件与事件。</summary>
        internal void Rebind(ActionBase value)
        {
            isBinding = true;
            try
            {
                action = value;
                configurationEditor.Rebind(value, applyRules: false);
                updateGeneralRules = value.CreateGeneralEditorRules(configurationEditor);
                updateGeneralRules?.Invoke();
            }
            finally { isBinding = false; }
        }

        /// <summary>按悬浮窗操作更新设计和隐藏按钮的可用状态。</summary>
        private void UpdateOverlayTools()
        {
            if (!ShowActionTools || !(action is ActionOverlayImage || action is ActionOverlayText))
                return;
            var operation = configurationEditor.GetEditor("Operation");
            var op = (ActionOverlayImage.OperationEnum)Convert.ToInt32(operation.GetValue());
            tools.Controls[0].Enabled = operation.Control.Enabled && op == ActionOverlayImage.OperationEnum.Activate;
            tools.Controls[1].Enabled = operation.Control.Enabled && op != ActionOverlayImage.OperationEnum.DeactivateAll;
        }

        /// <summary>将当前参数控件的值写回动作草稿。</summary>
        internal void WriteValues() => configurationEditor.WriteValues();

        /// <summary>将参数和辅助工具设为只读。</summary>
        internal void SetReadOnly()
        {
            configurationEditor.SetReadOnly(true);
            tools.Enabled = false;
        }

        /// <summary>添加当前动作的说明文本和文档入口。</summary>
        private void AddActionInformation()
        {
            if (action is ActionObsControl)
                AddInformation("ActionForm/lblObsWebsocketInfo", "If the endpoint or password fields are left empty, the values defined in Trigger Configuration - Constants will be used automatically:\r\n- Endpoint: ws://(OBSWebsocketEndpoint):(OBSWebsocketPort)\r\n- Password: (OBSWebsocketPassword)\r\n\r\nIf you are using OBS v27 or older, you will have to install the OBS WebSocket plugin to use OBS remote control features. There is a simple installer available at:");
            else if (action is ActionWindowMessage)
                AddInformation("ActionForm/lblWmsgWarning", "Actions of this type may be potentially dangerous and cause damage if, for example, the trigger is fired with parameters that fall outside of the expected values. Please be aware of the risk.");
            else if (action is ActionDiskOperation)
                AddInformation("ActionForm/lblFileWarning", "Actions of this type may be potentially dangerous and cause damage if, for example, the trigger is fired with parameters that fall outside of the expected values. Please be aware of the risk.");
            else if (action is ActionLaunchProcess)
                AddInformation("ActionForm/lblProcessWarning", "Actions of this type may be potentially dangerous and cause damage if, for example, the trigger is fired with parameters that fall outside of the expected values. Please be aware of the risk.");
            else if (action is ActionExecuteScript)
                AddInformation("ActionForm/lblScriptWarning", "Actions of this type may be potentially dangerous and cause damage if, for example, the trigger is fired with parameters that fall outside of the expected values. Please be aware of the risk.");
            else if (action is ActionPlaceholder)
                AddInformation("ActionForm/lblPlaceholderNoParams", "This action has no configurable parameters.");

            if (action is ActionKeypress || action is ActionObsControl)
            {
                documentationLink = new TextBox { ReadOnly = true, TabStop = false, Tag = I18n.DoNotTranslate };
                UpdateDocumentationLink();
                Control link = WithButton(documentationLink, "…", () => System.Diagnostics.Process.Start(documentationLink.Text),
                    (Image)resources.GetObject(action is ActionObsControl ? "btnObsWebsocketLink.Image" : "btnSendKeysLink.Image"));
                AddInformationControl(link);
            }
        }

        /// <summary>翻译说明文本并添加到帮助区域。</summary>
        private void AddInformation(string key, string text) => AddInformationControl(new Label
        {
            Text = I18n.Translate(key, text), AutoSize = true, Tag = I18n.DoNotTranslate
        });

        /// <summary>将说明控件作为独立行加入帮助区域。</summary>
        private void AddInformationControl(Control control)
        {
            int row = helpLayout.RowCount++;
            helpLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(4);
            helpLayout.Controls.Add(control, 0, row);
        }

        /// <summary>按当前操作更新文档链接。</summary>
        private void UpdateDocumentationLink()
        {
            if (documentationLink == null)
                return;
            if (action is ActionObsControl)
                documentationLink.Text = "https://obsproject.com/forum/resources/obs-websocket-remote-control-of-obs-studio-made-easy.466/";
            else
                documentationLink.Text = (ActionKeypress.OperationEnum)configurationEditor.GetEditor("Operation").GetValue() == ActionKeypress.OperationEnum.SendKeys
                    ? I18n.Translate("internal/ActionForm/urlSendKey", "https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys.send")
                    : I18n.Translate("internal/ActionForm/urlWinMessage", "https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.keys");
        }

        /// <summary>添加辅助操作按钮并绑定点击事件。</summary>
        private void AddTool(string key, string text, EventHandler click)
        {
            var button = new Button { Text = I18n.Translate(key, text), Tag = I18n.DoNotTranslate, AutoSize = true };
            button.Click += click;
            tools.Controls.Add(button);
        }

        /// <summary>为复杂字段和带选择按钮的字段创建专用编辑控件。</summary>
        private void CreateFieldEditor(object sender, CreateConfigurationEditorEventArgs e)
        {
            if (e.Field.ValueType == typeof(ConditionGroup))
            {
                var viewer = new ConditionViewer { Font = Font, UiContext = UiContext, Height = 320 };
                I18n.TranslateControl("", viewer);
                e.Editor = new ConfigValueEditor(viewer, () => viewer.ConditionToEdit,
                    value => viewer.ConditionToEdit = (ConditionGroup)value, value =>
                    {
                        if (value)
                            viewer.SetReadOnly();
                    }) { FullWidth = true };
                return;
            }
            if (e.Field.ValueType == typeof(List<ActionBase>))
            {
                var viewer = new ActionViewer { Font = Font, UiContext = UiContext, TreeView = TreeView, Images = Images, Height = 280 };
                I18n.TranslateControl("", viewer);
                e.Editor = new ConfigValueEditor(viewer, () => viewer.Actions, value =>
                {
                    viewer.Actions = (List<ActionBase>)value;
                    viewer.RefreshDgv();
                }, value =>
                {
                    if (value)
                        viewer.SetReadOnly();
                }) { FullWidth = true };
                return;
            }
            if (e.Field.Options is ReferenceEditorAttribute reference)
            {
                e.Editor = CreateReferenceEditor(reference.ReferenceType);
                return;
            }
            if (e.Field.Options is PersistenceEditorAttribute persistence)
            {
                var button = new PersistenceSwitch { RelatedTextbox = expressions[persistence.VariableMember], Size = new Size(24, 24) };
                e.Editor = new ConfigValueEditor(button, () => button.IsPersistent,
                    value => button.IsPersistent = (bool)value, value => button.Enabled = !value) { InlineWith = persistence.VariableMember };
                return;
            }
            if (!(e.Field.Options is ExpressionEditorAttribute))
                return;
            ConfigValueEditor editor = ConfigEditor.CreateExpressionEditor(e.Field, UiContext);
            var expression = (ExpressionTextBox)editor.Control;
            expression.Font = Font;
            expressions[e.Field.Member.Name] = expression;
            Control control = expression;
            if (e.Field.Options is ColorEditorAttribute)
                control = WithButton(expression, "…", () => SelectColor(expression));
            else if (e.Field.Options is FontEditorAttribute)
                control = WithButton(expression, "…", SelectFont);
            else if (e.Field.Options is KeypressEditorAttribute keypress)
                control = WithButton(expression, "…", () => SelectKeypress(expression),
                    (Image)resources.GetObject(keypress.Keycode ? "btnKeycodesListen.Image" : "btnSendKeysListen.Image"));
            else if (e.Field.Options is FileEditorAttribute file)
                control = WithButton(expression, "…", () => SelectFile(expression, file.FileType),
                    (Image)resources.GetObject(file.FileType == EditorFileType.Image ? "btnBrowseAura.Image"
                        : file.FileType == EditorFileType.Executable || file.FileType == EditorFileType.Directory ? "button6.Image" : "button3.Image"));
            Control picker = control == expression ? null : control.Controls[1];
            ColorSelector preview = null;
            if (e.Field.Options is FontEditorAttribute && e.Instance is ActionOverlayText)
            {
                control = WithFontPreview(control);
                preview = fontPreview;
            }
            e.Editor = new ConfigValueEditor(control, editor.GetValue, editor.SetValue, value =>
            {
                editor.SetReadOnly(value);
                if (picker != null)
                    picker.Enabled = !value;
                if (preview != null)
                    preview.Enabled = !value;
            }) { Expression = expression };
        }

        /// <summary>确定当前动作及操作对应的帮助资源键。</summary>
        private string GetHelpKey()
        {
            string prefix;
            if (action is ActionVariableScalar)
                prefix = "rtbHelperVar";
            else if (action is ActionVariableList)
                prefix = "rtbHelperLvar";
            else if (action is ActionVariableTable)
                prefix = "rtbHelperTvar";
            else if (action is ActionVariableDict)
                prefix = "rtbHelperDict";
            else if (action is ActionKeypress)
                prefix = "rtbHelperSendKeys";
            else if (action is ActionClipboard)
                return "rtbHelperClipboard";
            else if (action is ActionTraySlider)
                return "rtbHelperTraySlider";
            else if (action is ActionNamedCallback)
                return "rtbHelperCallback";
            else if (action is ActionWindowMessage)
                return "rtbHelperWmsg";
            else if (action is ActionJsonRequest)
                return "rtbHelperJson";
            else return null;
            return prefix + configurationEditor.GetEditor("Operation").GetValue();
        }

        /// <summary>显示辅助编辑操作中的错误。</summary>
        private void ShowEditorError(Exception ex) => MessageBox.Show(this, ex.Message,
            I18n.Translate("internal/ActionForm/error", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);

        /// <summary>在编辑控件右侧添加辅助按钮，并统一处理操作异常。</summary>
        private Control WithButton(Control editor, string caption, Action click, Image image = null)
        {
            var panel = new TableLayoutPanel { AutoSize = true, ColumnCount = 2, RowCount = 1 };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            editor.Dock = DockStyle.Fill;
            editor.Margin = Padding.Empty;
            var button = new Button { Text = image == null ? caption : "", Image = image, AutoSize = true, MinimumSize = new Size(30, 0), Tag = I18n.DoNotTranslate };
            button.Click += (s, e) =>
            {
                try { click(); }
                catch (Exception ex) { ShowEditorError(ex); }
            };
            panel.Controls.Add(editor, 0, 0);
            panel.Controls.Add(button, 1, 0);
            return panel;
        }

        /// <summary>记录按键并按当前发送方式填入表达式。</summary>
        private void SelectKeypress(ExpressionTextBox text)
        {
            configurationEditor.WriteValues();
            using (var dialog = new KeyListenForm())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                var keypress = (ActionKeypress)action;
                if (keypress.Operation == ActionKeypress.OperationEnum.SendKeys)
                    text.Expression += ToSendKeys(dialog.CurrentKey);
                else if (keypress.Operation == ActionKeypress.OperationEnum.WindowMessageCombo)
                    text.Expression += (text.Expression.Length == 0 ? "" : ",") + ((int)dialog.CurrentKey).ToString(CultureInfo.InvariantCulture);
                else text.Expression = ((int)dialog.CurrentKey).ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>选择文件或目录并填入路径表达式。</summary>
        private void SelectFile(ExpressionTextBox text, EditorFileType kind)
        {
            if (kind == EditorFileType.Directory)
            {
                using (var dialog = new FolderBrowserDialog())
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                        text.Expression = dialog.SelectedPath;
                return;
            }
            using (var dialog = new OpenFileDialog())
            {
                dialog.CheckFileExists = false;
                dialog.FileName = text.Expression;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    text.Expression = dialog.FileName;
            }
        }

        private static readonly Dictionary<string, string> _sendKeysMap = new Dictionary<string, string>
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

        /// <summary>将按键及修饰键转换为 SendKeys 格式。</summary>
        private string ToSendKeys(Keys keys)
        {
            string prefix = ((keys & Keys.Control) != 0 ? "^" : "") + ((keys & Keys.Shift) != 0 ? "+" : "") + ((keys & Keys.Alt) != 0 ? "%" : "");
            Keys code = keys & Keys.KeyCode;
            if (_sendKeysMap.TryGetValue(code.ToString(), out string key))
                return prefix + key;
            MessageBox.Show(this, I18n.Translate("internal/ActionForm/sendkeyinvalid", "This key is not supported in the SendKeys method."),
                I18n.Translate("internal/ConfigurationForm/warning", "Warning"));
            return "";
        }

        /// <summary>克隆主界面节点，创建触发器、分组或仓库选择器。</summary>
        private ConfigValueEditor CreateReferenceEditor(EditorReferenceType kind)
        {
            var tree = new TreeViewEx { Font = Font, ImageList = Images, HideSelection = false, Height = 260 };
            Func<TreeNode, Guid?> getId = node =>
            {
                if (kind == EditorReferenceType.Trigger && node?.Tag is Trigger trigger)
                    return trigger.Id;
                if (kind == EditorReferenceType.Folder && node?.Tag is Folder folder)
                    return folder.Id;
                if (kind == EditorReferenceType.Repository && node?.Tag is Repository repo)
                    return repo.Id;
                return null;
            };
            tree.BeforeSelect += (s, e) => e.Cancel = !getId(e.Node).HasValue;
            tree.BeforeExpand += (s, e) => SetFolderIcon(e.Node, true);
            tree.BeforeCollapse += (s, e) => SetFolderIcon(e.Node, false);
            var nodesById = new Dictionary<Guid, TreeNode>();
            // 复制时直接跳过无关子树，不先克隆所有触发器再删除。
            TreeNode CopyNode(TreeNode source)
            {
                var node = new TreeNode(source.Text, source.ImageIndex, source.SelectedImageIndex)
                {
                    Name = source.Name, Tag = source.Tag, StateImageIndex = source.StateImageIndex,
                    Checked = source.Checked, ForeColor = source.ForeColor, BackColor = source.BackColor,
                    NodeFont = source.NodeFont, ToolTipText = source.ToolTipText
                };
                if (!string.IsNullOrEmpty(source.ImageKey))
                    node.ImageKey = source.ImageKey;
                if (!string.IsNullOrEmpty(source.SelectedImageKey))
                    node.SelectedImageKey = source.SelectedImageKey;
                if (!string.IsNullOrEmpty(source.StateImageKey))
                    node.StateImageKey = source.StateImageKey;
                foreach (TreeNode child in source.Nodes)
                {
                    if (kind == EditorReferenceType.Folder && !(child.Tag is Folder) ||
                        kind == EditorReferenceType.Repository && !(child.Tag is Repository))
                        continue;

                    node.Nodes.Add(CopyNode(child));
                }
                SetFolderIcon(node, false);
                Guid? id = getId(node);
                if (id.HasValue)
                    nodesById[id.Value] = node;
                return node;
            }
            int rootIndex = kind == EditorReferenceType.Repository ? 1 : 0;
            if (TreeView != null && TreeView.Nodes.Count > rootIndex)
            {
                tree.Nodes.Add(CopyNode(TreeView.Nodes[rootIndex]));
            }
            Guid originalId = Guid.Empty;
            return new ConfigValueEditor(tree, () => getId(tree.SelectedNode) ?? originalId, value =>
            {
                // 未选择替代节点时保留原 GUID，避免丢失暂时无法找到的引用。
                originalId = (Guid)value;
                // 仅在编辑控件中为空引用预选当前目标，不改变配置读取或动作自身的默认值。
                if (originalId == Guid.Empty)
                {
                    if (kind == EditorReferenceType.Trigger)
                        originalId = UiContext?.Trigger?.Id ?? Guid.Empty;
                    else if (kind == EditorReferenceType.Folder)
                    {
                        // 新建触发器尚未设置 Parent 时，使用主树中准备添加到的分组。
                        Folder folder = UiContext?.Trigger?.Parent
                            ?? (TreeView?.SelectedNode?.Tag as Folder)
                            ?? (TreeView?.SelectedNode?.Tag as Trigger)?.Parent;
                        originalId = folder?.Id ?? Guid.Empty;
                    }
                }
                nodesById.TryGetValue(originalId, out TreeNode node);
                tree.SelectedNode = node;
                node?.EnsureVisible();
            }, value => tree.Enabled = !value);
        }

        /// <summary>按展开状态同步分组节点图标。</summary>
        private static void SetFolderIcon(TreeNode node, bool isExpanded)
        {
            if (node.Tag is Folder folder)
                node.ImageIndex = (int)(isExpanded ? UserInterface.GetImageIndexForOpenFolder(folder) : UserInterface.GetImageIndexForClosedFolder(folder));
            else if (node.Tag is RepositoryFolder repositories)
                node.ImageIndex = (int)(isExpanded ? UserInterface.GetImageIndexForOpenFolder(repositories) : UserInterface.GetImageIndexForClosedFolder(repositories));
            node.SelectedImageIndex = node.ImageIndex;
        }

        /// <summary>选择颜色并填入 RGB 表达式。</summary>
        private void SelectColor(ExpressionTextBox text)
        {
            using (var dialog = new ColorDialog { FullOpen = true, Color = ExpressionTextBox.ParseColor(text.Expression, Color.Black) })
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    text.Expression = dialog.Color.R + ", " + dialog.Color.G + ", " + dialog.Color.B;
        }

        /// <summary>选择字体并更新字体名称、字号和样式。</summary>
        private void SelectFont()
        {
            WriteValues();
            var overlay = (ActionOverlayText)action;
            using (var font = RealPlugin.CreateFontFromDefinition(overlay.Font, overlay.EvaluateFontSize(UiContext ?? Context.Unbound), overlay.Effect))
            using (var dialog = new FontDialog { Font = font })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                overlay.Font = dialog.Font.Name;
                // 只修改字体或样式时，保留独立的字号表达式。
                if (dialog.Font.SizeInPoints != font.SizeInPoints)
                    overlay.FontSize = dialog.Font.SizeInPoints.ToString(CultureInfo.InvariantCulture);
                // 描边标志独立于字体对话框中的样式选项。
                overlay.Effect = (overlay.Effect & ActionOverlayText.EffectEnum.Outline) | (ActionOverlayText.EffectEnum)(int)dialog.Font.Style;
                ReloadEditorValues();
            }
        }

        /// <summary>使用当前设置的动作副本隐藏指定悬浮窗。</summary>
        private void HideOverlay(object sender, EventArgs e)
        {
            WriteValues();
            ActionBase hidden = action.Copy();
            if (hidden is ActionOverlayImage image)
                image.Operation = ActionOverlayImage.OperationEnum.Deactivate;
            if (hidden is ActionOverlayText text)
                text.Operation = ActionOverlayText.OperationEnum.Deactivate;
            hidden.Execute(new Context(UiContext?.Trigger) { testByPlaceholder = true });
        }

        /// <summary>打开可视化设计器，并将布局结果写回动作草稿。</summary>
        private void DesignOverlay(object sender, EventArgs e)
        {
            try
            {
                WriteValues();
                bool isImageOverlay = action is ActionOverlayImage;
                var context = new Context(UiContext?.Trigger) { testByPlaceholder = !isImageOverlay };
                using (var dialog = new AuraDesignForm(isImageOverlay ? AuraContainerForm.AuraTypeEnum.Image : AuraContainerForm.AuraTypeEnum.Text))
                {
                    I18n.TranslateForm(dialog);
                    Image bitmap = null;
                    Font font = null;
                    try
                    {
                        if (action is ActionOverlayImage imageAction)
                        {
                            bitmap = AuraContainerForm.LoadImageData(UiContext.Plugin, context.EvaluateStringExpression(null, null, imageAction.Filename));
                            dialog.SetImage((Bitmap)bitmap);
                            dialog.SetImageMode(imageAction.SizeMode);
                        }
                        else if (action is ActionOverlayText textAction)
                        {
                            font = RealPlugin.CreateFontFromDefinition(textAction.Font, textAction.EvaluateFontSize(context), textAction.Effect);
                            dialog.AuraFont = font;
                            dialog.TextAlignment = textAction.Alignment;
                            dialog.TextColor = ExpressionTextBox.ParseColor(textAction.TextColor, Color.Black);
                            dialog.OutlineColor = ExpressionTextBox.ParseColor(textAction.OutlineColor, Color.Empty);
                            dialog.UseOutline = dialog.OutlineColor != Color.Empty;
                        }
                        var type = action.GetType();
                        string[] properties = { nameof(ActionOverlayImage.X_0), nameof(ActionOverlayImage.Y_0), nameof(ActionOverlayImage.W_0), nameof(ActionOverlayImage.H_0) };
                        string[] values = properties.Select(name => (string)type.GetProperty(name).GetValue(action)).ToArray();
                        if (values.All(string.IsNullOrWhiteSpace))
                            dialog.StartPosition = FormStartPosition.CenterParent;
                        else
                        {
                            int[] rectangle = values.Select(value => (int)Math.Round(context.EvaluateNumericExpression(null, null, value))).ToArray();
                            dialog.StartPosition = FormStartPosition.Manual;
                            dialog.SetBounds(rectangle[0], rectangle[1], Math.Max(1, rectangle[2]), Math.Max(1, rectangle[3]));
                        }
                        if (dialog.ShowDialog(this) != DialogResult.OK)
                            return;
                        int[] bounds = { dialog.Left, dialog.Top, dialog.Width, dialog.Height };
                        for (int i = 0; i < properties.Length; i++) type.GetProperty(properties[i]).SetValue(action, bounds[i].ToString(CultureInfo.InvariantCulture));
                        var opacity = type.GetProperty(nameof(ActionOverlayImage.O_0));
                        if (string.IsNullOrWhiteSpace((string)opacity.GetValue(action)))
                            opacity.SetValue(action, "100");
                        if (action is ActionOverlayImage resultImage)
                            resultImage.SizeMode = dialog.GetImageDisplay();
                        if (action is ActionOverlayText resultText)
                            resultText.Alignment = dialog.TextAlignment;
                        ReloadEditorValues();
                    }
                    finally { font?.Dispose(); bitmap?.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, I18n.Translate("internal/ActionForm/error", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

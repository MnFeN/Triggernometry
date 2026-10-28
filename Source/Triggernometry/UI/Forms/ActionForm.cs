using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Core.Actions;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;

namespace Triggernometry.UI.Forms
{
    /// <summary>编辑动作副本，并承载参数、条件、通用设置和描述区域。</summary>
    public partial class ActionForm : MemoryForm<ActionForm>
    {
        // 菜单和鼠标滚轮共用此显示顺序，新增类型时直接在列表中插入。
        private static readonly (Type Type, ActionBase.ActionAttribute Metadata)[] actionTypes = new[]
        {
            typeof(ActionBeep),
            typeof(ActionPlaySound),
            typeof(ActionPlaySpeech),

            typeof(ActionMessageBox),
            typeof(ActionOverlayText),
            typeof(ActionOverlayImage),

            typeof(ActionVariableScalar),
            typeof(ActionVariableList),
            typeof(ActionVariableTable),
            typeof(ActionVariableDict),

            typeof(ActionLogMessage),
            typeof(ActionNamedCallback),
            typeof(ActionExecuteScript),
            typeof(ActionLoop),
            typeof(ActionMutex),

            typeof(ActionTriggerOperation),
            typeof(ActionFolderOperation),
            typeof(ActionRepository),

            typeof(ActionMouse),
            typeof(ActionKeypress),
            typeof(ActionWindowMessage),
            typeof(ActionLaunchProcess),
            typeof(ActionDiskOperation),

            typeof(ActionActInteraction),
            typeof(ActionJsonRequest),
            typeof(ActionDiscordWebhook),
            typeof(ActionObsControl),
            typeof(ActionLiveSplitControl),

            typeof(ActionPlaceholder),
        }.Select(type => (Type: type, Metadata: type.GetCustomAttribute<ActionBase.ActionAttribute>())).ToArray();

        private static readonly ComponentResourceManager resources = new ComponentResourceManager(typeof(ActionForm));

        // 与 ConfigurationForm2 中的界面字体选择器共用。
        public class FontInfoContainer
        {
            public string Name { get; set; }
            public float Size { get; set; }
            public ActionOverlayText.EffectEnum Effect { get; set; }
        }

        private ActionBase draft;
        private bool isReadOnly;
        private int actionTypeWheelDelta;
        private ActionEditor specificEditor;
        private ActionEditor generalEditor;
        private ConditionViewer cndCondition;

        internal Context UiContext { get; }
        internal Trigger ParentTrigger => UiContext?.Trigger;
        internal RealPlugin Plugin => UiContext?.Plugin;
        private readonly TreeView sourceTree;
        private readonly ImageList images;

        /// <summary>初始化动作编辑窗口并载入独立的编辑草稿。</summary>
        public ActionForm(ActionBase action, Context uiContext, TreeView treeView, ImageList imageList)
        {
            UiContext = uiContext;
            sourceTree = treeView;
            images = imageList;
            InitializeComponent();
            expDescBgColor.UiContext = expDescTextColor.UiContext = UiContext;
            txtDescription.Tag = I18n.DoNotTranslate;
            txtDescription.GotFocus += ExpressionTextBox.ReplaceIncompleteLineBreaksInClipboard;
            btnActionType.Tag = I18n.DoNotTranslate;
            BuildActionTypeMenu();
            btnActionType.MouseWheel += ActionType_MouseWheel;
            btnTest.Tag = I18n.DoNotTranslate;
            btnTest.Text = I18n.Translate("ActionViewer/ctxTestAction", "Test Action");
            AddTestOption("ActionViewer/ctxTestPlaceholder", "Test action with placeholder values", false, false);
            AddTestOption("ActionViewer/ctxTestLive", "Test action with live values", true, false);
            AddTestOption("ActionViewer/ctxTestLiveIgnoreCnd", "Test action with live values (ignore conditions)", true, true);
            RestoredSavedDimensions();
            SettingsFromAction(action);
        }

        /// <summary>按集中定义的顺序生成动作分类菜单。</summary>
        private void BuildActionTypeMenu()
        {
            foreach (var group in actionTypes.GroupBy(item => item.Metadata.Category))
            {
                var category = new ToolStripMenuItem(GetCategoryName(group.Key))
                {
                    Image = (Image)resources.GetObject("actionCategory." + group.Key)
                };
                category.MouseEnter += ActionTypeCategory_MouseEnter;
                // 只有一个动作的分类可直接点击选择。
                if (group.Count() == 1)
                {
                    Type type = group.First().Type;
                    category.Tag = type;
                    category.Image = (Image)resources.GetObject("actionType." + type.Name);
                    category.Click += (s, e) => SelectActionType((Type)category.Tag);
                }
                else
                {
                    foreach (var item in group)
                    {
                        var choice = new ToolStripMenuItem(I18n.Translate(item.Metadata.TranslationKey, item.Metadata.Name))
                        {
                            Tag = item.Type,
                            Image = (Image)resources.GetObject("actionType." + item.Type.Name)
                        };
                        choice.Click += (s, e) => SelectActionType((Type)choice.Tag);
                        category.DropDownItems.Add(choice);
                    }
                }
                actionTypeMenu.Items.Add(category);
            }
        }

        /// <summary>鼠标移入分类时立即展开其子菜单。</summary>
        private void ActionTypeCategory_MouseEnter(object sender, EventArgs e)
        {
            var category = (ToolStripMenuItem)sender;
            foreach (ToolStripMenuItem sibling in actionTypeMenu.Items)
            {
                if (sibling != category) sibling.HideDropDown();
            }
            category.Select();
            if (category.HasDropDownItems) category.ShowDropDown();
        }

        /// <summary>按动作列表顺序滚动切换类型，并累计不足一格的滚轮量。</summary>
        private void ActionType_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e is HandledMouseEventArgs handled) handled.Handled = true;
            if (isReadOnly || actionTypeMenu.Visible) return;
            actionTypeWheelDelta += e.Delta;
            int steps = actionTypeWheelDelta / SystemInformation.MouseWheelScrollDelta;
            actionTypeWheelDelta %= SystemInformation.MouseWheelScrollDelta;
            if (steps == 0) return;
            int current = Array.FindIndex(actionTypes, item => item.Type == draft.GetType());
            int next = Math.Max(0, Math.Min(actionTypes.Length - 1, current - steps));
            SelectActionType(actionTypes[next].Type);
        }

        /// <summary>取得动作分类的本地化名称。</summary>
        private static string GetCategoryName(ActionBase.ActionAttribute.CategoryType category)
        {
            switch (category)
            {
                case ActionBase.ActionAttribute.CategoryType.Audio:
                    return I18n.Translate("internal/ActionForm/categoryAudio", "Audio");
                case ActionBase.ActionAttribute.CategoryType.Display:
                    return I18n.Translate("internal/ActionForm/categoryDisplay", "Information display");
                case ActionBase.ActionAttribute.CategoryType.Variables:
                    return I18n.Translate("internal/ActionForm/categoryVariables", "Variables");
                case ActionBase.ActionAttribute.CategoryType.Programming:
                    return I18n.Translate("internal/ActionForm/categoryProgramming", "Programming and flow control");
                case ActionBase.ActionAttribute.CategoryType.TriggerManagement:
                    return I18n.Translate("internal/ActionForm/categoryTriggerManagement", "Trigger management");
                case ActionBase.ActionAttribute.CategoryType.SystemInteraction:
                    return I18n.Translate("internal/ActionForm/categorySystemInteraction", "System interaction");
                case ActionBase.ActionAttribute.CategoryType.ExternalInteraction:
                    return I18n.Translate("internal/ActionForm/categoryExternalInteraction", "External interaction");
                case ActionBase.ActionAttribute.CategoryType.Placeholder:
                    return I18n.Translate("internal/ActionForm/categoryPlaceholder", "Placeholder");
                default:
                    throw new ArgumentOutOfRangeException(nameof(category));
            }
        }

        /// <summary>同步当前动作对应的菜单选中状态和显示文本。</summary>
        private void UpdateActionTypeSelection()
        {
            Type selectedType = draft.GetType();
            foreach (ToolStripMenuItem category in actionTypeMenu.Items)
            {
                category.Checked = category.Tag is Type type && type == selectedType;
                foreach (ToolStripMenuItem choice in category.DropDownItems)
                {
                    choice.Checked = (Type)choice.Tag == selectedType;
                    if (choice.Checked) category.Checked = true;
                }
            }
            var selected = actionTypes.First(item => item.Type == selectedType);
            btnActionType.Text = I18n.Translate(selected.Metadata.TranslationKey, selected.Metadata.Name);
        }

        /// <summary>复制待编辑动作并刷新界面。</summary>
        internal void SettingsFromAction(ActionBase action)
        {
            draft = action.Copy();
            UpdateActionTypeSelection();
            BindEditors();
        }

        /// <summary>为指定标签页创建参数编辑器并传入界面上下文。</summary>
        private ActionEditor CreateEditor(TabPage page)
        {
            var editor = new ActionEditor { Dock = DockStyle.Fill, UiContext = UiContext, TreeView = sourceTree, Images = images, ShowActionTools = page == tabActionSettings };
            page.Controls.Add(editor);
            if (isReadOnly) editor.SetReadOnly();
            return editor;
        }

        private const int WM_SETREDRAW = 0x000B;

        /// <summary>向窗口发送原生消息以控制重绘。</summary>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);

        /// <summary>刷新动作各编辑区域，并在切换期间暂停窗口重绘。</summary>
        private void BindEditors()
        {
            // 仅暂停布局无法阻止原生子控件重绘，切换期间还需暂停窗口重绘。
            bool shouldSuspendRedraw = IsHandleCreated && Visible;
            if (shouldSuspendRedraw) SendMessage(Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
            SuspendLayout();
            tabs.SuspendLayout();
            try
            {
                if (specificEditor == null) specificEditor = CreateEditor(tabActionSettings);
                specificEditor.Bind(draft, field => field.Member.DeclaringType != typeof(ActionBase));
                generalEditor?.Bind(draft, field => field.Member.DeclaringType == typeof(ActionBase));
                if (cndCondition != null) cndCondition.ConditionToEdit = draft.Condition;
                txtDescription.Text = draft.Description;
                chkOverrideDesc.Checked = draft.DescriptionOverride;
                expDescBgColor.Expression = draft.DescBgColor;
                expDescTextColor.Expression = draft.DescTextColor;
            }
            finally
            {
                try
                {
                    tabs.ResumeLayout(true);
                    ResumeLayout(true);
                }
                finally
                {
                    if (shouldSuspendRedraw)
                    {
                        SendMessage(Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
                        Invalidate(true);
                    }
                }
            }
        }

        /// <summary>首次打开标签页时创建对应的编辑器。</summary>
        private void EnsureSelectedEditor()
        {
            if (tabs.SelectedTab == tabGeneral && generalEditor == null)
            {
                generalEditor = CreateEditor(tabGeneral);
                generalEditor.Bind(draft, field => field.Member.DeclaringType == typeof(ActionBase));
            }
            if (tabs.SelectedTab == tabActionCondition && cndCondition == null)
            {
                cndCondition = new ConditionViewer { Font = tabActionCondition.Font, Dock = DockStyle.Fill, UiContext = UiContext, Name = "cndCondition" };
                I18n.TranslateControl("", cndCondition);
                tabActionCondition.Controls.Add(cndCondition);
                cndCondition.ConditionToEdit = draft.Condition;
                if (isReadOnly) cndCondition.SetReadOnly();
            }
        }

        /// <summary>切换标签页后确保对应编辑器已载入。</summary>
        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (draft != null) EnsureSelectedEditor();
        }

        /// <summary>切换动作类型，并将通用设置复制到新动作。</summary>
        private void SelectActionType(Type type)
        {
            if (isReadOnly || draft == null || type == draft.GetType() || !TryWriteValues()) return;
            ActionBase replacement = type == typeof(ActionObsControl)
                ? new ActionObsControl { Endpoint = "", Password = "" } : (ActionBase)Activator.CreateInstance(type);
            draft.CopyCommonPropertiesTo(replacement);
            draft = replacement;
            UpdateActionTypeSelection();
            BindEditors();
        }

        /// <summary>生成包含当前设置和父级关系的动作副本。</summary>
        internal ActionBase SettingsToAction()
        {
            WriteValues();
            ActionBase result = draft.Copy();
            ActionBase.RebuildParentage(new[] { result }, ParentTrigger);
            return result;
        }

        /// <summary>将参数、条件和描述区域的内容写回动作草稿。</summary>
        private void WriteValues()
        {
            specificEditor?.WriteValues();
            generalEditor?.WriteValues();
            if (isReadOnly) return;
            if (cndCondition != null) draft.Condition = cndCondition.ConditionToEdit;
            if (txtDescription.Text != (draft.Description ?? "")) draft.Description = txtDescription.Text;
            draft.DescriptionOverride = chkOverrideDesc.Checked;
            if (expDescBgColor.Expression != (draft.DescBgColor ?? "")) draft.DescBgColor = expDescBgColor.Expression;
            if (expDescTextColor.Expression != (draft.DescTextColor ?? "")) draft.DescTextColor = expDescTextColor.Expression;
        }

        /// <summary>校验并保存界面值，失败时显示提示。</summary>
        private bool TryWriteValues()
        {
            try { WriteValues(); return true; }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, I18n.Translate("internal/ConfigurationForm/warning", "Warning"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        /// <summary>将动作窗口切换为只读查看模式。</summary>
        internal void SetReadOnly()
        {
            isReadOnly = true;
            btnActionType.Enabled = false;
            btnOk.Visible = btnOk.Enabled = false;
            panel6.Visible = false;
            btnCancel.Dock = DockStyle.Fill;
            btnCancel.Text = I18n.Translate("internal/SimpleInputForm/close", "Close");
            btnCancel.Tag = I18n.DoNotTranslate;
            specificEditor?.SetReadOnly();
            generalEditor?.SetReadOnly();
            cndCondition?.SetReadOnly();
            txtDescription.ReadOnly = true;
            chkOverrideDesc.Enabled = false;
            expDescBgColor.ReadOnly = expDescTextColor.ReadOnly = true;
        }

        /// <summary>关闭前校验保存内容，并登记表达式中的动态变量。</summary>
        private void ActionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK && !TryWriteValues()) e.Cancel = true;
            if (!e.Cancel) ExpressionTextBox.RegisterAllDynamicVarNamesOnForm(this);
        }

        /// <summary>添加指定测试方式的下拉菜单项。</summary>
        private void AddTestOption(string key, string text, bool useLiveValues, bool ignoreConditions)
        {
            var item = testMenu.Items.Add(I18n.Translate(key, text));
            item.Click += (s, e) => TestAction(useLiveValues, ignoreConditions);
        }

        /// <summary>按配置中的默认方式测试当前动作。</summary>
        private void BtnTest_Click(object sender, EventArgs e) =>
            TestAction(Plugin.cfg.TestLiveByDefault, Plugin.cfg.TestIgnoreConditionsByDefault);

        /// <summary>使用当前动作副本及指定的取值和条件选项进行测试。</summary>
        private void TestAction(bool useLiveValues, bool ignoreConditions)
        {
            if (!TryWriteValues()) return;
            ActionBase action = SettingsToAction();
            if (ignoreConditions) action.Condition = new Core.Conditions.ConditionGroup();
            Action execute = () => action.Execute(null, new Context(ParentTrigger)
            {
                testByPlaceholder = !useLiveValues,
                soundhook = Plugin.SoundPlaybackSmart,
                ttshook = Plugin.TtsPlaybackSmart
            });
            if (action is ActionKeypress keypress && keypress.Operation == ActionKeypress.OperationEnum.SendKeys)
            {
                if (MessageBox.Show(this, I18n.Translate("internal/ActionForm/confirmkeypress",
                    "The keypresses you defined will be sent in two seconds after you hit OK, to allow you to change to the window you want to send the keypresses to."),
                    I18n.Translate("internal/ActionForm/confirmkeypressdelay", "Confirm"), MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information) != DialogResult.OK) return;
                var timer = new Timer(components) { Interval = 2000 };
                timer.Tick += (s, e) => { timer.Stop(); timer.Dispose(); execute(); };
                timer.Start();
            }
            else execute();
        }
    }
}

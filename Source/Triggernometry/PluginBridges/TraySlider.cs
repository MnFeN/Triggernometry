using System;
using System.Linq;
using System.Windows.Forms;

namespace Triggernometry.PluginBridges
{
    /// <summary> TraySlider with 1 button. </summary>
    public sealed class TraySlider1 : TraySliderBase
    {

        public TraySlider1(bool addNotification, int durationMs = 15000, bool forceShow = true) : base("OneButton")
        { 
            AddNotification = addNotification;
            DurationMs = durationMs;
            ForceShow = forceShow;
        }

        public Button Button1 => ButtonOK;
    }

    /// <summary> TraySlider with 2 buttons. </summary>
    public sealed class TraySlider2 : TraySliderBase
    {
        public TraySlider2(bool addNotification, int durationMs = 15000, bool forceShow = true) : base("TwoButton")
        {
            AddNotification = addNotification;
            DurationMs = durationMs;
            ForceShow = forceShow;
        }

        public Button Button1 => ButtonSW;
        public Button Button2 => ButtonSE;
    }

    /// <summary> TraySlider with 4 buttons. </summary>
    public sealed class TraySlider4 : TraySliderBase
    {
        public TraySlider4(bool addNotification, int durationMs = 15000, bool forceShow = true) : base("FourButton")
        {
            AddNotification = addNotification;
            DurationMs = durationMs;
            ForceShow = forceShow;
        }

        public Button Button1 => ButtonNW;
        public Button Button2 => ButtonNE;
        public Button Button3 => ButtonSW;
        public Button Button4 => ButtonSE;
    }

    public abstract class TraySliderBase : IDisposable
    {
        private static readonly Type TraySliderType;
        private static readonly Type ButtonLayoutEnumType;

        private readonly object _tray;

        static TraySliderBase()
        {
            TraySliderType = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetType("Advanced_Combat_Tracker.TraySlider", false))
                .FirstOrDefault(t => t != null);

            if (TraySliderType == null)
                throw new InvalidOperationException("Cannot find Advanced_Combat_Tracker.TraySlider.");

            ButtonLayoutEnumType = TraySliderType.GetNestedType("ButtonLayoutEnum");
            if (ButtonLayoutEnumType == null)
                throw new InvalidOperationException("Cannot find TraySlider.ButtonLayoutEnum.");
        }

        protected TraySliderBase(string btnLayoutEnumName)
        {
            _tray = Activator.CreateInstance(TraySliderType);
            SetProperty("ButtonLayout", Enum.Parse(ButtonLayoutEnumType, btnLayoutEnumName));
        }

        /*
        public object NativeObject => _tray;

        protected string Title
        {
            get { return TrayTitle.Text; }
            set { TrayTitle.Text = value ?? ""; }
        }

        protected string Text
        {
            get { return TrayText.Text; }
            set { TrayText.Text = value ?? ""; }
        }
        */

        protected bool AddNotification
        {
            get { return (bool)GetProperty("AddNotification"); }
            set { SetProperty("AddNotification", value); }
        }

        protected int DurationMs
        {
            get { return (int)GetProperty("ShowDurationMs"); }
            set { SetProperty("ShowDurationMs", value); }
        }

        protected bool ForceShow
        {
            get { return (bool)GetProperty("ForceShow"); }
            set { SetProperty("ForceShow", value); }
        }

        protected Label TrayTitle => GetControl<Label>("TrayTitle");
        protected Label TrayText => GetControl<Label>("TrayText");
        protected Button ButtonOK => GetControl<Button>("ButtonOK");
        protected Button ButtonNW => GetControl<Button>("ButtonNW");
        protected Button ButtonNE => GetControl<Button>("ButtonNE");
        protected Button ButtonSW => GetControl<Button>("ButtonSW");
        protected Button ButtonSE => GetControl<Button>("ButtonSE");

        public void Show(string text, string title = "")
        {
            InvokeMethod("ShowTraySlider", text ?? "", title ?? "");
        }

        public void Dispose() => (_tray as IDisposable)?.Dispose();

        private object GetProperty(string name)
        {
            var prop = TraySliderType.GetProperty(name)
                ?? throw new MissingMemberException(TraySliderType.FullName, name);

            return prop.GetValue(_tray, null);
        }

        private void SetProperty(string name, object value)
        {
            var prop = TraySliderType.GetProperty(name)
                ?? throw new MissingMemberException(TraySliderType.FullName, name);

            prop.SetValue(_tray, value, null);
        }

        private T GetControl<T>(string name) where T : Control
        {
            return (T)GetProperty(name);
        }

        private object InvokeMethod(string name, params object[] args)
        {
            Type[] argTypes = new Type[args.Length];

            for (int i = 0; i < args.Length; i++)
                argTypes[i] = args[i] == null ? typeof(object) : args[i].GetType();

            var method = TraySliderType.GetMethod(name, argTypes)
                ?? throw new MissingMethodException(TraySliderType.FullName, name);

            return method.Invoke(_tray, args);
        }
    }
}

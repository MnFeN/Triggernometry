using System.Xml.Serialization;

namespace Triggernometry.Core.Conditions
{

    [XmlInclude(typeof(ConditionSingle))]
    [XmlInclude(typeof(ConditionGroup))]    
    public abstract class ConditionComponent
    {

        public delegate void ChangeDelegate(ConditionComponent re);
        public event ChangeDelegate OnPropertyChange;

        [XmlAttribute]
        public bool Enabled { get; set; } = true;

        internal ConditionGroup Parent { get; set; } = null;

        protected void TriggerOnPropertyChange()
        {
            OnPropertyChange?.Invoke(this);
        }

        internal abstract bool CheckCondition(Context ctx, Context.LoggerDelegate logger, object o);
        internal abstract ConditionComponent Duplicate();
    }

}

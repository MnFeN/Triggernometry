using System.ComponentModel;
using System.Xml.Serialization;

namespace Triggernometry.Core.Variables
{

    /// <summary>
    /// Defines a folder's variable registration or a comment row kept alongside the definitions.
    /// </summary>
    public class RegisteredVariable
    {

        [XmlAttribute]
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsComment { get; set; } = false;

        [XmlAttribute]
        public string Name { get; set; } = "";

        [XmlAttribute]
        public string Value { get; set; } = "";

        [XmlAttribute]
        [DefaultValue(false)]
        public bool LoadWhenActive { get; set; } = false;

        /// <summary>
        /// Copies a definition without sharing edits between forms or pasted rows.
        /// </summary>
        public RegisteredVariable Duplicate()
        {
            return (RegisteredVariable)MemberwiseClone();
        }

    }

}

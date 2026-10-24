using System.ComponentModel;
using System.Xml.Serialization;

namespace Triggernometry.Core.Variables
{

    /// <summary>
    /// Defines a variable registration saved with a folder, including its key, initial value, and load mode.
    /// </summary>
    public class RegisteredVariable
    {

        [XmlAttribute]
        public string Name { get; set; } = "";

        [XmlAttribute]
        public string Value { get; set; } = "";

        [XmlAttribute]
        [DefaultValue(false)]
        public bool LoadWhenActive { get; set; } = false;

    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Serialization
{
    /// <summary>Reads legacy and typed action elements and writes the typed XML format.</summary>
    public sealed class ActionXmlCollection : IXmlSerializable
    {
        private static readonly Dictionary<Type, Lazy<XmlSerializer>> serializers = typeof(ActionBase)
            .GetCustomAttributes<XmlIncludeAttribute>()
            .ToDictionary(include => include.Type, include => new Lazy<XmlSerializer>(() => new XmlSerializer(include.Type)));

        private static readonly Dictionary<string, Type> actionTypes = serializers.Keys
            .ToDictionary(type => type.GetCustomAttribute<XmlRootAttribute>().ElementName, type => type);

        private static readonly Lazy<XmlSerializer> legacySerializer = new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(ActionOld)));

        public List<ActionBase> Actions { get; private set; } = new List<ActionBase>();

        public ActionXmlCollection() { }

        public ActionXmlCollection(List<ActionBase> actions)
        {
            Actions = actions;
        }

        public XmlSchema GetSchema() => null;

        /// <summary>Reads action children in their stored order, including mixed legacy and typed nodes.</summary>
        public void ReadXml(XmlReader reader)
        {
            bool isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (isEmpty) return;
            while (reader.MoveToContent() == XmlNodeType.Element)
                Actions.Add(ReadAction(reader));
            reader.ReadEndElement();
        }

        /// <summary>Writes each concrete action with its declared element name.</summary>
        public void WriteXml(XmlWriter writer)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            foreach (ActionBase action in Actions)
                serializers[action.GetType()].Value.Serialize(writer, action, ns);
        }

        private static ActionBase ReadAction(XmlReader reader)
        {
            if (reader.LocalName == "Action")
                return ((ActionOld)legacySerializer.Value.Deserialize(reader)).ConvertToNewAction();
            if (actionTypes.TryGetValue(reader.LocalName, out Type type))
                return (ActionBase)serializers[type].Value.Deserialize(reader);
            throw new XmlException(I18n.Translate("internal/Action/unknownxmlelement", "Unknown action XML element '{0}'.", reader.LocalName));
        }

        /// <summary>Uses a single typed element or the existing bundle envelope for clipboard data.</summary>
        internal static string ToXml(List<ActionBase> actions)
        {
            if (actions == null || actions.Count == 0) return string.Empty;
            using (var text = new StringWriter())
            using (var writer = XmlWriter.Create(text, new XmlWriterSettings { Indent = true }))
            {
                var collection = new ActionXmlCollection(actions.OrderBy(action => action.OrderNumber).ToList());
                writer.WriteStartDocument();
                if (actions.Count > 1)
                {
                    writer.WriteStartElement("ActionBundle");
                    writer.WriteStartElement("Actions");
                }
                collection.WriteXml(writer);
                if (actions.Count > 1)
                {
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndDocument();
                writer.Flush();
                return text.ToString();
            }
        }

        /// <summary>Accepts legacy and typed actions, both alone and inside an action bundle.</summary>
        internal static List<ActionBase> FromXml(string text)
        {
            using (var input = new StringReader(text))
            using (var reader = XmlReader.Create(input))
            {
                reader.MoveToContent();
                if (reader.LocalName != "ActionBundle")
                    return new List<ActionBase> { ReadAction(reader) };
                var collection = new ActionXmlCollection();
                bool isEmpty = reader.IsEmptyElement;
                reader.ReadStartElement();
                if (isEmpty) return collection.Actions;
                while (reader.MoveToContent() == XmlNodeType.Element)
                {
                    if (reader.LocalName == "Actions") collection.ReadXml(reader);
                    else reader.Skip();
                }
                reader.ReadEndElement();
                return collection.Actions;
            }
        }
    }
}

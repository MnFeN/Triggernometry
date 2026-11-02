using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Triggernometry.Core.Serialization
{

    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader x)
        {
            if (x.IsEmptyElement == true)
            {
                return;
            }
            x.Read();
            XmlSerializer xsk = new XmlSerializer(typeof(TKey));
            XmlSerializer xsv = new XmlSerializer(typeof(TValue));
            while (x.NodeType != XmlNodeType.EndElement)
            {
                TKey key;
                TValue value;
                x.ReadStartElement("Item");
                x.ReadStartElement("Key");
                key = (TKey)xsk.Deserialize(x);
                x.ReadEndElement();
                x.ReadStartElement("Value");
                value = (TValue)xsv.Deserialize(x);
                x.ReadEndElement();
                x.ReadEndElement();
                this[key] = value;
                x.MoveToContent();
            }
            x.Read();
        }

        public void WriteXml(XmlWriter x)
        {
            XmlSerializer xsk = new XmlSerializer(typeof(TKey));
            XmlSerializer xsv = new XmlSerializer(typeof(TValue));
            foreach (TKey k in Keys)
            {
                x.WriteStartElement("Item");
                x.WriteStartElement("Key");
                XmlSerialization.Serialize(x, xsk, k);
                x.WriteEndElement();
                x.WriteStartElement("Value");
                XmlSerialization.Serialize(x, xsv, this[k]);
                x.WriteEndElement();
                x.WriteEndElement();
            }
        }

    }

}

using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Triggernometry.Core.Serialization
{
    /// <summary>Shared XML output format for clipboard text and UTF-8 files.</summary>
    internal static class XmlSerialization
    {
        internal static readonly Encoding Utf8 = new UTF8Encoding(false);

        private static readonly XmlSerializerNamespaces namespaces = CreateNamespaces();

        private static XmlSerializerNamespaces CreateNamespaces()
        {
            var result = new XmlSerializerNamespaces();
            result.Add("", "");
            return result;
        }

        internal static XmlWriter CreateWriter(TextWriter output)
            => XmlWriter.Create(output, new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
            });

        internal static string Serialize(XmlSerializer serializer, object value)
        {
            using (var text = new StringWriter(CultureInfo.InvariantCulture))
            using (var writer = new DeclarationlessXmlTextWriter(text))
            {
                Serialize(writer, serializer, value);
                writer.Flush();
                return text.ToString();
            }
        }

        internal static void Serialize(XmlWriter writer, XmlSerializer serializer, object value)
            => serializer.Serialize(writer, value, namespaces);

        // Keep XmlSerializer's existing escaping, which configuration sanitization relies on.
        private sealed class DeclarationlessXmlTextWriter : XmlTextWriter
        {
            internal DeclarationlessXmlTextWriter(TextWriter output) : base(output)
            {
                Formatting = Formatting.Indented;
            }

            public override void WriteStartDocument() { }
            public override void WriteStartDocument(bool standalone) { }
        }
    }
}

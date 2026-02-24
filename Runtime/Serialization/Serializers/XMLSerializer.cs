using System;
using System.IO;
using System.Xml.Serialization;

namespace Serializers {
    public sealed class XMLSerializer : ISerializer {
        public string FileExtension => ".xml";

        public string Serialize<T>(T obj) {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var xmlSerializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        public T Deserialize<T>(string fileText) {
            if (string.IsNullOrEmpty(fileText)) throw new ArgumentNullException(nameof(fileText));

            var xmlSerializer = new XmlSerializer(typeof(T));
            using var stringReader = new StringReader(fileText);
            return (T)xmlSerializer.Deserialize(stringReader);
        }
    }
}
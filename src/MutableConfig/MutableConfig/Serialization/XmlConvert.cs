using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MutableConfig.Serialization {
    public static class XmlConvert {
        public static string SerializeObject<T>(T obj) {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringWriter = new Utf8StringWriter();
            xmlSerializer.Serialize(stringWriter, obj);
            var xml = stringWriter.ToString();
            stringWriter.Dispose();
            return xml;
        }

        public static T DeserializeObject<T>(string xml) {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringReader = new StringReader(xml);
            var deserializedObject = (T)xmlSerializer.Deserialize(stringReader);
            stringReader.Dispose();
            return deserializedObject;
        }
    }

    public class Utf8StringWriter : StringWriter {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
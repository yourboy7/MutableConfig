namespace MutableConfig.Serialization {
    public class XmlConfigSerializer : IConfigSerializer {
        public string SerializeObject<T>(T obj) => 
            XmlConvert.SerializeObject(obj);

        public T DeserializeObject<T>(string text) => 
            XmlConvert.DeserializeObject<T>(text);
    }
}
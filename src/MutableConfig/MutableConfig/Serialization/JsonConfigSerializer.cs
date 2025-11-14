using Newtonsoft.Json;

namespace MutableConfig.Serialization {
    public class JsonConfigSerializer : IConfigSerializer {
        public string SerializeObject<T>(T obj) => 
            JsonConvert.SerializeObject(obj, Formatting.Indented);

        public T DeserializeObject<T>(string text) => 
            JsonConvert.DeserializeObject<T>(text);
    }
}
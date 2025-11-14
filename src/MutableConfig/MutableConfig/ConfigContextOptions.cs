using MutableConfig.Serialization;
using System;
using System.IO;

namespace MutableConfig {
    public class ConfigContextOptions<T> where T : class, new() {
        public string BasePath { get; private set; } = AppContext.BaseDirectory;
        public T DefaultConfig { get; private set; } = new T();
        public IConfigSerializer Serializer { get; private set; } = new JsonConfigSerializer();

        public ConfigContextOptions<T> SetBasePath(string path) {
            BasePath = path;
            return this;
        }

        public ConfigContextOptions<T> SetupDefaultConfigIfNotExists(
            T defaultConfig) {
            DefaultConfig = defaultConfig;
            return this;
        }

        public ConfigContextOptions<T> LoadConfigFromFile(string filePath) {    
            var isJsonFile = Path.GetExtension(filePath) == ".json";
            var text = File.ReadAllText(filePath);
            var serializer = isJsonFile ? (IConfigSerializer)new JsonConfigSerializer() : new XmlConfigSerializer();
            DefaultConfig = serializer.DeserializeObject<T>(text);
            return this;
        }

        public ConfigContextOptions<T> UseJson()
            => UseSerializer(new JsonConfigSerializer());

        public ConfigContextOptions<T> UseXml()
            => UseSerializer(new XmlConfigSerializer());

        private ConfigContextOptions<T> UseSerializer(IConfigSerializer serializer) {
            Serializer = serializer;
            return this;
        }
    }
}
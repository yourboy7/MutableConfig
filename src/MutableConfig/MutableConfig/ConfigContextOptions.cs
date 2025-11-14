using MutableConfig.Serialization;
using System;
using System.IO;

namespace MutableConfig {
    public class ConfigContextOptions<T> where T : class, new() {
        private GenerationStrategy? _generationStrategy;

        public string FolderPath { get; private set; } = AppContext.BaseDirectory;
        public T ConfigObject { get; private set; } = new T();
        public IConfigSerializer Serializer { get; private set; } = new JsonConfigSerializer();

        public ConfigContextOptions<T> SetupDefaultConfigIfNotExists(
            T defaultConfig, string folderPath) {
            _generationStrategy = GenerationStrategy.CodeFirst;

            if (!Directory.Exists(folderPath))  
                Directory.CreateDirectory(folderPath);
            var configFilePath =    
                Path.Combine(folderPath, $"{typeof(T).Name}.{(Serializer is JsonConfigSerializer ? "json" : "xml")}");
            if (!File.Exists(configFilePath)) {
                var fileStream = new FileStream(configFilePath, FileMode.Create,
                    FileAccess.ReadWrite);
                fileStream.Close();
                File.WriteAllText(configFilePath,
                    Serializer.SerializeObject(defaultConfig));
                ConfigObject = defaultConfig;
            } else {
                ConfigObject =
                    Serializer.DeserializeObject<T>(
                        File.ReadAllText(configFilePath));
            }

            FolderPath = folderPath;
            return this;
        }

        public ConfigContextOptions<T> LoadConfigFromFile(string filePath) {
            _generationStrategy = GenerationStrategy.FileFirst;
            var isJsonFile = Path.GetExtension(filePath).ToLowerInvariant() == ".json";
            var text = File.ReadAllText(filePath);
            Serializer = isJsonFile ? (IConfigSerializer)new JsonConfigSerializer() : new XmlConfigSerializer();
            ConfigObject = Serializer.DeserializeObject<T>(text);
            FolderPath = filePath;
            return this;
        }

        public ConfigContextOptions<T> UseJson() {
            if (_generationStrategy != null)
                throw new Exception(
                    "Please adjust the call order of the UseJson() method to be earlier.");
            return UseSerializer(new JsonConfigSerializer());
        }

        public ConfigContextOptions<T> UseXml() {
            if (_generationStrategy != null)
                throw new Exception(
                    "Please adjust the call order of the UseXml() method to be earlier.");
            return UseSerializer(new XmlConfigSerializer());
        }

        private ConfigContextOptions<T> UseSerializer(IConfigSerializer serializer) {
            Serializer = serializer;
            return this;
        }
    }

    public enum GenerationStrategy {
        CodeFirst,
        FileFirst
    }
}
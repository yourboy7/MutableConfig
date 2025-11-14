using MutableConfig.Serialization;
using System;
using System.IO;

namespace MutableConfig {
    public class ConfigContext<T> where T : class, new() {
        private readonly string _configFilePath;
        private readonly object _locker = new object();
        private readonly IConfigSerializer _serializer;
        private T _value;

        public ConfigContext(ConfigContextOptions<T> contextOptions) {
            _serializer = contextOptions.Serializer;
            _value = contextOptions.DefaultConfig;

            var configDirectory = contextOptions.BasePath;
            if (!Directory.Exists(configDirectory))
                Directory.CreateDirectory(configDirectory);
            _configFilePath =
                Path.Combine(configDirectory, $"{typeof(T).Name}.{(_serializer is JsonConfigSerializer ? "json" : "xml")}");
            if (File.Exists(_configFilePath))
                return;

            var fileStream = new FileStream(_configFilePath, FileMode.Create,
                FileAccess.ReadWrite);
            fileStream.Close();

            SaveChanges();
        }

        public T Value {
            get {
                if (_value != null)
                    return _value;

                lock (_locker) {
                    if (_value == null) {
                        if (!File.Exists(_configFilePath))
                            throw new FileNotFoundException(_configFilePath);

                        var text = File.ReadAllText(_configFilePath);
                        _value = _serializer.DeserializeObject<T>(text);
                    }
                }

                return _value;
            }
        }

        public void SaveChanges() {
            lock (_locker) {
                if (_value == null)
                    throw new InvalidOperationException(
                        "Cannot save null configuration.");
                File.WriteAllText(_configFilePath,
                    _serializer.SerializeObject(_value));
            }
        }
    }
}
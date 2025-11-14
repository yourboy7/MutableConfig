using MutableConfig.Serialization;
using System;
using System.IO;

namespace MutableConfig {
    public class ConfigContext<T> where T : class, new() {
        private readonly string _configFilePath;
        private readonly IConfigSerializer _serializer;
        private readonly object _locker = new object();
        private T _value;

        public ConfigContext(ConfigContextOptions<T> contextOptions) {
            _configFilePath = Path.Combine(contextOptions.FolderPath, $"{typeof(T).Name}.{(contextOptions.Serializer is JsonConfigSerializer ? "json" : "xml")}");
            _serializer = contextOptions.Serializer;
            _value = contextOptions.ConfigObject;
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
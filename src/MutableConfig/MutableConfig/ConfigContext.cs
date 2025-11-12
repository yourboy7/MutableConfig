using System;
using System.IO;
using Newtonsoft.Json;

namespace MutableConfig {
    public class ConfigContext<T> where T : class, new() {
        private readonly string _configFilePath;
        private readonly object _locker = new object();
        private T _value;

        public ConfigContext(ConfigContextOptions<T> contextOptions) {
            var configDirectory = contextOptions.BasePath;
            if (!Directory.Exists(configDirectory))
                Directory.CreateDirectory(configDirectory);
            _configFilePath =
                Path.Combine(configDirectory, $"{typeof(T).Name}.json");
            if (File.Exists(_configFilePath))
                return;

            var fileStream = new FileStream(_configFilePath, FileMode.Create,
                FileAccess.ReadWrite);
            fileStream.Close();

            _value = contextOptions.DefaultConfig;
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

                        var json = File.ReadAllText(_configFilePath);
                        _value = JsonConvert.DeserializeObject<T>(json);
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
                    JsonConvert.SerializeObject(_value, Formatting.Indented));
            }
        }
    }
}
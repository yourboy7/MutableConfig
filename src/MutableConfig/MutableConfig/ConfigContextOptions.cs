using System;

namespace MutableConfig {
    public class ConfigContextOptions<T> where T : class, new() {
        public string BasePath { get; private set; } = AppContext.BaseDirectory;
        public T DefaultConfig { get; private set; } = new T();

        public ConfigContextOptions<T> SetBasePath(string path) {
            BasePath = path;
            return this;
        }

        public ConfigContextOptions<T> SetupDefaultConfigIfNotExists(
            T defaultConfig) {
            DefaultConfig = defaultConfig;
            return this;
        }
    }
}
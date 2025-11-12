using System;
using Microsoft.Extensions.DependencyInjection;

namespace MutableConfig {
    public static class ConfigContextExtensions {
        public static IServiceCollection AddConfigContext<TConfig>(
            this IServiceCollection services,
            Action<ConfigContextOptions<TConfig>> configure = null)
            where TConfig : class, new() {
            {
                var options = new ConfigContextOptions<TConfig>();

                if (configure != null)
                    configure(options);

                services.AddSingleton(options);
                services.AddSingleton<ConfigContext<TConfig>>();

                return services;
            }
        }
    }
}
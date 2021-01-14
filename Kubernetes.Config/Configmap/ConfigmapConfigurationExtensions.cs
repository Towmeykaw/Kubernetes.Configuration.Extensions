using System;
using Microsoft.Extensions.Configuration;

namespace Kubernetes.Configuration.Extensions.Configmap
{
    public static class ConfigmapConfigurationExtensions
    {   
        public static IConfigurationBuilder AddKubernetesConfigmap(this IConfigurationBuilder builder, string labelSelector, string namespaceSelector = "default", bool reloadOnChange = false, string separator = "__")
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(labelSelector))
            {
                throw new ArgumentException("Invalid label selector", nameof(labelSelector));
            }
            builder.Add(new ConfigmapConfigurationSource { Namespace = namespaceSelector, LabelSelector = labelSelector, ReloadOnChange = reloadOnChange});
            return builder;
        }

        /// <summary>
        /// Adds a JSON configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddKubernetesConfigmap(this IConfigurationBuilder builder, Action<ConfigmapConfigurationSource> configureSource)
            => builder.Add(configureSource);       
    }
}

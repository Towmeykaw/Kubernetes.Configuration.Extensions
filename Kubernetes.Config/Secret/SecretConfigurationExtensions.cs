using System;
using Microsoft.Extensions.Configuration;

namespace Kubernetes.Configuration.Extensions.Secret
{
    public static class SecretConfigurationExtensions
    {
        public static IConfigurationBuilder AddKubernetesSecret(this IConfigurationBuilder builder, string labelSelector, string namespaceSelector = "default", bool reloadOnChange = false)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(labelSelector))
            {
                throw new ArgumentException("Invalid label selector", nameof(labelSelector));
            }
            builder.Add(new SecretConfigurationSource { Namespace = namespaceSelector, LabelSelector = labelSelector, ReloadOnChange = reloadOnChange});
            return builder;
        }

        /// <summary>
        /// Adds a JSON configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddKubernetesSecret(this IConfigurationBuilder builder, Action<SecretConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}

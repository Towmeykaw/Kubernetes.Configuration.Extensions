using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Configuration;

namespace Kubernetes.Configuration.Extensions.Secret
{
    public class SecretConfigurationProvider : ConfigurationProvider
    {
        private readonly string _namespaceSelector;
        private readonly string _labelSelector;
        private readonly string _separator;
        private readonly k8s.Kubernetes _client;
        private readonly bool _decodeData;

        public SecretConfigurationProvider() : this(string.Empty, string.Empty, "__", false)
        { }
        public SecretConfigurationProvider(string? namespaceSelector, string? labelSelector, string? separator, bool reloadOnChange, bool decodeData = true)
        {
            _namespaceSelector = namespaceSelector ?? string.Empty;
            _labelSelector = labelSelector ?? string.Empty;
            _separator = separator ?? "__";
            _decodeData = decodeData;
            KubernetesClientConfiguration config;
            try
            {
                config = KubernetesClientConfiguration.InClusterConfig();
            }
            catch
            {
                config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            }
            _client = new k8s.Kubernetes(config);

            if (!reloadOnChange) return;
            
            var secretResponse = _client.ListNamespacedSecretWithHttpMessagesAsync(_namespaceSelector, labelSelector: _labelSelector, watch: true).Result;
            secretResponse.Watch<V1Secret, V1SecretList>((type, item) =>
            {
                if(type.Equals(WatchEventType.Modified))
                    Load(true);
            });
        }

        private void Load(bool reload)
        {
            if (reload)
            {
                Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
            try
            {
                var secrets = _client.ListNamespacedSecret(_namespaceSelector, labelSelector: _labelSelector);
                var dataList = secrets.Items.Where(w => w.Data != null).Select(s => s.Data);
                foreach (var dataItem in dataList)
                {
                    foreach (var (key, value) in dataItem)
                    {
                        Data.Add(key.Replace(_separator, ":"), _decodeData ? DecodeSecret(value) : Encoding.UTF8.GetString(value));
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
        public override void Load()
        {
            Load(false);
        }

        public static string DecodeSecret(byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentException("Invalid secret value null", nameof(value));
            }

            var base64String = Encoding.UTF8.GetString(value);
            try
            {
                var bytes = Convert.FromBase64String(base64String);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return base64String;
            }
        }
    }
}

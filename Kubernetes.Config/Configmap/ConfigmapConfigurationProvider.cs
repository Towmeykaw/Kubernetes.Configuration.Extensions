using System;
using System.Collections.Generic;
using System.Linq;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Configuration;

namespace Kubernetes.Configuration.Extensions.Configmap
{
    public class ConfigmapConfigurationProvider : ConfigurationProvider
    {
        private readonly string _namespaceSelector;
        private readonly string _labelSelector;
        private readonly k8s.Kubernetes _client;
        public ConfigmapConfigurationProvider() : this(string.Empty, string.Empty, false)
        { }
        public ConfigmapConfigurationProvider(string namespaceSelector, string labelSelector, bool reloadOnChange)
        {
            _namespaceSelector = namespaceSelector ?? string.Empty;
            _labelSelector = labelSelector ?? string.Empty;
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            _client = new k8s.Kubernetes(config);

            if (!reloadOnChange) return;
            var configMapsResponse = _client.ListNamespacedConfigMapWithHttpMessagesAsync(_namespaceSelector, labelSelector: _labelSelector, watch: true).Result;
            configMapsResponse?.Watch<V1ConfigMap>((type, item) =>
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
                var configMaps = _client.ListNamespacedConfigMap(_namespaceSelector, labelSelector: _labelSelector);
                var dataList = configMaps.Items.Where(w => w.Data != null).Select(s => s.Data);
                foreach (var dataItem in dataList)
                {
                    foreach (var item in dataItem)
                        Data.Add(item.Key, item.Value);
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
    }
}

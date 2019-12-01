using Microsoft.Extensions.Configuration;

namespace Kubernetes.Configuration.Extensions.Configmap
{
    public class ConfigmapConfigurationSource : IConfigurationSource
    {
        public string? Namespace { get; set; }
        public string? LabelSelector { get; set; }
        public bool ReloadOnChange { get; set; }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConfigmapConfigurationProvider(Namespace, LabelSelector, ReloadOnChange);
        }
    }
}
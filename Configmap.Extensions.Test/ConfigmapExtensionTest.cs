using System;
using Kubernetes.Configuration.Extensions.Configmap;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Kubernetes.Configuration.Extensions.Test
{
    public class ConfigmapExtensionTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddConfigmap_ThrowsIfLabelIsNullOrEmpty(string label)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var ex = Assert.Throws<ArgumentException>(() => configurationBuilder.AddKubernetesConfigmap(label));
            Assert.Equal("labelSelector", ex.ParamName);
            Assert.StartsWith("Invalid label selector", ex.Message);
        }
        [Fact]
        public void AddConfigmap_HasDefaultNamespace()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var source = configurationBuilder.AddKubernetesConfigmap("label").Sources[0] as ConfigmapConfigurationSource;
            Assert.Equal("default", source?.Namespace);
        }
    }
}
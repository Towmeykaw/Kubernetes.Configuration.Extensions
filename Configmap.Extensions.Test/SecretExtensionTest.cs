using System;
using System.Collections.Generic;
using System.Text;
using Kubernetes.Configuration.Extensions.Secret;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Kubernetes.Configuration.Extensions.Test
{
    public class SecretExtensionTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddSecret_ThrowsIfLabelIsNullOrEmpty(string label)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var ex = Assert.Throws<ArgumentException>(() => configurationBuilder.AddKubernetesSecret(label));
            Assert.Equal("labelSelector", ex.ParamName);
            Assert.StartsWith("Invalid label selector", ex.Message);
        }
        [Fact]
        public void AddSecret_HasDefaultNamespace()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var source = configurationBuilder.AddKubernetesSecret("label").Sources[0] as SecretConfigurationSource;
            Assert.Equal("default", source?.Namespace);
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void DecodeSecret_HandlesNormalCases(byte[] value)
        {
            var result = SecretConfigurationProvider.DecodeSecret(value);
            Assert.Equal(Encoding.UTF8.GetBytes("testdata"), Encoding.UTF8.GetBytes(result));
        }
        
        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { Encoding.UTF8.GetBytes("dGVzdGRhdGE=")};
        }
    }
}

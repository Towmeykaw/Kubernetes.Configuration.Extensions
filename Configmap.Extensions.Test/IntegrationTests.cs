using System.Linq;
using Kubernetes.Configuration.Extensions.Configmap;
using Kubernetes.Configuration.Extensions.Secret;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Kubernetes.Configuration.Extensions.Test
{
    public class IntegrationTests
    {
        [Trait("Type", "IntegrationTest")]
        [Fact]
        public void Test_Add_ConfigMap_And_Secret_From_Kubernetes()
        {
            var builder = new ConfigurationBuilder();
            builder.AddKubernetesConfigmap("app=testapp");
            builder.AddKubernetesSecret("app=testapp");

            var config = builder.Build();
            Assert.NotEmpty(config.Providers);
            Assert.Equal("testvalue", config.AsEnumerable().Single(x => x.Key == "Test:Config").Value);
            Assert.Equal("testsecretvalue", config.AsEnumerable().Single(x => x.Key == "Test:Secret").Value);
        }
    }
}

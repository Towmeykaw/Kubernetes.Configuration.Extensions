﻿using Microsoft.Extensions.Configuration;

namespace Kubernetes.Configuration.Extensions.Secret
{
    public class SecretConfigurationSource : IConfigurationSource
    {
        public string? Namespace { get; set; }
        public string? LabelSelector { get; set; }
        public string? Separator { get; set; }
        public bool ReloadOnChange { get; set; }
        public bool DecodeData { get; set; } = true;
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        { 
            return new SecretConfigurationProvider(Namespace, LabelSelector, Separator, ReloadOnChange, DecodeData);
        }
    }
}

![.NET Core](https://github.com/Towmeykaw/Kubernetes.Configuration.Extensions/workflows/.NET%20Core/badge.svg)

# Kubernetes.Configuration.Extensions

Adds extension methods for reading Kubernetes Configmaps and Secrets

### Installing

Available on nuget. Run command 
``` Install-Package Kubernetes.Configuration.Extensions ```

### Usage
To use with DI just add the following.
```
 .ConfigureAppConfiguration((hostingContext, config) =>
  {
      config.AddKubernetesConfigmap("app=testapp", reloadOnChange:true);
      config.AddKubernetesSecret("app=testapp", reloadOnChange:true);

  })
```
A label is required on the configmap since otherwise every configmap on the system will be fetched.

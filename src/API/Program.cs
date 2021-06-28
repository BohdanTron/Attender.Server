using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Attender.Server.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    var buildConfiguration = builder.Build();
                    var keyVaultEndpoint = buildConfiguration["KeyVault:Endpoint"];

                    var client = new SecretClient(new Uri(keyVaultEndpoint), new DefaultAzureCredential());
                    builder.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

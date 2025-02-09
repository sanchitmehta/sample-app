using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var builtConfig = config.Build();
                var credential = new DefaultAzureCredential();
                config.AddAzureAppConfiguration(options =>
                    options.Connect(builtConfig["AzureAppConfigurationConnection"])
                           .ConfigureKeyVault(kv => kv.SetCredential(credential)));
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
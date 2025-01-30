using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        // Example usage of DefaultAzureCredential for SQL Server authentication.
        var credential = new DefaultAzureCredential();

        // Placeholder for ensuring the connection logic can integrate as needed.
        Console.WriteLine("Azure Managed Identity is configured with DefaultAzureCredential.");

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

}
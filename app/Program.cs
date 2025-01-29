using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using System;
using System.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString = "Server=<SQL_SERVER_PLACEHOLDER>;Database=<DATABASE_PLACEHOLDER>;Authentication=Active Directory Default;TrustServerCertificate=True;";
        var credential = new DefaultAzureCredential();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.AccessToken = credential.GetToken(
                new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })
            ).Token;

            connection.Open();
            Console.WriteLine("Successfully connected to the database using Managed Identity.");
        }

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
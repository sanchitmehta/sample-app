using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using System.Data.SqlClient;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString = "Server=oa-demo-sql-stage.database.windows.net;Database=products;Authentication=Active Directory Default;TrustServerCertificate=True;";
        var credential = new DefaultAzureCredential();
        
        using (var connection = new SqlConnection(connectionString))
        {
            connection.AccessToken = credential.GetToken(
                new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/" })
            ).Token;

            try
            {
                connection.Open();
                Console.WriteLine("Connection to SQL Database successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
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
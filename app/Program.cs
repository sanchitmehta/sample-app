using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using System.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString = "Server=unspecified.sql.server;Database=unspecified_database;Authentication=Active Directory Default;TrustServerCertificate=True;";
        var credential = new DefaultAzureCredential();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.AccessToken = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/" })).Token;
            connection.Open();
            // Perform database operations here if required
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
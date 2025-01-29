using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using System.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString = "Server=unknown-sql-server;Database=unknown-database;Authentication=Active Directory Default;TrustServerCertificate=True;";
        var tokenCredential = new DefaultAzureCredential();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.AccessToken = tokenCredential.GetToken(
                new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })
            ).Token;

            connection.Open();
            // Perform database operations if needed
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
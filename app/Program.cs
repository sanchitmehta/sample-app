using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient;
using Azure.Identity;

public class Program
{
    public static void Main(string[] args)
    {
        string connectionString = "Server=example_sql_server;Database=example_database;Authentication=Active Directory Default;TrustServerCertificate=True;";

        var sqlConnection = new SqlConnection(connectionString)
        {
            AccessToken = new DefaultAzureCredential().GetToken(
                new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/" })).Token
        };

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

}
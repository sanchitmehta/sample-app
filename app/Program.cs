using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Microsoft.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    public static SqlConnection GetSqlConnection()
    {
        var connectionString = "Server=unknown;Database=unknown;Authentication=Active Directory Default;TrustServerCertificate=True;";
        var connection = new SqlConnection(connectionString);
        connection.AccessToken = new DefaultAzureCredential().GetToken(
            new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })
        ).Token;
        return connection;
    }
}
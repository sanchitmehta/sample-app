using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using System.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString = "Server=oa-demo-sql-stage;Database=products;Authentication=Active Directory Default;TrustServerCertificate=True;";
        var credential = new DefaultAzureCredential();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.AccessToken = new Azure.Identity.DefaultAzureCredential().GetToken(
                new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })
            ).Token;
            connection.Open();
            // Connection is ready to use
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
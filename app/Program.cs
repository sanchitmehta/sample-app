using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Microsoft.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString = "Server=SpecifyServerHere;Database=SpecifyDatabaseHere;Authentication=Active Directory Default;TrustServerCertificate=True;";
        var sqlConnection = new SqlConnection(connectionString)
        {
            AccessToken = new DefaultAzureCredential().GetToken(
                new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })
            ).Token
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
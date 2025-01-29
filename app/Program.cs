using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient;
using Azure.Identity;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString = new SqlConnectionStringBuilder
        {
            DataSource = "oa-demo-sql-mnilsen.database.windows.net",
            InitialCatalog = "products",
            Authentication = SqlAuthenticationMethod.ActiveDirectoryDefault,
            TrustServerCertificate = true
        };

        var credential = new DefaultAzureCredential();
        using var connection = new SqlConnection(connectionString.ConnectionString)
        {
            AccessToken = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })).Token
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
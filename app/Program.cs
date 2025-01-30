using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Microsoft.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString = new SqlConnectionStringBuilder
        {
            DataSource = "unknown-sql-server",
            InitialCatalog = "unknown-database",
            Authentication = SqlAuthenticationMethod.ActiveDirectoryDefault,
            TrustServerCertificate = true
        }.ConnectionString;

        var credential = new DefaultAzureCredential();
        using (var sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.AccessToken = credential.GetToken(
                new Azure.Core.TokenRequestContext(
                    new[] { "https://database.windows.net//.default" }
                )
            ).Token;
            
            // Use sqlConnection as needed
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
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
            DataSource = "oa-demo-sql-mnilsen.database.windows.net",
            InitialCatalog = "products",
            Authentication = SqlAuthenticationMethod.ActiveDirectoryDefault,
            TrustServerCertificate = true
        }.ConnectionString;

        var credential = new DefaultAzureCredential();
        using var connection = new SqlConnection(connectionString, credential);

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

}
using System;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = hostContext.Configuration;

                string connectionString = "Server=Not provided;Database=Not provided;Authentication=Active Directory Default;TrustServerCertificate=True;";
                var options = new SqlConnectionStringBuilder(connectionString);

                var tokenProvider = new DefaultAzureCredential();
                services.AddSingleton<IDbConnection>(sp =>
                {
                    var connection = new SqlConnection(options.ConnectionString);
                    connection.AccessToken = tokenProvider.GetToken(
                        new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })
                    ).Token;
                    return connection;
                });
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
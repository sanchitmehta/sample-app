using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using System.Data.SqlClient;
using System.Data;

public class Program
{
    public static void Main(string[] args)
    {
        using (var connection = new SqlConnection("Server=oa-demo-sql-sanchit.database.windows.net;Database=products;Authentication=Active Directory Default;TrustServerCertificate=True;"))
        {
            var credential = new DefaultAzureCredential();
            connection.AccessToken = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/" })).Token;
            connection.Open();

            // Application logic here if needed
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
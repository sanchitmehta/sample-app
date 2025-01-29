using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using System.Data.SqlClient;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        var connectionString = "Server=unknown-sql-server;Database=unknown-database;Authentication=Active Directory Default;TrustServerCertificate=True;";
        var sqlConnection = new SqlConnection(connectionString)
        {
            AccessToken = new DefaultAzureCredential().GetToken(
                new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })).Token
        };

        services.AddSingleton(sqlConnection);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
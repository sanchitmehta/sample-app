using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using System.Data.SqlClient;
using System;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        string connectionString = "Server=oa-demo-sql-mnilsen.database.windows.net;Database=products;Authentication=Active Directory Default;TrustServerCertificate=True;";
        var credentials = new DefaultAzureCredential();

        services.AddScoped(_ =>
        {
            var connection = new SqlConnection(connectionString);
            connection.AccessToken = credentials.GetToken(
                new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/" })
            ).Token;
            return connection;
        });
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
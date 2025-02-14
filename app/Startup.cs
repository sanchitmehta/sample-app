using System;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        
        string sqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
        if (!string.IsNullOrEmpty(sqlConnectionString))
        {
            var credential = new DefaultAzureCredential();
            var connection = new SqlConnection(sqlConnectionString);
            connection.AccessToken = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })).Token;
            services.AddSingleton<SqlConnection>(connection);
        }
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
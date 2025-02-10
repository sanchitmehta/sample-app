using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        var sqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
        if (!string.IsNullOrEmpty(sqlConnectionString))
        {
            services.AddScoped<SqlConnection>(sp =>
            {
                var connection = new SqlConnection(sqlConnectionString);
                var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
                connection.AccessToken = new DefaultAzureCredential().GetToken(tokenRequestContext).Token;
                return connection;
            });
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
using System;
using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient;
using Azure.Core;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        string sqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
        if (!string.IsNullOrEmpty(sqlConnectionString))
        {
            services.AddTransient<SqlConnection>(sp =>
            {
                var credential = new DefaultAzureCredential();
                var token = credential.GetToken(new TokenRequestContext(new[] { "https://database.windows.net/.default" })).Token;
                var connection = new SqlConnection(sqlConnectionString);
                connection.AccessToken = token;
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
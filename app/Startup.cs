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
        
        // Setup SQL connection using Managed Identity if environment variable is present
        string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddSingleton<SqlConnection>(provider =>
            {
                var credential = new DefaultAzureCredential();
                var tokenRequestContext = new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" });
                var token = credential.GetToken(tokenRequestContext);
                var connection = new SqlConnection(connectionString)
                {
                    AccessToken = token.Token
                };
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
using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        string sqlConnectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
        if (!string.IsNullOrEmpty(sqlConnectionString))
        {
            // Using Managed Identity for SQL Server authentication
            var defaultAzureCredential = new DefaultAzureCredential();
            var token = defaultAzureCredential.GetToken(new TokenRequestContext(new[] { "https://database.windows.net/.default" }));
            // The token can be used when establishing SQL connections. 
            // For example, when constructing a SqlConnection:
            // var connection = new SqlConnection(sqlConnectionString);
            // connection.AccessToken = token.Token;
            services.AddSingleton(defaultAzureCredential);
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
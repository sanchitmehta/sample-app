using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Azure.Core;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Retrieve SQL connection string from environment variable
        var sqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

        // Use Azure Managed Identity for SQL Server Authentication
        var credential = new DefaultAzureCredential();
        var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
        var token = credential.GetToken(tokenRequestContext);
        
        // sqlConnectionString is used as before; token.Token can be applied when establishing a SQL connection

        services.AddControllersWithViews();
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
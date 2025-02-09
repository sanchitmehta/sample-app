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

        var connectionString = "Server=oa-demo-sql-stage.database.windows.net;Database=products;Authentication=Active Directory Default;TrustServerCertificate=True;";

        var credential = new DefaultAzureCredential();
        using var connection = new SqlConnection(connectionString);
        connection.AccessToken = credential.GetToken(
            new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })).Token;

        // Configure additional services that depend on the SQL connection if needed.
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
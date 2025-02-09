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

        // Configure SQL Server with Azure Managed Identity
        var connectionString = "Server=oa-demo-sql-stage;Database=products;Authentication=Active Directory Default;TrustServerCertificate=True;";
        services.AddTransient(provider =>
        {
            var sqlConnection = new SqlConnection(connectionString)
            {
                AccessToken = new DefaultAzureCredential().GetToken(
                    new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })
                ).Token
            };
            return sqlConnection;
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
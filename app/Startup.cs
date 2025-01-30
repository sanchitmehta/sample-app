using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient;
using Azure.Identity;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddSingleton(provider =>
        {
            var connectionString = "Server=unknown;Database=unknown;Authentication=Active Directory Default;TrustServerCertificate=True;";
            var sqlConnection = new SqlConnection(connectionString)
            {
                AccessToken = new DefaultAzureCredential().GetToken(
                    new Azure.Core.TokenRequestContext(
                        new[] { "https://database.windows.net/.default" }
                    )).Token
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
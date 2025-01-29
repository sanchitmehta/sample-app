using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Microsoft.Data.SqlClient;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        var connectionString = "Server=oa-demo-sql-mnilsen.database.windows.net;Database=products;Authentication=Active Directory Default;TrustServerCertificate=True;";
        var azureCredential = new DefaultAzureCredential();
        var builder = new SqlConnectionStringBuilder(connectionString);
        builder.AccessToken = azureCredential.GetToken(
            new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net//.default" })
        ).Token;

        services.AddSingleton(new SqlConnection(builder.ConnectionString));
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
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Azure.Core;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        var sqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
        services.AddSingleton(new SqlConnectionFactory(sqlConnectionString));
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

public class SqlConnectionFactory
{
    private readonly string _connectionString;
    private readonly DefaultAzureCredential _credential;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
        _credential = new DefaultAzureCredential();
    }

    public SqlConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
        var token = _credential.GetToken(tokenRequestContext);
        connection.AccessToken = token.Token;
        return connection;
    }
}
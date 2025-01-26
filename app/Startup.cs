```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

public class Startup : IDisposable
{
    private bool disposed = false;

    public void ConfigureServices(IServiceCollection services)
    {
        // Utilize Microsoft.Extensions.DependencyInjection's dependency injection for scoped disposables
        services.AddControllersWithViews();

        // Add IHttpClientFactory for better management of HTTP clients
        services.AddHttpClient();

        // Register logging services to ensure proper scope management
        services.AddLogging();
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

        // Ensure proper disposal of resources during requests such as buffers, streams, etc.
        app.Use(async (context, next) =>
        {
            try
            {
                await next(); // Proceed to the next middleware
            }
            finally
            {
                if (context.Response.Body != null)
                {
                    await context.Response.Body.DisposeAsync();
                }
            }
        });
    }

    // Dispose pattern to clean up unmanaged resources and implement proper cleanup of services
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Dispose of managed resources here if any
            }

            // Clean up event handlers or static field references
            ClearStaticResources();

            disposed = true;
        }
    }

    private void ClearStaticResources()
    {
        // Example: Clear static handlers or collections to prevent memory leaks
        // Example: StaticEvent -= EventHandler;
    }

    ~Startup()
    {
        Dispose(false);
    }
}
```
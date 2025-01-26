```csharp
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

public class Program : IDisposable
{
    private static IHost _host;

    public static void Main(string[] args)
    {
        _host = CreateHostBuilder(args).Build();

        // Gracefully handle application exit and resource disposal
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

        try
        {
            _host.Run();
        }
        finally
        {
            DisposeResources();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    private static void OnProcessExit(object sender, EventArgs e)
    {
        DisposeResources();
    }

    private static void DisposeResources()
    {
        if (_host != null)
        {
            _host.Dispose();
            _host = null;

            // Clear event handlers
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
        }
    }

    public void Dispose()
    {
        DisposeResources();
        GC.SuppressFinalize(this);
    }

    ~Program()
    {
        DisposeResources();
    }
}
```
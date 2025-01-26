```csharp
using System;
using System.Collections.Generic;

namespace SampleApp.Models
{
    public class HomeModel : IDisposable
    {
        private bool _disposed;

        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Features { get; set; }
        public List<Product> Products { get; set; }

        public HomeModel()
        {
            Features = new List<string>();
            Products = new List<Product>();
        }

        // Implement IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Dispose managed resources
                if (Features != null)
                {
                    Features.Clear();
                    Features = null;
                }

                if (Products != null)
                {
                    Products.Clear();
                    Products = null;
                }
            }

            // Clean up unmanaged resources here if any

            _disposed = true;
        }

        ~HomeModel()
        {
            Dispose(false);
        }
    }

    public class Product : IDisposable
    {
        private bool _disposed;

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Implement IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Dispose managed resources here if any
                Name = null;
            }

            // Clean up unmanaged resources here if any

            _disposed = true;
        }

        ~Product()
        {
            Dispose(false);
        }
    }
}
```
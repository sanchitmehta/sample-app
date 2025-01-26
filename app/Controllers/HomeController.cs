```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SampleApp.Models;
using System;
using System.Collections.Generic;

namespace SampleApp.Controllers
{
    public class HomeController : Controller, IDisposable
    {
        private readonly string connectionString;
        private bool disposed = false;

        public HomeController()
        {
            var envConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
            if (string.IsNullOrEmpty(envConnectionString))
            {
                throw new InvalidOperationException("Environment variable SQL_CONNECTION_STRING is not set.");
            }
            connectionString = envConnectionString;
        }

        public IActionResult Index()
        {
            using (var model = new HomeModel())
            {
                model.Products = GetProducts();
                return View(model);
            }
        }

        private List<Product> GetProducts()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT TOP 10 ProductId, Name, ListPrice FROM [SalesLT].[Product]", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDecimal(2)
                            });
                        }
                    }
                }
            }

            return products;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                }

                // Free any unmanaged resources here, if any.
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~HomeController()
        {
            Dispose(false);
        }
    }
}
```
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SampleApp.Models;
using System;
using System.Collections.Generic;
using Azure.Identity;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString;

        public HomeController()
        {
            // eg: Server=tcp:<server>,1433;Initial Catalog=<database-name>;Persist Security Info=False;Authentication=Active Directory Default;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
            var credential = new DefaultAzureCredential(); // using Managed Identity via DefaultAzureCredential
            var envConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
            if (string.IsNullOrEmpty(envConnectionString))
            {
                throw new InvalidOperationException("Environment variable SQL_CONNECTION_STRING is not set.");
            }
            connectionString = envConnectionString;
        }

        public IActionResult Index()
        {
            var model = new HomeModel
            {
                Products = GetProducts()
            };
            return View(model);
        }

        private List<Product> GetProducts()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT TOP 10 ProductId, Name, ListPrice FROM [SalesLT].[Product]", connection);
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

            return products;
        }
    }
}
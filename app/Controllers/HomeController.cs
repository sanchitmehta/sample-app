using Microsoft.AspNetCore.Mvc;
using Azure.Identity;
using Azure.Core;
using Microsoft.Data.SqlClient;
using SampleApp.Models;
using System;
using System.Collections.Generic;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString;

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
                DefaultAzureCredential credential = new DefaultAzureCredential();
                var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/" });
                connection.AccessToken = credential.GetToken(tokenRequestContext).Token;
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
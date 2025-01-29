using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Azure.Identity;
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
            connectionString = "Server=oa-demo-sql-mnilsen.database.windows.net;Database=products;Authentication=Active Directory Default;TrustServerCertificate=True;";
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
            var credential = new DefaultAzureCredential();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.AccessToken = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/" })).Token;
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
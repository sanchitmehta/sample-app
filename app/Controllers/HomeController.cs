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
        private readonly DefaultAzureCredential credential;

        public HomeController()
        {
            var envConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
            if (string.IsNullOrEmpty(envConnectionString))
            {
                throw new InvalidOperationException("Environment variable SQL_CONNECTION_STRING is not set.");
            }
            connectionString = envConnectionString;
            credential = new DefaultAzureCredential();
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
            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                Authentication = SqlAuthenticationMethod.ActiveDirectoryDefault
            };

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.AccessToken = credential.GetToken(
                    new Azure.Core.TokenRequestContext(
                        new[] { "https://database.windows.net/.default" }
                    )
                ).Token;

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
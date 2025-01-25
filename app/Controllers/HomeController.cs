using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SampleApp.Models;
using System.Collections.Generic;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = "YourConnectionStringHere";

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
                var command = new SqlCommand("SELECT Id, Name, Price FROM Products", connection);
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
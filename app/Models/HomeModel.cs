using System.Collections.Generic;

namespace SampleApp.Models
{
    public class HomeModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Features { get; set; }
        public List<Product> Products { get; set; }

        public HomeModel()
        {
            Features = new List<string>();
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
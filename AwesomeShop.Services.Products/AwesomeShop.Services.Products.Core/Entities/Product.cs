using System;
using AwesomeShop.Services.Products.Core.Events;
using AwesomeShop.Services.Products.Core.ValueObjects;

namespace AwesomeShop.Services.Products.Core.Entities
{
    public class Product : AggregateRoot
    {
        public Product(string title, string description, decimal price, int quantity, Category category)
        {
            Title = title;
            Description = description;
            Price = price;
            Quantity = quantity;
            Category = category;

            CreatedAt = DateTime.Now;
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int Quantity { get; private set; }
        public Category Category { get; private set; }

        public void Update(string description, decimal price, Category category) {
            if (category != null) {
                Category = category;
            }

            Description = description;
            Price = price;

            AddEvent(new ProductUpdated(Id));
        }

        public static Product Create(string title, string description, decimal price, int quantity, Category category) {
            var product = new Product(title, description, price, quantity, category);

            return product;
        }
    }
}
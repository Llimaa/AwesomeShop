using System;
using AwesomeShop.Services.Products.Core.Entities;
using MediatR;
using Domain = AwesomeShop.Services.Products.Core.ValueObjects;

namespace AwesomeShop.Services.Products.Application.Commands
{
    public class AddProduct : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public CategoryDto Category { get; set; }

        public Product ToEntity() {
            return new Product(Title, Description, Price, Quantity, new Domain.Category(Category.Description, Category.Subcategory));
        }
    }

    public class CategoryDto {
        public string Description { get; set; }
        public string Subcategory { get; set; }

        public Domain.Category ToValueObject() {
            return new Domain.Category(Description, Subcategory);
        }
    }
}
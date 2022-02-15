using System;

namespace AwesomeShop.Services.Products.Application.Dtos.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
    }
}
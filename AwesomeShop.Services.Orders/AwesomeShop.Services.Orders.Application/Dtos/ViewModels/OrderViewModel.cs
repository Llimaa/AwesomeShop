using System;
using AwesomeShop.Services.Orders.Core.Entities;

namespace AwesomeShop.Services.Orders.Application.Dtos.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel(Guid id, decimal totalPrice, DateTime createdAt, string status)
        {
            Id = id;
            TotalPrice = totalPrice;
            CreatedAt = createdAt;
            this.status = status;
        }

        public Guid Id { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string status { get; private set; }

        public static OrderViewModel FromEntity(Order order) {
            return new OrderViewModel(order.Id, order.TotalPrice, order.CreatedAt, order.Status.ToString());
        }
    }
}
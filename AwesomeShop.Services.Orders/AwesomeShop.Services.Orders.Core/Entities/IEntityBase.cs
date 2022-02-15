using System;

namespace AwesomeShop.Services.Orders.Core.Entities
{
    public interface IEntityBase
    {
        Guid Id { get; }
    }
}
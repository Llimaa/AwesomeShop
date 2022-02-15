using System;
using AwesomeShop.Services.Customers.Core.ValueObjects;
using MediatR;

namespace AwesomeShop.Services.Customers.Application.Commands
{
    public class UpdateCustomer : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDto Address { get; set; }
    }

    public class AddressDto {
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public Address ToEntity() 
            => new Address(Street, Number, City, State, ZipCode);

        public static AddressDto ToDto(Address address)
            => new AddressDto {
                Street = address?.Street,
                Number = address?.Number,
                City = address?.City,
                State = address?.State,
                ZipCode = address?.ZipCode
            };

    }
}
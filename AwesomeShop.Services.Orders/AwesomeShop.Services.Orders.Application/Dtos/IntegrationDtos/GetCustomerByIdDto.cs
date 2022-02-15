using System;

namespace AwesomeShop.Services.Orders.Application.Dtos.IntegrationDtos
{
    public class GetCustomerByIdDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public AddressDto Address { get; set; }

    }

    public class AddressDto {
         public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
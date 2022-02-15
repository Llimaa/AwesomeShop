using System.Collections.Generic;

namespace AwesomeShop.Services.Orders.Application.Dtos
{
    public class GenericHandlerResult
    {
        public GenericHandlerResult(string message, object data, bool success, List<ValidationObject> validations)
        {
            Message = message;
            Data = data;
            Success = success;
            Validations = validations;
        }

        public string Message { get; private set; }
        public object Data { get; private set; }
        public bool Success { get; private set; }
        public List<ValidationObject> Validations { get; private set; }
    }

    public class ValidationObject {
        public ValidationObject(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public string Property { get; private set; }
        public string Message { get; private set; }
    }
}
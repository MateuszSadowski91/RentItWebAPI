using System;

namespace RentItAPI.Exceptions
{
    public class ExternalServerError : Exception
    {
        public ExternalServerError(string message) : base(message)
        {
        }
    }
}
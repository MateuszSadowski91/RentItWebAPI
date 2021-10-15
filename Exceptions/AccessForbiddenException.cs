using System;

namespace RentItAPI.Exceptions
{
    public class AccessForbiddenException : Exception
    {
        public AccessForbiddenException(string message) : base(message)
        {
        }
    }
}
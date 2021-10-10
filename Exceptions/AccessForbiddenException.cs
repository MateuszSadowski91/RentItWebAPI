using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Exceptions
{
    public class AccessForbiddenException : Exception
    {
        public AccessForbiddenException(string message) : base(message)
        {

        }
    }
}

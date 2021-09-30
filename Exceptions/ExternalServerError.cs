using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Exceptions
{
    public class ExternalServerError : Exception
    {
        public ExternalServerError(string message) : base(message)
        {

        }
    }
}

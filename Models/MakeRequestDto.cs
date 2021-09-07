using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Models
{
    public class MakeRequestDto
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string? Message { get; set; }
    }
}

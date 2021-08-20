using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Models
{
    public class MakeReservationDto
    {
        
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Info { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Models
{
    public class CreateBusinessDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string TaxNumber { get; set; }
    }
}

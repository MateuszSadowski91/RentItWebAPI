using System.ComponentModel.DataAnnotations;

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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Models
{
    public class ModifyItemDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string ImageThumbnail { get; set; }
        [Required]
        public bool IsActive { get; set; } = true; //False indicates that this item is not for a public display anymore.
        [Required]
        public bool RequestRequired { get; set; }
    }
}

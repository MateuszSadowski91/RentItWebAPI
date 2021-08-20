using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Entities
{
    public class Business
    {
        public int Id { get; set; }
        public int? CreatedById { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string TaxNumber { get; set; }
     
        public virtual List<Item> Items { get; set; }
        
    }
}

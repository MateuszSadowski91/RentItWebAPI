using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Models
{
    public class CreateInvoiceDto
    {
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Vendee { get; set; }
        public string TaxRate { get; set; }
        public string Currency { get; set; }
        public string Quantity { get; set; }
        public IEnumerable<string> Items {get; set;}
    }
}

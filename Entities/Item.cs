using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string ImageThumbnail { get; set; }

        public virtual List <Reservation> Reservations { get; set; }
        public virtual Business Business { get; set; }
        public bool IsActive { get; set; } = true; //False indicates that this item is not for a public display anymore.
        public bool RequestRequired { get; set; }
      
        /// <summary>
        ///If true, then customer must first send a request that requires an explicit approval from business manager in order to make a valid reservation.
        ///Useful for businesses strongly dependent on seasonal rentals, providing an owner with more freedom of choice regarding their target customer.
    }  /// </summary>



}

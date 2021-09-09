using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int? CreatedById { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Status ReservationStatus { get; set; } = Status.Accepted;
        public virtual Business Business { get; set; }
        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}

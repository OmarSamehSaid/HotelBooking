using System;
using System.Collections.Generic;

#nullable disable

namespace HotelBooking.Models
{
    public partial class Room
    {
        public Room()
        {
            Photos = new HashSet<Photo>();
            Reservations = new HashSet<Reservation>();
        }

        public int Roomid { get; set; }
        public int Hotelid { get; set; }
        public bool? Isreserved { get; set; }
        public int? Cost { get; set; }
        public string? Time { get; set; }
        public string? Type { get; set; }
        public bool? Isdeleted { get; set; }

        public virtual User Hotel { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}

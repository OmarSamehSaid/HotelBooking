using System;
using System.Collections.Generic;

#nullable disable

namespace HotelBooking.Models
{
    public partial class User
    {
        public User()
        {
            Reservations = new HashSet<Reservation>();
            ReviewClients = new HashSet<Review>();
            ReviewHotels = new HashSet<Review>();
            Rooms = new HashSet<Room>();
        }


        public int Userid { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Isdeleted { get; set; }
        public string Photo { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double? Lng { get; set; }
        public double? Lat { get; set; }


        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Review> ReviewClients { get; set; }
        public virtual ICollection<Review> ReviewHotels { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace HotelBooking.Models
{
    public partial class Reservation
    {
        public int Reserveid { get; set; }
        public int Clientid { get; set; }
        public int Roomid { get; set; }
        public bool? Isdeleted { get; set; }

        public string? Startdate { get; set; }
        public string? Enddate { get; set; }

        public double? Totalcost { get; set; }
        public int? Number { get; set; }

        public virtual User Client { get; set; }
        public virtual Room Room { get; set; }
    }
}

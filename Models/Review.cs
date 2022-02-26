using System;
using System.Collections.Generic;

#nullable disable

namespace HotelBooking.Models
{
    public partial class Review
    {
        public int Reviewid { get; set; }
        public int Hotelid { get; set; }
        public int Clientid { get; set; }
        public string Review1 { get; set; }
        public string Ishappy { get; set; }
        public bool? Isdeleted { get; set; }

        public virtual User Client { get; set; }
        public virtual User Hotel { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace HotelBooking.Models
{
    public partial class Photo
    {
        public int Photoid { get; set; }
        public int Roomid { get; set; }
        public string Photo1 { get; set; }

        public virtual Room Room { get; set; }
    }
}

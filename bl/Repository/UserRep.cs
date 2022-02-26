using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Models;

namespace HotelBooking.bl.Repository
{
    public class UserRep
    {
        private readonly hotelsprojectContext DB;
        public UserRep(hotelsprojectContext DB)
        {
            this.DB = DB;
        }

        public IEnumerable<User> Get()
        {
            var data = DB.Users.Select(a => a);
            return data;
        }

    }
}

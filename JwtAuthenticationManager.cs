using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly hotelsprojectContext _context;
        public string Authenticate(string email, string password)
        {
            
        }
    }
}

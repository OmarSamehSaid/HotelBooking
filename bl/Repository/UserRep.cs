using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;

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
            var data = DB.Users.Where(a => a.Isdeleted != false).Select(a => a);
            return data;
        }

        public void Add(User user)
        {
            DB.Users.Add(user);
            DB.SaveChanges();
        }

        private User GetUserById(int Id)
        {
            return DB.Users.Where(a => a.Userid == Id).FirstOrDefault();
        }

        public User GetById(int Id)
        {
            User data = GetUserById(Id);
            return data;
        }

        public void Update(User user)
        {
            DB.Entry(user).State = EntityState.Modified;
            DB.SaveChanges();
        }

    }
}

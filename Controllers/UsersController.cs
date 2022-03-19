using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Models;

namespace HotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly hotelsprojectContext _context;
        public UsersController(hotelsprojectContext context)
        {
            _context = context;

        }


        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.Where(a => a.Isdeleted != true).Select(a => a).ToListAsync();
        }

        [HttpGet("~/api/Users/Client")]
        public async Task<ActionResult<IEnumerable<User>>> GetClients()
        {
            return await _context.Users.Where(a => a.Isdeleted != true && a.Role=="client").Select(a => a).ToListAsync();
        }

        [HttpGet("~/api/Users/Login")]
        public async Task<ActionResult<IEnumerable<User>>> Login(string email, string password)
        {
            return await _context.Users.Where(a => a.Isdeleted != true && a.Email == email && a.Password == password).Select(a => a).ToListAsync();

        }


        [HttpGet("~/api/Users/Hotel")]
        public async Task<ActionResult<IEnumerable<User>>> GetHotels()
        {
            return await _context.Users.Where(a => a.Isdeleted != true && a.Role == "hotel").Select(a => a).ToListAsync();
        }


        //[HttpGet("~/api/Users/Login")]
        //public IActionResult Login(string email,string password)
        //{
        //    var user = _context.Users.Where(a => a.Isdeleted != true && a.Email == email && a.Password == password).Select(a => a).ToListAsync();
        //    return Ok(user);

        //}
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Userid)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        [HttpPut("~/api/Users/delete")]
        public async Task<IActionResult> DelUser(int id)
        {
            var user = _context.Users.Where(a => a.Userid == id).FirstOrDefault();

            user.Isdeleted = true;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Userid }, user);
        //}

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            var data = _context.Users.Any(e => e.Email == user.Email);
            if (!data)
            {
                user.Isdeleted = false;
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok(user);
            }
            else
            {
                return Ok("Email already exist");
            }

        }

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpGet("~/api/Hotel/Name")]
        public async Task<ActionResult<IEnumerable<User>>> GetHotelByName(string name)
        {
            return await _context.Users.Where(a => a.Role == "hotel" && a.Isdeleted != true && EF.Functions.Like(a.Name, $"%{name}%")).Select(a => a).ToListAsync();

        }

        [HttpGet("~/api/Hotel/Address")]
        public async Task<ActionResult<IEnumerable<User>>> GetHotelByAddress(string address)
        {
            return await _context.Users.Where(a => a.Role == "hotel" && a.Isdeleted != true && EF.Functions.Like(a.Address, $"%{address}%")).Select(a => a).ToListAsync();

        }
   

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Userid == id);
        }

    }
}

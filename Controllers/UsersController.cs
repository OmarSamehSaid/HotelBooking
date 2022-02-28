﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Models;
using HotelBooking.bl.Repository;

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

        [HttpPut("~/api/Users/delete/{id}")]
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
            user.Isdeleted = false;
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);

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

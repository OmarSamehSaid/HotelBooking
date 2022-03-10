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
    public class RoomsController : ControllerBase
    {
        private readonly hotelsprojectContext _context;

        public RoomsController(hotelsprojectContext context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _context.Rooms.Where(a => a.Isdeleted != true).Select(a => a).ToListAsync();

        } 
        [HttpGet("~/api/Rooms/Hotel/{id}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomsbyHotel(int id)
        {
            return await _context.Rooms.Where(a => a.Isdeleted != true && a.Hotelid == id && a.Isreserved == false).Select(a => a).ToListAsync();

        }
        [HttpGet("~/api/Rooms/Hotel/reserved/{Hotelid}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetReservedRoomsbyHotel(int id)
        {
            return await _context.Rooms.Where(a => a.Isdeleted != true && a.Hotelid == id && a.Isreserved == true).Select(a => a).ToListAsync();

        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        // PUT: api/Rooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.Roomid)
            {
                return BadRequest();
            }

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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
        [HttpPut("~/api/Rooms/delete/{id}")]
        public async Task<IActionResult> DelRoom(int id)
        {
            var room = _context.Rooms.Where(a => a.Roomid == id).FirstOrDefault();

            room.Isdeleted = true;

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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

        // POST: api/Rooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //public async Task<ActionResult<Room>> PostRoom(Room room)
        //{
        //    _context.Rooms.Add(room);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetRoom", new { id = room.Roomid }, room);
        //}
        public IActionResult CreateRoom(Room room)
        {
            room.Isdeleted = false;
            room.Isreserved = false;
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return Ok(room);
        }

        // DELETE: api/Rooms/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRoom(int id)
        //{
        //    var room = await _context.Rooms.FindAsync(id);
        //    if (room == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Rooms.Remove(room);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Roomid == id);
        }
    }
}

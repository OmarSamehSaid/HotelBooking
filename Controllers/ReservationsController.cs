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
    public class ReservationsController : ControllerBase
    {
        private readonly hotelsprojectContext _context;

        public ReservationsController(hotelsprojectContext context)
        {
            _context = context;
        }
        // GET: api/Rooms
        [HttpGet("~/api/Reservations/client/{clientId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationsByClientId(int clientId)
        {

            return await _context.Reservations.Where(a => a.Isdeleted != true && a.Clientid == clientId).Select(a => a).ToListAsync();

        }

        [HttpGet("~/api/Reservations/hotel/{hotelId}")]
        public async Task<ActionResult<IEnumerable<MyClass>>> GetReservationsByHotelId(int hotelId)
        {
            return await _context.Reservations.Join(_context.Rooms, a => a.Roomid, b => b.Roomid, (a, b) => new
            {
                reserveid = a.Reserveid,
                clientid = a.Clientid,
                roomid = a.Roomid,
                isdeleted = a.Isdeleted,
                startdate = a.Startdate,
                enddate = a.Enddate,
                totalcost = a.Totalcost,
                number = a.Number,
                hotelid = b.Hotelid

            }).Where(x => x.isdeleted != true && x.hotelid == hotelId).Select(x => new MyClass()
            {
                Reserveid = x.reserveid,
                Clientid = x.clientid,
                Roomid = x.roomid,
                Isdeleted = x.isdeleted,
                Startdate = x.startdate,
                Enddate = x.enddate,
                Totalcost = x.totalcost,
                Number = x.number,
                Hotelid = x.hotelid
            }).ToListAsync();

        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _context.Reservations.Where(a => a.Isdeleted != true).Select(a => a).ToListAsync();

        }

        // GET: api/Reservations/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Reservation>> GetReservation(int id)
        //{
        //    var reservation = await _context.Reservations.FindAsync(id);

        //    if (reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    return reservation;
        //}

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        //{
        //    if (id != reservation.Reserveid)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(reservation).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ReservationExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult Createreservation(Reservation reservation)
        {
            reservation.Isdeleted = false;
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return Ok(reservation);
        }
        //public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        //{
        //    _context.Reservations.Add(reservation);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetReservation", new { id = reservation.Reserveid }, reservation);
        //}

        // DELETE: api/Reservations/5
        [HttpPut("~/api/Reservations/delete/{id}")]
        public async Task<IActionResult> DelReservation(int id)
        {
            var reserve = _context.Reservations.Where(a => a.Reserveid == id).FirstOrDefault();

            reserve.Isdeleted = true;

            _context.Entry(reserve).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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


        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Reserveid == id);
        }

        public class MyClass
        {
            public int Reserveid { get; set; }
            public int Clientid { get; set; }
            public int Roomid { get; set; }
            public bool? Isdeleted { get; set; }

            public string? Startdate { get; set; }
            public string? Enddate { get; set; }

            public double? Totalcost { get; set; }
            public int? Number { get; set; }
            public int Hotelid { get; set; }
        }
    }
}

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
            var reservation = _context.Reservations.Where(a => a.Reserveid == id).FirstOrDefault();

            reservation.Isdeleted = true;

            _context.Entry(reservation).State = EntityState.Modified;

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
    }
}

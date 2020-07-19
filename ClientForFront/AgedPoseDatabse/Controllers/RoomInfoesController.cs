using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgedPoseDatabse.Models;

namespace AgedPoseDatabse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomInfoesController : ControllerBase
    {
        private readonly AiForAgedDbContext _context;

        public RoomInfoesController(AiForAgedDbContext context)
        {
            _context = context;
        }

        // GET: api/RoomInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomInfo>>> GetRoomInfos()
        {
            return await _context.RoomInfos.ToListAsync();
        }

        // GET: api/RoomInfoes/id
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomInfo>> GetRoomInfo(long id)
        {
            var roomInfo = await _context.RoomInfos.FindAsync(id);

            if (roomInfo == null)
            {
                return NotFound();
            }

            return roomInfo;
        }

        // PUT: api/RoomInfoes/id
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomInfo(long id, RoomInfo roomInfo)
        {
            if (id != roomInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(roomInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomInfoExists(id))
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

        // POST: api/RoomInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RoomInfo>> PostRoomInfo(RoomInfo roomInfo)
        {
            _context.RoomInfos.Add(roomInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoomInfo", new { id = roomInfo.Id }, roomInfo);
        }

        // DELETE: api/RoomInfoes/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomInfo>> DeleteRoomInfo(long id)
        {
            var roomInfo = await _context.RoomInfos.FindAsync(id);
            if (roomInfo == null)
            {
                return NotFound();
            }

            _context.RoomInfos.Remove(roomInfo);
            await _context.SaveChangesAsync();

            return roomInfo;
        }

        private bool RoomInfoExists(long id)
        {
            return _context.RoomInfos.Any(e => e.Id == id);
        }
    }
}

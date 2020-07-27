using AgedPoseDatabse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgedPoseDatabse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgesInfoesController : ControllerBase
    {
        private readonly AiForAgedDbContext _context;

        public AgesInfoesController(AiForAgedDbContext context)
        {
            _context = context;
        }

        // GET: api/AgesInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgesInfo>>> GetAgesInfos()
        {
            return await _context.AgesInfos.ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<AgesInfo>>> GetAgesInfos(long roominfoId)
        {
            var agesInfo = await _context.AgesInfos.Where(x => x.RoomInfoId == roominfoId).ToListAsync();
            if (agesInfo.Count == 0)
            {
                return NotFound();
            }
            return agesInfo;
        }

        // GET: api/AgesInfoes/id
        [HttpGet("{id}")]
        public async Task<ActionResult<AgesInfo>> GetAgesInfo(long id)
        {
            var agesInfo = await _context.AgesInfos.FindAsync(id);

            if (agesInfo == null)
            {
                return NotFound();
            }

            return agesInfo;
        }

        // PUT: api/AgesInfoes/id
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgesInfo(long id, AgesInfo agesInfo)
        {
            if (id != agesInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(agesInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgesInfoExists(id))
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

        // POST: api/AgesInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AgesInfo>> PostAgesInfo(AgesInfo agesInfo)
        {
            _context.AgesInfos.Add(agesInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgesInfo", new { id = agesInfo.Id }, agesInfo);
        }

        // DELETE: api/AgesInfoes/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<AgesInfo>> DeleteAgesInfo(long id)
        {
            var agesInfo = await _context.AgesInfos.FindAsync(id);
            if (agesInfo == null)
            {
                return NotFound();
            }

            _context.AgesInfos.Remove(agesInfo);
            await _context.SaveChangesAsync();

            return agesInfo;
        }

        private bool AgesInfoExists(long id)
        {
            return _context.AgesInfos.Any(e => e.Id == id);
        }
    }
}

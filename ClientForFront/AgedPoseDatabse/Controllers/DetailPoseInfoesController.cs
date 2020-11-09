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
    public class DetailPoseInfoesController : ControllerBase
    {
        private readonly AiForAgedDbContext _context;

        public DetailPoseInfoesController(AiForAgedDbContext context)
        {
            _context = context;
        }

        // GET: api/DetailPoseInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetailPoseInfo>>> GetDeatilPoseInfos()
        {
            return await _context.DeatilPoseInfos.ToListAsync();
        }

        // GET: api/DetailPoseInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailPoseInfo>> GetDetailPoseInfo(long id)
        {
            var detailPoseInfo = await _context.DeatilPoseInfos.FindAsync(id);

            if (detailPoseInfo == null)
            {
                return NotFound();
            }

            return detailPoseInfo;
        }

        // PUT: api/DetailPoseInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetailPoseInfo(long id, DetailPoseInfo detailPoseInfo)
        {
            if (id != detailPoseInfo.AgesInfoId)
            {
                return BadRequest();
            }

            _context.Entry(detailPoseInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailPoseInfoExists(id))
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

        // POST: api/DetailPoseInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DetailPoseInfo>> PostDetailPoseInfo(DetailPoseInfo detailPoseInfo)
        {
            _context.DeatilPoseInfos.Add(detailPoseInfo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DetailPoseInfoExists(detailPoseInfo.AgesInfoId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDetailPoseInfo", new { id = detailPoseInfo.AgesInfoId }, detailPoseInfo);
        }

        // DELETE: api/DetailPoseInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DetailPoseInfo>> DeleteDetailPoseInfo(long id)
        {
            var detailPoseInfo = await _context.DeatilPoseInfos.FindAsync(id);
            if (detailPoseInfo == null)
            {
                return NotFound();
            }

            _context.DeatilPoseInfos.Remove(detailPoseInfo);
            await _context.SaveChangesAsync();

            return detailPoseInfo;
        }

        private bool DetailPoseInfoExists(long id)
        {
            return _context.DeatilPoseInfos.Any(e => e.AgesInfoId == id);
        }
    }
}

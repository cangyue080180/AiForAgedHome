using AgedPoseDatabse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgedPoseDatabse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoseInfoesController : ControllerBase
    {
        private readonly AiForAgedDbContext _context;

        public PoseInfoesController(AiForAgedDbContext context)
        {
            _context = context;
        }

        // GET: api/PoseInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PoseInfo>>> GetPoseInfos()
        {
            return await _context.PoseInfos.ToListAsync();
        }

        // GET: api/PoseInfoes/?id=xx&minDate=xx&maxDate=xx
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PoseInfo>>> GetPoseInfo(long id,DateTime minDate,DateTime maxDate)
        {
            var poseInfo= await _context.PoseInfos.Where(x => x.AgesInfoId == id && x.Date>=minDate && x.Date<maxDate).ToListAsync<PoseInfo>();

            //var poseInfo = await _context.PoseInfos.FindAsync(id);

            if (poseInfo == null)
            {
                return NotFound();
            }

            return poseInfo;
        }

        // PUT: api/PoseInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoseInfo(long id, PoseInfo poseInfo)
        {
            if (id != poseInfo.AgesInfoId)
            {
                return BadRequest();
            }

            _context.Entry(poseInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoseInfoExists(id))
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

        // POST: api/PoseInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PoseInfo>> PostPoseInfo(PoseInfo poseInfo)
        {
            _context.PoseInfos.Add(poseInfo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PoseInfoExists(poseInfo.AgesInfoId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPoseInfo", new { id = poseInfo.AgesInfoId }, poseInfo);
        }

        // DELETE: api/PoseInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PoseInfo>> DeletePoseInfo(long id)
        {
            var poseInfo = await _context.PoseInfos.FindAsync(id);
            if (poseInfo == null)
            {
                return NotFound();
            }

            _context.PoseInfos.Remove(poseInfo);
            await _context.SaveChangesAsync();

            return poseInfo;
        }

        private bool PoseInfoExists(long id)
        {
            return _context.PoseInfos.Any(e => e.AgesInfoId == id);
        }
    }
}

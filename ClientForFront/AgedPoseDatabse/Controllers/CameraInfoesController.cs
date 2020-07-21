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
    public class CameraInfoesController : ControllerBase
    {
        private readonly AiForAgedDbContext _context;

        public CameraInfoesController(AiForAgedDbContext context)
        {
            _context = context;
        }

        // GET: api/CameraInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CameraInfo>>> GetCameraInfos()
        {
            return await _context.CameraInfos.ToListAsync();
        }

        // GET: api/CameraInfoes/id
        [HttpGet("{id}")]
        public async Task<ActionResult<CameraInfo>> GetCameraInfo(long id)
        {
            var cameraInfo = await _context.CameraInfos.FindAsync(id);

            if (cameraInfo == null)
            {
                return NotFound();
            }

            return cameraInfo;
        }

        // PUT: api/CameraInfoes/id
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCameraInfo(long id, CameraInfo cameraInfo)
        {
            if (id != cameraInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(cameraInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CameraInfoExists(id))
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

        // POST: api/CameraInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CameraInfo>> PostCameraInfo(CameraInfo cameraInfo)
        {
            _context.CameraInfos.Add(cameraInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCameraInfo", new { id = cameraInfo.Id }, cameraInfo);
        }

        // DELETE: api/CameraInfoes/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<CameraInfo>> DeleteCameraInfo(long id)
        {
            var cameraInfo = await _context.CameraInfos.FindAsync(id);
            if (cameraInfo == null)
            {
                return NotFound();
            }

            _context.CameraInfos.Remove(cameraInfo);
            await _context.SaveChangesAsync();

            return cameraInfo;
        }

        private bool CameraInfoExists(long id)
        {
            return _context.CameraInfos.Any(e => e.Id == id);
        }
    }
}

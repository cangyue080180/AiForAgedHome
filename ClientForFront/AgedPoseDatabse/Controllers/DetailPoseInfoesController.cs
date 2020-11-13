using AgedPoseDatabse.Helper;
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
    public class DetailPoseInfoesController : ControllerBase
    {
        private readonly AiForAgedDbContext _context;

        public DetailPoseInfoesController(AiForAgedDbContext context)
        {
            _context = context;
        }

        // GET: api/DetailPoseInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetailPoseInfo>>> GetDetailPoseInfos()
        {
            return await _context.DeatilPoseInfos.Take(10).ToListAsync();//为了防止数据过大，限制只返回10条数据
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<DetailPoseInfo>> GetDetailPoseInfo(long id, DateTime datetime)
        {
            var deatilPoseInfo = await _context.DeatilPoseInfos.FirstOrDefaultAsync(x => x.AgesInfoId == id && x.DateTime == datetime);

            if (deatilPoseInfo == null)
            {
                return NotFound();
            }

            return deatilPoseInfo;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedList<DetailPoseInfo>>> GetDetailPoseInfosByDay(long id , string date, int pageIndex, int pageSize)
        {
            DateTime startTime = DateTime.Parse(date);
            var detailPoseInfoes = _context.DeatilPoseInfos.Where(x => x.AgesInfoId == id && x.DateTime >= startTime && x.DateTime <= startTime.AddHours(24));

            var queryDetailPoseInfoes =await PaginatedList<DetailPoseInfo>.CreateAsync(detailPoseInfoes,pageIndex,pageSize);

            return queryDetailPoseInfoes;
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
                if (DetailPoseInfoExists(detailPoseInfo.AgesInfoId, detailPoseInfo.DateTime))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDetailPoseInfo", new { id = detailPoseInfo.AgesInfoId, datetime=detailPoseInfo.DateTime }, detailPoseInfo);
        }


        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteDetailPoseInfoByDay(long id, string date)
        {
            DateTime startTime = DateTime.Parse(date);
            var detailPoseInfoes = await _context.DeatilPoseInfos.Where(x=>x.AgesInfoId == id && x.DateTime >= startTime && x.DateTime <= startTime.AddHours(24)).ToListAsync();
            if (detailPoseInfoes.Count == 0)
            {
                return NotFound();
            }

            _context.DeatilPoseInfos.RemoveRange(detailPoseInfoes);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool DetailPoseInfoExists(long id, DateTime date)
        {
            return _context.DeatilPoseInfos.Any(e => e.AgesInfoId == id && e.DateTime == date);
        }
    }
}

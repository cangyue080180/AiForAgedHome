using AgedPoseDatabse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AgedPoseDatabse.Controllers
{
    [Route("age/[controller]")]
    [ApiController]
    public class RecEventsController : ControllerBase
    {
        private readonly AiForAgedDbContext _context;

        public RecEventsController(AiForAgedDbContext context)
        {
            _context = context;
        }

        // GET: api/<RecEventController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecEvent>>> GetRecEvents()
        {
            return await _context.RecEvents.Take(10).Select(x =>new RecEvent {Id=x.Id,Name=x.Name,UpdateTime=x.UpdateTime }).ToListAsync();
        }

        // GET api/<RecEventController>/id
        [HttpGet("{id}")]
        public async Task<ActionResult<RecEvent>> Get(long id)
        {
            var recEventItem = await _context.RecEvents.FindAsync(id);
            if(recEventItem==null)
            {
                return NotFound();
            }

            return recEventItem;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<string>> GetImg(long id)
        {
            var recEventItem = await _context.RecEvents.FindAsync(id);
            if (recEventItem == null)
            {
                return NotFound();
            }
            return recEventItem.Img;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<RecEvent>> GetWithoutImg(long id)
        {
            var recEventItem = await _context.RecEvents.FindAsync(id);
            if (recEventItem == null)
            {
                return NotFound();
            }
            return new RecEvent() { Id=recEventItem.Id,Name=recEventItem.Name,UpdateTime=recEventItem.UpdateTime,Img=null};
        }

        // POST api/<RecEventController> 添加新项
        [HttpPost]
        public async Task<ActionResult<RecEvent>> PostRecEvent(RecEvent recEvent)
        {
            _context.RecEvents.Add(recEvent);
            await _context.SaveChangesAsync();

            recEvent.Img = null;
            return recEvent;
            //return CreatedAtAction("GetWithoutImg", new { id = recEvent.Id }, recEvent);
        }

        // DELETE: api/AgesInfoes/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<RecEvent>> DeleteRecEvent(long id)
        {
            var tempRecEvent = await _context.RecEvents.FindAsync(id);
            if (tempRecEvent == null)
            {
                return NotFound();
            }

            _context.RecEvents.Remove(tempRecEvent);
            await _context.SaveChangesAsync();

            return new RecEvent() { Id = tempRecEvent.Id, Name = tempRecEvent.Name, UpdateTime = tempRecEvent.UpdateTime };
        }
    }
}

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
    public class ServerInfoesController : ControllerBase
    {
        private readonly AiForAgedDbContext _context;

        public ServerInfoesController(AiForAgedDbContext context)
        {
            _context = context;
        }

        // GET: api/ServerInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerInfo>>> GetServerInfos()
        {
            return await _context.ServerInfos.ToListAsync();
        }

        // GET: api/ServerInfoes/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ServerInfo>> GetServerInfo(long id)
        {
            var serverInfo = await _context.ServerInfos.FindAsync(id);

            if (serverInfo == null)
            {
                return NotFound();
            }

            return serverInfo;
        }

        //Get:api/ServerInfoes/ip
        [HttpGet]
        public async Task<ActionResult<ServerInfo>> GetServerInfo(string ip)
        {
            var serverInfo = await _context.ServerInfos.FirstAsync(x => x.Ip == ip);

            if (serverInfo == null)
            {
                return NotFound();
            }
            return serverInfo;
        }

        // PUT: api/ServerInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServerInfo(long id, ServerInfo serverInfo)
        {
            if (id != serverInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(serverInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServerInfoExists(id))
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

        // POST: api/ServerInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ServerInfo>> PostServerInfo(ServerInfo serverInfo)
        {
            _context.ServerInfos.Add(serverInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServerInfo", new { id = serverInfo.Id }, serverInfo);
        }

        // DELETE: api/ServerInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServerInfo>> DeleteServerInfo(long id)
        {
            var serverInfo = await _context.ServerInfos.FindAsync(id);
            if (serverInfo == null)
            {
                return NotFound();
            }

            _context.ServerInfos.Remove(serverInfo);
            await _context.SaveChangesAsync();

            return serverInfo;
        }

        private bool ServerInfoExists(long id)
        {
            return _context.ServerInfos.Any(e => e.Id == id);
        }
    }
}

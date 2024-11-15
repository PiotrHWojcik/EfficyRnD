using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using EfficyRnD.Data;
using EfficyRnD.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EfficyRnD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpPost]
        [SwaggerOperation(Summary = "User story 1: Create new counter", OperationId =("US1CreateNewCounter"))]
        public async Task<ActionResult<Counter>> US1CreateNewCounter(Counter counter)
        {
            _context.Counters?.Add(counter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCounter), new { id = counter.Id }, counter);
        }

        [HttpPost("{id}/increment")]
        [SwaggerOperation(Summary = "User story 2: Increment one counter", OperationId = ("US2IncrementCounter"))]
        public async Task<IActionResult> US2IncrementCounter(int id)
        {
            if (_context.Counters == null)
                BadRequest("There is no counters in database!");
            else
            {

                var counter = await _context.Counters.FindAsync(id);

                if (counter == null)
                {
                    return NotFound();
                }

                await _context.IncrementCounterAsync(id); return NoContent();
            }

            return BadRequest();
        }

        [HttpGet("team/{teamId}/sum")]
        [SwaggerOperation(Summary = "User story 3: Get counters sum for given team", OperationId = ("US3GetCountersSumForTeam"))]
        public async Task<ActionResult<(int, Int128)>> US3GetCountersSumForTeam(int teamId)
        {
            if (_context.Counters == null)
                return NotFound("There is no Counters");

            var sum = await _context.Counters
                .Where(c => c.TeamId == teamId)
                .SumAsync(c => c.Value);

            return Ok(new { TeamId = teamId, Sum = sum });
        }

        [HttpGet("team/sums")]
        [SwaggerOperation(Summary = "User story 4: score board", OperationId = ("US4GetAllSums"))]
        public async Task<ActionResult<List<Models.TeamCounterSums>>> US4GetAllSums()
        {
            if (_context.Counters == null)
                return NotFound("There is no Counters");

            if (_context.Teams == null)
                return NotFound("There is no Teams");

            var counters = await _context.Counters.ToListAsync();   
            var teams = await _context.Teams.ToListAsync();

            var sums = teams  
                .GroupJoin(counters, 
                    team => team.Id, 
                    counter => counter.TeamId,
                    (team, counterGroup) => new Models.TeamCounterSums
                    {
                        TeamId = team.Id,
                        TeamName = team.Name ?? string.Empty,
                        Sum = counterGroup.Sum(c => c.Value)
                    })
                .GroupBy(c => c.TeamId)
                .ToList();

                return new JsonResult(sums);
        }

        [HttpGet("team/{teamId}/counters")]
        [SwaggerOperation(Summary = "User story 5: Get counters for given team", OperationId = ("US5GetCountersForTeam"))]
        public async Task<ActionResult<(int, Int128)>> US5GetCountersForTeam(int teamId)
        {
            if (_context.Counters == null)
                return NotFound("There is no Counters");

            var couters = await _context.Counters
                .Where(c => c.TeamId == teamId)
                .ToListAsync();

            return Ok(couters);
        }

        [HttpPost("addteam")]
        [SwaggerOperation(Summary = "User story 6a: Add team", OperationId = ("US6aAddTeam"))]
        public async Task<ActionResult<Team>> US6aAddTeam(Team team)
        {
            _context.Teams?.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, team);
        }

        [HttpDelete("deleteteam/{id}")]
        [SwaggerOperation(Summary = "User story 6b: Delete team", OperationId = ("US6bAddTeam"))]
        public async Task<IActionResult> US6bAddTeam(int id)
        {
            if (_context.Teams == null)
                return NotFound("Not found team id: " + id);

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team); await _context.SaveChangesAsync(); return NoContent();
        }

        [HttpDelete("deletecounter{id}")]
        [SwaggerOperation(Summary = "User story 7: Delete counter", OperationId = ("US7DeleteCounter"))]
        public async Task<IActionResult> US7DeleteCounter(int id)
        {
            if (_context.Counters == null)
                return NotFound("Not found counter id: " + id);

            var counter = await _context.Counters.FindAsync(id);

            if (counter == null)
            {
                return NotFound();
            }

            _context.Counters.Remove(counter);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("counter/{id}")]
        public async Task<ActionResult<Counter>> GetCounter(int id)
        {
            if (_context.Counters == null)
                return NotFound();

            var counter = await _context.Counters.FindAsync(id);
            if (counter == null)
            {
                return NotFound();
            }
            return counter;
        }

        [HttpGet("team/{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            if (_context.Teams == null)
                return NotFound();

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return team;
        }
    }
}
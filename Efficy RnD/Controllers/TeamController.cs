using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EfficyRnD.Data;
using EfficyRnD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EfficyRnD.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TeamsController(AppDbContext context) : ControllerBase
	{
		private readonly AppDbContext _context = context;

        [HttpGet]
		public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
		{
			if (_context.Teams == null)
				return NotFound("There is no Teams");

			return await _context.Teams.ToListAsync();
		}

		[HttpGet("{id}")]
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

		[HttpPost]
		public async Task<ActionResult<Team>> PostTeam(Team team)
		{
			_context.Teams?.Add(team);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, team);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutTeam(int id, Team team)
		{
			if (id != team.Id)
			{
				return BadRequest();
			}
			_context.Entry(team).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TeamExists(id)) { return NotFound(); } else { throw; }
			}
			return NoContent();
		}

		[HttpDelete("{id}")] 
		public async Task<IActionResult> DeleteTeam(int id) 
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

		private bool TeamExists(int id) 
		{
			if (_context.Teams == null)
				return false;

			return _context.Teams.Any(e => e.Id == id); 
		}
	}
}
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
	public class CountersController(AppDbContext context) : ControllerBase 
	{ 
		private readonly AppDbContext _context = context;

        [HttpGet] 
		public async Task<ActionResult<IEnumerable<Counter>>> GetCounters() 
		{
			if (_context.Counters == null)
				return NotFound("There is no Counters");

			return await _context.Counters.ToListAsync(); 
		} 
		
		[HttpGet("{id}")] 
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
		
		[HttpPost]
		public async Task<ActionResult<Counter>> PostCounter(Counter counter) 
		{ 
			_context.Counters?.Add(counter); 
			await _context.SaveChangesAsync(); 
			
			return CreatedAtAction(nameof(GetCounter), new { id = counter.Id }, counter); 
		} 
		
		[HttpPut("{id}")] 
		public async Task<IActionResult> PutCounter(int id, Counter counter) 
		{ 
			if (id != counter.Id) 
			{ 
				return BadRequest(); 
			} 

			_context.Entry(counter).State = EntityState.Modified; 

			try 
			{ 
				await _context.SaveChangesAsync(); 
			} 
			catch (DbUpdateConcurrencyException) 
			{ 
				if (!CounterExists(id)) 
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
		
		[HttpDelete("{id}")] 
		public async Task<IActionResult> DeleteCounter(int id) 
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
		
		private bool CounterExists(int id) 
		{
			if (_context.Counters == null)
				return false;

			return _context.Counters.Any(e => e.Id == id); 
		} 
	}
}
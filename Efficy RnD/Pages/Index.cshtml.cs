using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EfficyRnD.Data;
using EfficyRnD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EfficyRnD.Pages
{ 
	public class IndexModel(AppDbContext context) : PageModel 
	{ 
		private readonly AppDbContext _context = context;

        public IList<Team> Teams { get; set; } = [];
		public IList<Counter> Counters { get; set; } = [];
		public async Task OnGetAsync() 
		{
			if (_context.Teams == null)
				Teams = [];
			else
				Teams = await _context.Teams.ToListAsync();


			if (_context.Counters == null)
				Counters = [];
			else
				Counters = await _context.Counters.ToListAsync(); 
		} 
	}
}

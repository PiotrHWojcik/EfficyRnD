using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using EfficyRnD.Models;

namespace EfficyRnD.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
        public DbSet<Team>? Teams { get; set; }
		public DbSet<Counter>? Counters { get; set; }

        public async Task IncrementCounterAsync(int counterId) 
        { 
            await Database.ExecuteSqlRawAsync("EXEC IncrementCounter @CounterId = {0}", counterId); 
        }
    }
}
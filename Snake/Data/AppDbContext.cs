using Microsoft.EntityFrameworkCore;

namespace Snake.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<PlayerRecord> Players { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=records.db");
        }
        public AppDbContext()
        {
            Database.EnsureCreated();
        }
    }
}

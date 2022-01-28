using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Void.DAL.Entities;

namespace Void.DAL
{
    public class VoidContext : DbContext
    {
        public VoidContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Coin> Coins { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Ticker> Tickers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

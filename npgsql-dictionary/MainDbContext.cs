using Microsoft.EntityFrameworkCore;
using npgsql_dictionary.Models;

namespace npgsql_dictionary
{
    public class MainDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Mikrocop.ManagingUsers.Models
{
    public class AdministrationContext : DbContext
    {
        public AdministrationContext(DbContextOptions<AdministrationContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

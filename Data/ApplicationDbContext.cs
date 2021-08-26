using AuthorizationExample.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationExample.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        
        public DbSet<User> Users { get; set; }
        
        public DbSet<Role> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(user => user.Role)
                .WithMany()
                .HasForeignKey(user => user.RoleId);
        }
    }
}
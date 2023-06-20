using IdentityApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityApi.Context
{
    public class AppDbContext : DbContext 
    {
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=identity_db;Port=5432;Database=identity_db;User Id=postgres;Password=postgres;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(p=>p.Id);

            modelBuilder.Entity<User>().Property(p=>p.UserName)
                .IsRequired()
                    .IsUnicode(true)
                        .HasMaxLength(20);

            modelBuilder.Entity<User>().Property(p=>p.Password)
                .HasMaxLength(8)
                    .IsRequired()
                        .IsFixedLength(true);
        }
    }
}

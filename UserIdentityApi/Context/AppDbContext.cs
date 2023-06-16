using ChatLibrary.Entities;
using IdentityApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityApi.Context
{
    public class AppDbContext : DbContext 
    {
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres;");
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
                .IsRequired().IsFixedLength(true);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using OrganizationApi.Entities;

namespace OrganizationApi.Context;

public class AppDbContext : DbContext 
{
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Room> Rooms => Set<Room>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=organization_db;Port=5432;Database=mehmon_organ_db;User Id=postgres;Password=postgres;");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}

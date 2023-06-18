using ChatApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Context;

public class AppDbContext : DbContext
{
    public DbSet<ChatGroup> ChatGroups { get; set; }
    public DbSet<Message> Messages { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=chat_db;Port=5432;Database=contact_api;User Id=postgres;Password=postgres;");
    }
}

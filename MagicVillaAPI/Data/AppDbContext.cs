using MagicVillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
    
    public DbSet<Villa> Villas { get; set; }
}
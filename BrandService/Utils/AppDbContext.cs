using BrandService.Models;
using Microsoft.EntityFrameworkCore;

namespace BrandService.Utils;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<Brand> Brands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>()
            .HasOne(b => b.Parent)
            .WithMany(b => b.Children)
            .HasForeignKey(b => b.ParentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
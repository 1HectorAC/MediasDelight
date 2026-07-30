
using MediasDelight.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediasDelight.Web.Data;

public class AppDbContext: IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MediaType>().HasData(
            new MediaType {Id= 1, Name="Movie"},
            new MediaType {Id= 2, Name="Show"},
            new MediaType {Id= 3, Name="Video Game"}
        );
    }

    public DbSet<MediaItem> MediaItems {get; set;}
    public DbSet<MediaType> MediaTypes {get; set;}
}
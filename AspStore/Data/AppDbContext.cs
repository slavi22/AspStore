using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspStore.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);  
        var adminRole = new IdentityRole("Admin");
        adminRole.NormalizedName = adminRole.Name.ToUpper();
        var userRole = new IdentityRole("User");
        userRole.NormalizedName = userRole.Name.ToUpper();
        builder.Entity<IdentityRole>().HasData(adminRole, userRole);
    }
}
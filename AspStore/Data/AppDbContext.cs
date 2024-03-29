using AspStore.Models.Account;
using AspStore.Models.Cart;
using AspStore.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspStore.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ProductModel> Products { get; set; }
    public DbSet<ProductCategoryModel> ProductsCategory { get; set; }
    public DbSet<ProductImageModel> ProductsImages { get; set; }
    public DbSet<AddressModel> UserAddress { get; set; }
    public DbSet<OrderModel> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedAspNetRolesTable(builder);
        SeedProductCategoryTable(builder);
    }

    private void SeedAspNetRolesTable(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(new IdentityRole("Admin") { NormalizedName = "ADMIN" },
            new IdentityRole("User") { NormalizedName = "USER" });
    }

    private void SeedProductCategoryTable(ModelBuilder builder)
    {
        builder.Entity<ProductCategoryModel>().HasData(new ProductCategoryModel("CPU") { Id = 1 },
            new ProductCategoryModel("GPU") { Id = 2 },
            new ProductCategoryModel("RAM") { Id = 3 },
            new ProductCategoryModel("Motherboard") { Id = 4 });
    }
}
using AspStore.Data;
using AspStore.Policies.Handlers;
using AspStore.Policies.Requirements;
using AspStore.Services;
using AspStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AspStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("AspStore"));
                
            });
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAuthorizationHandler, FirstTimeSetupHandler>();
            builder.Services.AddScoped<IUserPageService, UserPageService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("FirstTimeSetupComplete",
                    policy => policy.Requirements.Add(new FirstTimeSetupRequirement(1)));
            });
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.MaxAge = TimeSpan.FromDays(14);
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "SessionData";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "account",
                pattern: "{controller=Account}/{action}"
            );

            app.MapControllerRoute(
                name: "product",
                pattern: "{controller=Product}/{action}"
            );

            app.Run();
        }
    }
}
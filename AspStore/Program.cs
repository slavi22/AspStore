using AspStore.Data;
using AspStore.Policies.Handlers;
using AspStore.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
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
            string loginPath = String.Empty;
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

            builder.Services.AddScoped<IAuthorizationHandler, FirstTimeSetupHandler>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("FirstTimeSetupComplete",
                    policy => policy.Requirements.Add(new FirstTimeSetupRequirement(1)));
            });/*.ConfigureApplicationCookie(options =>
            {
                //access db context here
                /*var db = builder.Services.BuildServiceProvider().GetService<AppDbContext>();
                if (db.Users.Count() == 0)
                {
                    options.LoginPath = "/Account/Register";
                }
                else
                {
                    options.LoginPath = "/Account/Login";
                }#1#
                //options.LoginPath = loginPath;
            });*/

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "account",
                pattern: "{controller=Account}/{action}"
            );

            app.Run();
        }
    }
}
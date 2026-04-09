using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HDKTech.Areas.Identity.Data;
using HDKTech.Models;
using HDKTech.Data;
using HDKTech.Repositories;
using HDKTech.Repositories.Interfaces;
using HDKTech.Services;

namespace HDKTech
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("HDKTechContextConnection") ?? throw new InvalidOperationException("Connection string 'HDKTechContextConnection' not found.");

            builder.Services.AddDbContext<HDKTechContext>(options => options.UseSqlServer(connectionString));

            builder.Services
                 .AddIdentity<NguoiDung, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true) 
                 .AddEntityFrameworkStores<HDKTechContext>()
                 .AddDefaultUI()
                 .AddDefaultTokenProviders();

            // Đăng ký Repository Pattern
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ProductRepository>();
            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            // Đăng ký Cart Service (Session) - 7 ngày để giỏ hàng không bị mất
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(7); // 7 ngày
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Lax; // Security
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICartService, SessionCartService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<HDKTechContext>();
                // Ensure database schema is up-to-date before running seed data
                await context.Database.MigrateAsync();

                await DataSeed.KhoiTaoDuLieuMacDinh(scope.ServiceProvider);
                await DataSeedProductsOptimized.SeedProducts(scope.ServiceProvider);
            }

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
            app.UseSession();  // Thêm dòng này

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.Run();
            
        }
    }
}
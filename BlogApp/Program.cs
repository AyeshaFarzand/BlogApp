using BlogApp.Data;
using BlogApp.Repositories;
using Microsoft.EntityFrameworkCore;



namespace BlogApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options=>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AppDb"))
            );

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // ✅ Add distributed memory cache (required for session)
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddScoped<IPostRepository, PostRepository>();

            var app = builder.Build();
            
            app.UseSession();
            

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // ✅ Shows detailed errors in development mode
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Custom error page for production
                app.UseHsts();
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
            builder.Services.AddSession();
            builder.Services.AddDistributedMemoryCache();
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
using BlogApp.Data;
using BlogApp.Repositories;
using BlogApp.Repository;
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
            builder.Services.AddScoped<ILikesRepository, LikesRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();

            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            /* builder.Services.AddSession(options =>
             {
                 options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                 options.Cookie.HttpOnly = true;
                 options.Cookie.IsEssential = true;
             });*/


            builder.Services.AddAuthentication("MyCookieAuth")
            .AddCookie("MyCookieAuth", options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

            builder.Services.AddAuthorization();
            // ✅ Add distributed memory cache (required for session)
            // builder.Services.AddDistributedMemoryCache();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddSession();
            builder.Services.AddDistributedMemoryCache();
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
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
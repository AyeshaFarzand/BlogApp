using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using BlogApp.Data;

namespace BlogApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly Data.AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 Show Register Form
        public IActionResult Register()
        {
            return View();
        }

        // 🔹 Handle Register Logic
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "User already exists!");
                return View(model);
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                RoleId = model.RoleId,
                Password = model.Password
            };

            //user.SetPassword(model.Password); // Hash password

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // 🔹 Show Login Form
        public IActionResult Login()
        {
            return View();
        }

        // 🔹 Handle Login Logic
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !user.VerifyPassword(model.Password))
            {
                ModelState.AddModelError("", "Invalid email or password!");
                return View(model);
            }

            // Store user data in session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home"); // Redirect to Home page
        }

        // 🔹 Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session
            return RedirectToAction("Login");
        }
    }
}

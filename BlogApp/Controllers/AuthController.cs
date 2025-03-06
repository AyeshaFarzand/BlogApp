using BlogApp.Models;
using BlogApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // ✅ Show Registration Form
        public IActionResult Register()
        {
            return View();
        }

        // ✅ Handle Registration
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "User already exists!");
                return View(model);
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password, // This will be hashed inside the repository
                RoleId = model.RoleId
            };

            await _userRepository.AddUserAsync(user);
            return RedirectToAction("Login");
        }

        // ✅ Show Login Form
        public IActionResult Login()
        {
            return View();
        }

        // ✅ Handle Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isValidUser = await _userRepository.VerifyUserCredentialsAsync(model.Email, model.Password);
            if (!isValidUser)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            // Store session (optional)
            HttpContext.Session.SetString("UserEmail", model.Email);

            return RedirectToAction("Index", "Home");
        }

        // ✅ Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using BlogApp.Repositories;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        // ✅ Show Login Page
        public IActionResult Login()
        {
            return View();
        }

        // ✅ Handle Login with Lockout Mechanism
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // 🔐 Check if user is locked out
            if (user.LockoutEndTime.HasValue && user.LockoutEndTime.Value > DateTime.Now)
            {
                var remaining = (user.LockoutEndTime.Value - DateTime.Now).Minutes;
                ModelState.AddModelError("", $"Account is locked. Try again in {remaining} minute(s).");
                return View(model);
            }

            // ✅ Password Check using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                user.FailedLoginAttempts += 1;

                if (user.FailedLoginAttempts >= 5)
                {
                    user.LockoutEndTime = DateTime.Now.AddMinutes(5);
                    await _userRepository.UpdateUserAsync(user);
                    ModelState.AddModelError("", "Account is locked for 5 minutes due to multiple failed login attempts.");
                }
                else
                {
                    await _userRepository.UpdateUserAsync(user);
                    int remainingAttempts = 5 - user.FailedLoginAttempts;
                    ModelState.AddModelError("", $"Invalid password. {remainingAttempts} attempt(s) remaining.");
                }

                return View(model);
            }

            // ✅ Successful Login — Reset failed attempts
            user.FailedLoginAttempts = 0;
            user.LockoutEndTime = null;
            await _userRepository.UpdateUserAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);

            // ✅ Redirect based on user role
            if (user.Role.RoleName == "Admin")
                return RedirectToAction("Index", "AdminHome");
            else if (user.Role.RoleName == "User")
                return RedirectToAction("Index", "UserHome");

            return RedirectToAction("Index", "Home");
        }

        // ✅ Show Register Page
        public IActionResult Register()
        {
            return View();
        }
        
        //  Handle Registration
       
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
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                RoleId = model.RoleId,
                Password = hashedPassword,
                FailedLoginAttempts = 0,
                LockoutEndTime = null
            };

            await _userRepository.AddUserAsync(user);

            return RedirectToAction("Login");
        }

        // ✅ Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Auth");
        }

        // ✅ Access Denied Page
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

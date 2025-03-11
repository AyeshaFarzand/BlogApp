﻿using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using BlogApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

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

        // ✅ Handle Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }


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
            {
                return RedirectToAction("Index", "AdminHome");
            }
            else if(user.Role.RoleName == "User")
            {
                return RedirectToAction("Index", "UserHome");
            }
            return RedirectToAction("Index", "Home");
        }
        /*[HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Returns the same view with validation errors
            }

            // ✅ Registration logic here
            return RedirectToAction("Login");
        } */


        // ✅ Show Register Page
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
                RoleId = model.RoleId,
                Password = model.Password
            };

            await  _userRepository.AddUserAsync(user);

            return RedirectToAction("Login");
        }

        // ✅ Logout

       // public IActionResult Logout()
      //  {
      //      HttpContext.Session.Clear();
       //     return RedirectToAction("Login");
      //  }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Auth");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}

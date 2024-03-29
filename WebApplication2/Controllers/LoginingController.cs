﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using System.Linq;
using NLog;

namespace WebApplication2.Controllers
{
    public class LoginingController : Controller
    {
        private DataBaseContext _context;
        Logger logger;
        public LoginingController(DataBaseContext context)
        {
            _context = context;
            logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("News", "Users");
            }
            return View();
        }

        [HttpGet]
        public IActionResult RegistrationUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("News", "Users");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);

                if (user != null)
                {
                    await Authenticate(user);
                    switch (user.RoleId)
                    {
                        case 1: return RedirectToAction("Profil", "Users");
                        case 2: return RedirectToAction("Profil", "Users");
                    }
         
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationUser(RegistrationModel model)
        {
            User UserLogin = await _context.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
            if (UserLogin != null)
            {
                ModelState.AddModelError("Login","Такой логин уже существует");
            }

            User userEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (userEmail != null)
            {
                ModelState.AddModelError("Email", "Такая почта уже зарегистрирована");
            }

            if (ModelState.IsValid)
            {
                User UserReg = new User { Email = model.Email, Login = model.Login, Password = model.Password, RoleId = 2 };
                _context.Add(UserReg);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Logining");
            }

            return View(model);
        }

        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                new Claim("UserName", user.Login)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Logining");
        }

        public IActionResult ErrorPage(string ReturnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View(new ErrorPageModel { ReturnURL = ReturnUrl });
            }

            return RedirectToAction("Login", "Logining");
        }
    }
}
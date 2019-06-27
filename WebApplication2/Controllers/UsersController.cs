using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Authorize(Roles ="user,admin")]
    public class UsersController : Controller
    {
        private DataBaseContext _context;
        public UsersController(DataBaseContext context)
        {
            _context = context;
        }
        public IActionResult Profil()
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            TempData["LoginInOutHref"] = "/Logining/Logout";
            TempData["LoginInOut"] = "Выход";
            return View();
        }
    }
}
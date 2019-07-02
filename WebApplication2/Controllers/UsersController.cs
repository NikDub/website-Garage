using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize(Roles ="user,admin")]
    public class UsersController : Controller
    {
        private DataBaseContext _context;
        private string _AuthorizeUser;
        public UsersController(DataBaseContext context)
        {
            _context = context;
        }
        public IActionResult Profil()
        {
            _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            TempData["UserName"] = _AuthorizeUser;
            TempData["LoginInOutHref"] = "/Logining/Logout";
            TempData["LoginInOut"] = "Выход";

            var post = _context.Posts.Where(p => p.User.Login == _AuthorizeUser).ToList(); 
            return View(post);
        }

        public IActionResult AddPost()
        {
            _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            TempData["UserName"] = _AuthorizeUser;
            TempData["LoginInOutHref"] = "/Logining/Logout";
            TempData["LoginInOut"] = "Выход";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Profil(string Headline)
        {
            Post postDel = await _context.Posts.FirstOrDefaultAsync(u => u.Headline == Headline);

            if (postDel != null)
            {
                _context.Posts.Remove(postDel);
                await _context.SaveChangesAsync();
            }

            var post =  _context.Posts.Where(p => p.User.Login == _AuthorizeUser).ToList(); 

            return Redirect("/users/profil");
        }
    }
}
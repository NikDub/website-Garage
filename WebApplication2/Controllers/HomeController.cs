using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private DataBaseContext _context;
        public HomeController(DataBaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeHref();
            List<Post> post = new List<Post>(_context.Posts);
            return View(post);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Index(int iD)
        {
            return RedirectToAction("postview", "home", new { id=iD} );
        }

        public IActionResult PostView(int id)
        {
            HomeHref();
            Post post = _context.Posts.FirstOrDefault(r => r.Id == id);
            List<Post> posts = new List<Post>();
            posts.Add(post);
            return View(posts);
        }

        public void HomeHref()
        {
            TempData["News"] = "/";
            TempData["LoginInOutHref"] = "/Logining/Login";
            TempData["LoginInOut"] = "Вход";
        }
    }
}

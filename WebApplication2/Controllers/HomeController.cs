using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private DataBaseContext _context;
        Logger logger;
        public HomeController(DataBaseContext context)
        {
            _context = context;
            logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("News", "Users");
            }

            List<Post> post = new List<Post>(_context.Posts);
            if (post == null)
            {
                logger.Error("list of publications is empty");
                return Redirect("/shared/errorpage");
            }
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PostView", "Users", new { id = iD });
            }
            return RedirectToAction("PostView", "Home", new { id = iD } );
        }

        [HttpGet]
        public IActionResult PostView(int id)
        {
            Post post = _context.Posts.FirstOrDefault(r => r.Id == id);
            List<Post> posts = new List<Post>{post};
            if (post == null)
            {
                logger.Error("list of publications is empty");
                return Redirect("/shared/errorpage");
            }
            return View(posts);
        }

    }
}

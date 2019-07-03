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
            AddHref();

            var post = _context.Posts.Where(p => p.User.Login == _AuthorizeUser).ToList(); 
            return View(post);
        }

        public IActionResult AddPost()
        {
            AddHref();

            return View();
        }

        public IActionResult News()
        {
            AddHref();
            _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            var user = _context.Users.Where(r => r.Login == _AuthorizeUser).ToList();

            System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter("@param1", user[0].Id);
            var posts = _context.Posts.FromSql("NewsUsers @param1", sqlParameter).ToList();

            return View(posts);
        }

        public IActionResult Users()
        {
            AddHref();
            _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            var user = _context.Users.Where(r => r.Login == _AuthorizeUser).ToList();

            System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter("@param1", user[0].Id);
            var users = _context.Users.FromSql("UserNotSubUser @param1", sqlParameter).ToList();
            return View(users);
        }


        public IActionResult UserView(string ID)
        {
            AddHref();
            TempData["UserProfilName"] = _context.Users.Where(r => r.Id == Convert.ToInt32(ID)).ToList()[0].Login;
            TempData["UserID"] = _context.Users.Where(r => r.Id == Convert.ToInt32(ID)).ToList()[0].Id;
            var posts = _context.Posts.Where(r => r.UserId == Convert.ToInt32(ID)).ToList();
            return View(posts);
        }
        public void AddHref()
        {
            _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            TempData["News"] = "/users/news";
            TempData["UserButtonHref"] = "/users/users";
            TempData["UserButtonName"] = "Пользователи";
            TempData["UserName"] = _AuthorizeUser;
            TempData["UserNameHref"] = "/users/profil";
            TempData["LoginInOutHref"] = "/Logining/Logout";
            TempData["LoginInOut"] = "Выход";
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Profil(string Headline)
        {
            Post postDel = await _context.Posts.FirstOrDefaultAsync(u => u.Headline == Headline);

            if (postDel != null)
            {
                _context.Posts.Remove(postDel);
                await _context.SaveChangesAsync();
            }

            return Redirect("/users/profil");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> AddPost(PostModel model)
        {
            _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            var user = _context.Users.Where(r => r.Login==_AuthorizeUser).ToList();
            Post post = new Post { Headline= model.Headline, MainText=model.MainText, DateTime=DateTime.Now, User=user[0], UserId= user[0].Id};
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return Redirect("/users/profil");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Users(int Id_sub)
        {
            
            return RedirectToAction("userview", "users", new { id = Id_sub });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> News(int iD)
        {
            return RedirectToAction("postview", "users", new { id = iD });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, user")]
        public IActionResult PostView(int id)
        {
            AddHref();
            Post post = _context.Posts.FirstOrDefault(r => r.Id == id);
            List<Post> posts = new List<Post>();
            posts.Add(post);
            return View(posts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> UserView(int Id_sub)
        {
            _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            var user = _context.Users.Where(r => r.Login == _AuthorizeUser).ToList();
            Subscription subscription = new Subscription { User_Sub= Id_sub, User_Id=user[0].Id, User=user[0], UserId=user[0].Id };
            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();

            return Redirect("/users/users");
        }
    }
}
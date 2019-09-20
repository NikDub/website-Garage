using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize(Roles ="user,admin")]
    public class UsersController : Controller
    {
        private DataBaseContext _context;
        private string _AuthorizeUser;
        Logger logger;
        public UsersController(DataBaseContext context)
        {
            _context = context;
            logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        public IActionResult Profil()
        {
            _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            var post = _context.Posts.Where(p => p.User.Login == _AuthorizeUser).ToList();
            if (post == null)
            {
                logger.Error("list of publications is empty");
                return Redirect("/shared/errorpage");
            }
            return View(post);
        }

        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpGet]
        public IActionResult News()
        {
            try {
                _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
                var user = _context.Users.SingleOrDefault(r => r.Login == _AuthorizeUser);

                System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter("@param1", user.Id);
                var posts = _context.Posts.FromSql("NewsUsers @param1", sqlParameter).ToList();

                return View(posts);
            }
            catch(Exception e)
            {
                logger.Error("News" + e);
                return Redirect("/shared/errorpage");
            }
        }

        [HttpGet]
        public IActionResult Users()
        {
            try
            {
                _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
                var user = _context.Users.Where(r => r.Login == _AuthorizeUser).ToList();

                System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter("@param1", user[0].Id);
                var users = _context.Users.FromSql("UserNotSubUser @param1", sqlParameter).ToList();
                return View(users);
            }
            catch(Exception e)
            {
                logger.Error("Users" + e);
                return Redirect("/shared/errorpage");
            }
        }


        [HttpGet]
        public IActionResult UserView(string ID)
        {
            try
            {
                TempData["UserProfilName"] = _context.Users.Where(r => r.Id == Convert.ToInt32(ID)).ToList()[0].Login;
                TempData["UserID"] = _context.Users.Where(r => r.Id == Convert.ToInt32(ID)).ToList()[0].Id;
                var posts = _context.Posts.Where(r => r.UserId == Convert.ToInt32(ID)).ToList();
                return View(posts);
            }
            catch(Exception e)
            {
                logger.Error("UserView" + e);
                return Redirect("/shared/errorpage");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profil(string delete)
        {
            try
            {
                Post postDel = await _context.Posts.FirstOrDefaultAsync(u => u.Headline == delete);

                if (postDel != null)
                {
                    _context.Posts.Remove(postDel);
                    await _context.SaveChangesAsync();
                }

                return Redirect("/users/profil");
            }
            catch (Exception e){
                logger.Error("Task Profil" + e);
                return Redirect("/users/profil");
            }
        }

      /*  public async Task<IActionResult> Profil(int edit)
        {
            Post PostEdit = await _context.Posts.FirstOrDefaultAsync(u => u.Id == edit);

            return RedirectToAction();
        }
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(PostModel model)
        {
            try
            {
                _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
                var user = _context.Users.Where(r => r.Login == _AuthorizeUser).ToList();
                Post post = new Post { Headline = model.Headline, MainText = model.MainText, DateTime = DateTime.Now, User = user[0], UserId = user[0].Id };
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return Redirect("/users/profil");
            }
            catch(Exception e)
            {
                logger.Error("Task AddPost" + e);
                return Redirect("/users/profil");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Users(int Id_sub)
        {
            return RedirectToAction("userview", "users", new { id = Id_sub });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> News(int iD)
        {
            return RedirectToAction("PostView", "Users", new { id = iD });
        }

        [HttpGet]
        public IActionResult PostView(int id)
        {
            try
            {
                Post post = _context.Posts.FirstOrDefault(r => r.Id == id);
                List<Post> posts = new List<Post> { post };
                return View(posts);
            }
            catch(Exception e)
            {
                logger.Error("Task PostView" + e);
                return Redirect("/shared/errorpage");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserView(int Id_sub)
        {
            try
            {
                _AuthorizeUser = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
                var user = _context.Users.Where(r => r.Login == _AuthorizeUser).ToList();
                Subscription subscription = new Subscription { User_Sub = Id_sub, User_Id = user[0].Id, User = user[0] };
                _context.Subscriptions.Add(subscription);
                _context.SaveChanges();

                return Redirect("/users/users");
            }
            catch (Exception e)
            {
                logger.Error("Task UserView"+e);
                return Redirect("/users/users");
            }
        }
    }
}
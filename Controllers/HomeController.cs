using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Belt_Exam.Models;
using Microsoft.AspNetCore.Identity;
using Belt_Exam.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Belt_Exam.Utilities;

namespace Belt_Exam.Controllers
{
    public class HomeController : Controller
    {
        DatabaseContext database;
        PasswordHasher<Register> RegisterHasher = new PasswordHasher<Register>();
        PasswordHasher<Login> LoginHasher = new PasswordHasher<Login>();
        public HomeController(DatabaseContext x) { database = x; }


        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
            {
                return View();
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Login(LogAndReg data)
        {
            if (ModelState.IsValid)
            {
                bool CheckEmail = database.Accounts.Any(x => x.Email == data.Login.Email.ToLower());
                if (CheckEmail)
                {
                    Account Cur = database.Accounts.FirstOrDefault(x => x.Email == data.Login.Email.ToLower());
                    var CheckPassword = LoginHasher.VerifyHashedPassword(data.Login, Cur.Password, data.Login.Password);
                    if (CheckPassword != 0)
                    {
                        HttpContext.Session.SetInt32("AccountId", Cur.AccountId);
                        return RedirectToAction("Index");
                    }
                }
            }
            ModelState.AddModelError("Login.Password", "Email/Password is not recognized");
            return View("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Register(LogAndReg data)
        {
            if (ModelState.IsValid)
            {
                bool CheckEmail = database.Accounts.Any(x => x.Email == data.Register.Email);
                if (CheckEmail)
                {
                    ModelState.AddModelError("Register.Emai", "This email is associated with another account");
                    return View("Index");
                }
                Account NewAccount = new Account
                {
                    Name = data.Register.Name,
                    Alias = data.Register.Alias,
                    Email = data.Register.Email.ToLower(),
                    Password = RegisterHasher.HashPassword(data.Register, data.Register.Password)
                };
                database.Add(NewAccount);
                database.SaveChanges();
                TempData["Success"] = "Successfully Registered. You can now log-in";
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult NewPost(DashboardViewModel data)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                Post NewPost = new Post
                {
                    AccountId = (int)HttpContext.Session.GetInt32("AccountId"),
                    Content = data.NewPost.Content,
                };
                database.Add(NewPost);
                database.SaveChanges();
                return RedirectToAction("DashBoard");
            }
            data.LoggedAccount = database.Accounts.FirstOrDefault(x => x.AccountId == HttpContext.Session.GetInt32("AccountId"));
            data.Greeting = TimeUtilities.Greeting();
            data.AllPost = database.Posts
                    .Include(x => x.Creator)
                    .Include(x => x.LikesFroms)
                    .ThenInclude(y => y.Account).OrderByDescending(x => x.LikesFroms.Count).ToList();
            return View("Dashboard", data);
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
            {
                return RedirectToAction("Index");
            }
            Account cur = database.Accounts
                .Include(x => x.Likes)
                .ThenInclude(y => y.Post)
                .FirstOrDefault(x => x.AccountId == HttpContext.Session.GetInt32("AccountId"));

            DashboardViewModel Model = new DashboardViewModel
            {
                LoggedAccount = cur,
                Greeting = TimeUtilities.Greeting(),
                AllPost = database.Posts
                    .Include(x => x.Creator)
                    .Include(x => x.LikesFroms)
                    .ThenInclude(y => y.Account).OrderByDescending(x => x.LikesFroms.Count).ToList()
            };
            return View(Model);
        }


        public IActionResult Account(int id)
        {
            Account AccountInQuestion = database.Accounts.Include(z => z.Posts)
                .Include(wq => wq.Likes)
                .ThenInclude(y => y.Post)
                .FirstOrDefault(wow => wow.AccountId == id);
            AccountViewModel Model = new AccountViewModel
            {
                SaidAccount = AccountInQuestion,
                AllLikes = database.Posts.Where(post => post.AccountId == id).Include(pos => pos.LikesFroms).Select(like => like.LikesFroms.Count).ToList().Sum()
            };
            return View(Model);
        }


        public IActionResult Post(int id)
        {
            Post PostInQuestion = database.Posts
                .Include(x => x.Creator)
                .Include(x => x.LikesFroms).ThenInclude(x => x.Account)
                .FirstOrDefault(x => x.PostId == id);
            PostViewModel Model = new PostViewModel { SaidPost = PostInQuestion };
            return View(Model);
        }

        public IActionResult AddLike(int Id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
            {
                return RedirectToAction("Index");
            }
            Like newLike = new Like
            {
                AccountId = (int)HttpContext.Session.GetInt32("AccountId"),
                PostId = Id,
            };
            database.Add(newLike);
            database.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        public IActionResult RemoveLike(int Id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
            {
                return RedirectToAction("Index");
            }
            Like currentLike = database.Likes.FirstOrDefault(x => x.AccountId == HttpContext.Session.GetInt32("AccountId") && x.PostId == Id);
            database.Remove(currentLike);
            database.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        public IActionResult DeletePost (int Id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null) {
                return RedirectToAction("Index");
            }
            Post PostInQuestion = database.Posts.FirstOrDefault(x=> x.PostId == Id);
            database.Remove(PostInQuestion);
            database.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}

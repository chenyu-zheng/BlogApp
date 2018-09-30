using BlogApp.Models;
using BlogApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<PostListItem> posts = db.Posts
                .OrderByDescending(p => p.Id)
                .Take(5)
                .Select(p => new PostListItem
                {
                    Id = p.Id,
                    Title = p.Title,
                    Slug = p.Slug,
                    AuthorName = p.Author.DisplayName,
                    Created = p.Created,
                    Updated = p.Updated,
                    Snippet = p.Snippet,
                    MediaUrl = p.MediaUrl,
                    Published = p.Published,
                    CommentCount = p.Comments.Count()
                })
                .ToList();

            return View(posts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <strong>{0}</strong>({1})</p> <p>Subject: <strong>{2}</strong></p> <p>Message:</p><p>{3}</p>";
                    var from = "MyBlogApp<{0}>";

                    var email = new MailMessage(string.Format(from, WebConfigurationManager.AppSettings["emailfrom"]),
                                WebConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = "Blog App Contact Email",
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Subject, model.Body),
                        IsBodyHtml = true

                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View("_ContactSent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }

    }
}
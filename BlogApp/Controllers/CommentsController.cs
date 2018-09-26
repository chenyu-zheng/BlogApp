using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogApp.Models;
using BlogApp.ViewModels;
using Microsoft.AspNet.Identity;

namespace BlogApp.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index(string postSlug)
        {
            if (string.IsNullOrWhiteSpace(postSlug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostComments postComments = db.Posts
                .Include(p => p.Comments.Select(c => c.Author))
                .Where(p => p.Slug == postSlug)
                .Select(p => new PostComments
                {
                    Title = p.Title,
                    Slug = p.Slug,
                    Comments = p.Comments
                    .OrderByDescending(c => c.Id)
                    .ToList()
                })
                .FirstOrDefault();

            if (postComments == null)
            {
                return HttpNotFound();
            }

            return View(postComments);
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create(int? postId)
        {
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = new Comment()
            {
                PostId = postId ?? default(int)
            };
            return PartialView(comment);
        }

        // GET: Comments/Create
        public ActionResult CreateBySlug(string postSlug)
        {
            if (string.IsNullOrWhiteSpace(postSlug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = db.Posts
                .Where(p => p.Slug == postSlug)
                .Select(p => new { Id = p.Id })
                .FirstOrDefault();

            if (post == null)
            {
                return HttpNotFound();
            }

            Comment comment = new Comment()
            {
                PostId = post.Id
            };
            return PartialView("Create", comment);
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,PostId,Body")] Comment comment)
        {
            Post post = db.Posts.Where(p => p.Id == comment.PostId).FirstOrDefault();

            if (post == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                comment.AuthorId = User.Identity.GetUserId();
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("DetailsBySlug", "Posts", new { slug = post.Slug });
            }

            return RedirectToAction("DetailsBySlug", "Posts", new { slug = post.Slug });
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Edit([Bind(Include = "Id,Body,UpdateReason")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                Comment oc = db.Comments
                    .Where(p => p.Id == comment.Id)
                    .FirstOrDefault();

                oc.Body = comment.Body;
                oc.UpdateReason = comment.UpdateReason;
                oc.Updated = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index", new { postSlug = oc.Post.Slug});
            }

            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            string slug = comment.Post.Slug;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index", new { postSlug = slug });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

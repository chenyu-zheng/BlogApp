using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogApp.Helpers;
using BlogApp.Models;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult DetailsBySlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.FirstOrDefault(p => p.Slug == slug);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View("Details", post);
        }


        // GET: Posts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Title,Body,MediaUrl,Published")] Post post, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                string slug = StringUtilities.URLFriendly(post.Title);
                if (string.IsNullOrWhiteSpace(slug))
                {
                    ModelState.AddModelError(nameof(Post.Title), "Invalid title");
                    return View(post);
                }
                if (db.Posts.Any(p => p.Slug == slug))
                {
                    ModelState.AddModelError(nameof(Post.Title), "The title must be unique");
                    return View(post);
                }
                post.Slug = slug;
                if (image != null)
                {
                    if (!ImageUploadValidator.IsWebFriendlyImage(image))
                    {
                        ModelState.AddModelError(nameof(Post.MediaUrl), "Invalid image file");
                        return View(post);
                    }
                    string fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), fileName));
                    post.MediaUrl = "/Uploads/" + fileName;
                }
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,MediaUrl,Published")] Post post, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {  
                Post op = db.Posts.Where(p => p.Id == post.Id).FirstOrDefault();
                if (post.Title != op.Title)
                {
                    string slug = StringUtilities.URLFriendly(post.Title);
                    if (db.Posts.Any(p => p.Slug == slug))
                    {
                        ModelState.AddModelError(nameof(Post.Title), "The title must be unique");
                        return View(post);
                    }
                    op.Slug = slug;
                }
                if (image != null)
                {
                    if (!ImageUploadValidator.IsWebFriendlyImage(image))
                    {
                        ModelState.AddModelError(nameof(Post.MediaUrl), "Invalid image file");
                        return View(post);
                    }
                    string fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), fileName));
                    op.MediaUrl = "/Uploads/" + fileName;
                }
                op.Title = post.Title;
                op.Body = post.Body;
                op.Published = post.Published;
                op.Updated = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
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

using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogApp.ViewModels
{
    public class PostComments
    {
        public List<Comment> Comments { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
    }
}
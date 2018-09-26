using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogApp.ViewModels
{
    public class PostDetails
    {
        public Post Post { get; set; }
        public List<Comment> RecentComments { get; set; }
        public int CommentCount { get; set; }
    }
}
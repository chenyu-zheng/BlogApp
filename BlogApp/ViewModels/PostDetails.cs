﻿using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogApp.ViewModels
{
    public class PostDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string AuthorName { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string MediaUrl { get; set; }
        public string Body { get; set; }
        public List<Comment> RecentComments { get; set; }
        public int CommentCount { get; set; }
    }
}
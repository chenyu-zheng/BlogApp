﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogApp.Models
{
    public class Post
    {
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Title must be between 4 and 100 characters.")]
        [RegularExpression(@".*[a-zA-Z]+.*", ErrorMessage = "Title must contain at least one letter.")]
        public string Title { get; set; }
        public string Slug { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string MediaUrl { get; set; }
        public bool Published { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public Post()
        {
            Comments = new HashSet<Comment>();
            Created = DateTime.Now;
        }
    }
}
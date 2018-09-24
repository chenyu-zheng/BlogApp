using System;
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
        [StringLength(100, MinimumLength = 4, ErrorMessage = "The {0} must be between {2} and {1} characters.")]
        [RegularExpression(@".*[a-zA-Z]+.*", ErrorMessage = "The {0} must contain at least one letter.")]
        public string Title { get; set; }
        public string Slug { get; set; }
        [AllowHtml]
        [StringLength(20000, ErrorMessage = "The {0} cannot exceed {1} characters.")]
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
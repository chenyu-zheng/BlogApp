using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; } 
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "The comment must be between {2} and {1} characters.")]
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string UpdateReason { get; set; }

        public Comment()
        {
            Created = DateTime.Now;
        }
    }
}
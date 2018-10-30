using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Posts.Api.Models
{
    public class BlogPost
    {
        public long Id { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Should start from capital letter and consi")]
        public string Title { get; set; }
        [StringLength(4096)]
        public string Description { get; set; }
    }
}

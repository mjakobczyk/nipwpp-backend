using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Posts.Api.Models;

namespace Posts.Api.Data
{
    public class BlogPostContext : DbContext
    {

        public BlogPostContext(DbContextOptions<BlogPostContext> options) : base(options) { }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}

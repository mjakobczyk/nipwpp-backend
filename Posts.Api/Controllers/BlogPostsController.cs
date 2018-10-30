using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Posts.Api.Data;
using Posts.Api.Models;

namespace Posts.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly BlogPostContext _postsDbContext;
        public BlogPostsController(BlogPostContext postsDbContext)
        {
            _postsDbContext = postsDbContext;
        }

        // GET api/blogposts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> Get()
        {
            throw new Exception("");
            return Ok(await _postsDbContext.BlogPosts.ToAsyncEnumerable().ToList());
        }

        // POST api/blogposts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BlogPost post)
        {
            await _postsDbContext.BlogPosts.AddAsync(post);
            await _postsDbContext.SaveChangesAsync();
            return CreatedAtRoute("GetBlogPost", new { id = post.Id }, post);
        }


        // GET api/blogposts/5
        [HttpGet("{id}", Name = "GetBlogPost")]
        public ActionResult<BlogPost> Get(long id)
        {
            var item = _postsDbContext.BlogPosts.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            else
            {

                return Ok(item);
            }
        }

        // PUT api/blogposts/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] BlogPost updatedPost)
        {
            var post = _postsDbContext.BlogPosts.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            else
            {
                post.Title = updatedPost.Title;
                post.Description = updatedPost.Description;
                _postsDbContext.BlogPosts.Update(post);
                _postsDbContext.SaveChanges();
                return NoContent();
            }
        }

        // DELETE api/blogposts/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var post = _postsDbContext.BlogPosts.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            else
            {
                _postsDbContext.BlogPosts.Remove(post);
                _postsDbContext.SaveChanges();
                return NoContent();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Posts.Api.Data;
using Posts.Api.Models;
using Posts.Api.Repositories;

namespace Posts.Api.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/BlogPosts")]
    [ApiController]
    public class BlogPostsV2Controller : ControllerBase
    {
        private readonly IBlogPostRepository _postsDbRepository;

        private readonly ILogger<BlogPostsController> _logger;

        public BlogPostsV2Controller(ILogger<BlogPostsController> logger, IBlogPostRepository postsDbRepository)
        {
            this._logger = logger;
            this._postsDbRepository = postsDbRepository;
        }

        // GET api/v2/blogposts[?pageIndex=3&pageSize=10]
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))]
        [ProducesResponseType(200, Type = typeof(PaginatedItems<BlogPost>))]
        public async Task<IActionResult> Get([FromQuery]int pageIndex = -1, [FromQuery]int pageSize = 5)
        {
            if (pageIndex < 0)
            {
                var posts = this._postsDbRepository.GetAllAsync();
                return Ok(posts);
            }
            else
            {
                var pagedPosts = await this._postsDbRepository.GetAllPagedAsync(pageIndex, pageSize);
                var isLastPage = pagedPosts.TotalItems <= (pageIndex * pageSize + pageSize);
                pagedPosts.NextPage = !isLastPage ? Url.Link(null, new { pageIndex = pageIndex + 1, pageSize = pageSize }) : null;
                return Ok(pagedPosts);
            }
        }
        // GET api/v2/blogposts[?pageIndex=3&pageSize=10]
        [HttpGet("withtitle/{title:minlength(1)}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(PaginatedItems<BlogPost>))]
        public async Task<IActionResult> Get(string title, [FromQuery]int pageIndex = -1, [FromQuery]int pageSize = 5)
        {
            if (pageIndex < 0)
            {
                var posts = this._postsDbRepository.GetAllAsync();
                return Ok(posts);
            }
            else
            {
                var pagedPosts = await this._postsDbRepository.GetAllPagedAsync(pageIndex, pageSize, x => x.Title.Contains(title));
                var isLastPage = pagedPosts.TotalItems <= (pageIndex * pageSize + pageSize);
                pagedPosts.NextPage = !isLastPage ? Url.Link(null, new { pageIndex = pageIndex + 1, pageSize = pageSize }) : null;
                return Ok(pagedPosts);
            }
        }

        [HttpGet("{id}/comments", Name = "GetBlogPostComments")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(PaginatedItems<BlogPostComment>))]
        public async Task<ActionResult<IEnumerable<BlogPostComment>>> GetAllComments(long id)
        {
            var item = await this._postsDbRepository.GetAsync(id);

            if (item == null)
            {
                _logger.LogWarning("Post {0} not found", id);
                return NotFound();
            }
            else
            {
                return Ok(await this._postsDbRepository.GetCommentsAsync(id));
            }
        }

        [HttpPost("{id}/comments")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(PaginatedItems<BlogPostComment>))]
        public async Task<ActionResult<IEnumerable<BlogPostComment>>> PostComment(long id, [FromBody] BlogPostComment comment)
        {
            var item = await this._postsDbRepository.GetAsync(id);

            if (item == null)
            {
                _logger.LogWarning("Post {0} not found", id);
                return NotFound();
            }
            else
            {
                await this._postsDbRepository.AddCommentAsync(id, comment);
                return CreatedAtRoute("GetBlogPostComments", new { id = id }, comment);
            }
        }

        // POST api/blogposts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BlogPost post)
        {
            await this._postsDbRepository.AddAsync(post);
            return CreatedAtRoute("GetBlogPostV2", new { id = post.Id }, post);
        }


        // GET api/blogposts/5
        [HttpGet("{id}", Name = "GetBlogPostV2")]
        public async Task<ActionResult<BlogPost>> Get(long id)
        {
            var item = await this._postsDbRepository.GetAsync(id);

            if (item == null)
            {
                _logger.LogWarning("Post {0} not found", id);
                return NotFound();
            }
            else
            {
                return Ok(item);
            }
        }

        // PUT api/blogposts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] BlogPost updatedPost)
        {
            _logger.LogInformation("Updating post {0}", id);
            _logger.LogDebug("Received post id {0} with new title: {1}'", id, updatedPost.Title);

            var post = await this._postsDbRepository.GetAsync(id);

            if (post == null)
            {
                _logger.LogWarning("Post {0} not found", id);
                return NotFound();
            }
            else
            {  
                post.Title = updatedPost.Title;
                post.Description = updatedPost.Description;
                await this._postsDbRepository.UpdateAsync(post);
                return Ok(post);
            }
        }

        // DELETE api/blogposts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            _logger.LogInformation("Deleting post {0}", id);

            var post = await this._postsDbRepository.GetAsync(id);

            if (post == null)
            {
                _logger.LogWarning("Post {0} not found", id);
                return NotFound();
            }
            else
            {
               await this._postsDbRepository.DeleteAsync(id);
               return NoContent();
            }
        }
    }
}

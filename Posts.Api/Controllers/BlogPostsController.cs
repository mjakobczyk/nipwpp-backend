﻿using System;
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
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/BlogPosts")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _postsDbRepository;

        private readonly ILogger<BlogPostsController> _logger;

        public BlogPostsController(ILogger<BlogPostsController> logger, IBlogPostRepository postsDbRepository)
        {
            this._logger = logger;
            this._postsDbRepository = postsDbRepository;
        }

        // GET api/blogposts
        [HttpGet]
        // Add code status returning
        public async Task<ActionResult<IEnumerable<BlogPost>>> Get()
        {
            // throw new Exception(""); // Faked exception

            var list = await this._postsDbRepository.GetAllAsync().ToList();

            if (list == null)
            {
                _logger.LogWarning("No posts were found");
                return NoContent();
            }
            else
            {
                return Ok(list);
            }
        }

        // POST api/blogposts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BlogPost post)
        {
            await this._postsDbRepository.AddAsync(post);
            return CreatedAtRoute("GetBlogPost", new { id = post.Id }, post);
        }


        // GET api/blogposts/5
        [HttpGet("{id}", Name = "GetBlogPost")]
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

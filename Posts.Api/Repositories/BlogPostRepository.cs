using Posts.Api.Data;
using Posts.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Posts.Api.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogPostContext _context;

        public BlogPostRepository(BlogPostContext context)
        {
            _context = context;
        }
        public IAsyncEnumerable<BlogPost> GetAllAsync()
        {
            return this._context.BlogPosts.ToAsyncEnumerable();
        }

        public async Task<BlogPost> GetAsync(long id)
        {
            return await this._context.BlogPosts.FindAsync(id);
        }

        public async Task AddAsync(BlogPost post)
        {
            await this._context.BlogPosts.AddAsync(post);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BlogPost post)
        {
            this._context.BlogPosts.Update(post);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var post = await this.GetAsync(id);
            this._context.BlogPosts.Remove(post);
            await this._context.SaveChangesAsync();
        }

        public async Task<PaginatedItems<BlogPost>> GetAllPagedAsync(int pageIndex, int pageSize)
        {
            var totalItems = await this._context.BlogPosts.CountAsync();
            var posts = await _context.BlogPosts.OrderByDescending(c => c.Id).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

            var pagedPosts = new PaginatedItems<BlogPost> { PageIndex = pageIndex, PageSize = pageSize };
            pagedPosts.Items = posts;
            pagedPosts.TotalItems = totalItems;
            return pagedPosts;
        }
    }
}

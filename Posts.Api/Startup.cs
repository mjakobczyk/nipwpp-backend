using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Posts.Api.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Posts.Api.Models;

namespace Posts.Api
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Deprecated - in-memory database
            //services.AddDbContext<BlogPostContext>(opt => 
            //    opt.UseInMemoryDatabase("BlogPosts")
            //);

            // Deprecated - MsSQL server
            //var connection = @"Server=(localdb)\mssqllocaldb;Database=BlogPostsDb;Trusted_Connection=True;ConnectRetryCount=0";
            //services.AddDbContextPool<BlogPostContext>(options => options.UseSqlServer(connection));

            // Latest: SQLite
            var connection = @"Data Source=Data/Posts.db";
            services.AddDbContextPool<BlogPostContext>(opt => opt.UseSqlite(connection));

            services.AddSwaggerGen(c =>
            {
                Contact contact = new Contact { Name = "name", Email = "a@a.a", Url = "asdf.asdf"};
                c.SwaggerDoc("v1", new Info { Title = "Blog Posts API", Version = "v1", Contact = contact });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, BlogPostContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/api/Error");
                app.UseHsts();

                context.Database.EnsureCreated();

                if (!context.BlogPosts.Any())
                {
                    var posts = new List<BlogPost>
                    {
                        new BlogPost{Id=1, Title="RazDwaTrzy", Description="CzteryPiecSzesc"}
                    };
                    context.BlogPosts.AddRange(posts);
                    context.SaveChanges();
                }
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog Posts API v1");
            });

            
        }
    }
}

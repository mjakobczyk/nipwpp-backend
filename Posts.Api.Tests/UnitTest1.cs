using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Posts.Api.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Posts.Api.Models;
using Posts.Api.Repositories;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Testing;
using Posts.Api.Controllers;
using Xunit;

namespace Posts.Api.Tests
{
    public class BasicTests
: IClassFixture<WebApplicationFactory<Posts.Api.Startup>>
    {
        private readonly WebApplicationFactory<Posts.Api.Startup> _factory;

        public BasicTests(WebApplicationFactory<Posts.Api.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/v1/BlogPosts")]
        [InlineData("/api/v2/BlogPosts?pageIndex=0&pageSize=5")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}

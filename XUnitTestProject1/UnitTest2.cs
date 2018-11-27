using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using MyNamespace;
using System.Threading.Tasks;
using System.Net.Http;

namespace XUnitTestProject1
{
    public class BasicTests
: IClassFixture<WebApplicationFactory<Client>>
    {
        [Theory]
        [InlineData("/api/v1/BlogPosts")]
        public async Task TestGet(string a)
        {
            // Arrange
            HttpClient hc = new HttpClient();

            Client c = new Client("https://localhost:5001", hc);

            // Act
            var res = await c.ApiV1BlogPostsGetAsync();

            // Assrt
            Assert.Equal(1, res.Count);
        }
    }
}

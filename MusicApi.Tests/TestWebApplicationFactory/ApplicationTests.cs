using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MusicApi.Tests.TestWebApplicationFactory
{
    public class ApplicationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public ApplicationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_EndpointReturnsSuccessAndCorrectContentType()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Songs");
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType?.ToString());
        }
    }
}
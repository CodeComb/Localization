using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.Extensions.Primitives;
using Xunit;
using Moq;

namespace CodeComb.AspNet.Localization.Tests
{
    public class RequestCultureProviderTests
    {
        [Fact]
        public void determine_request_culture_test()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Query)
                .Returns(new QueryCollection(new Dictionary<string, StringValues> { { "lang", "zh" } }));
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);
            var queryStringProvider = new QueryStringRequestCultureProvider(accessor.Object);

            // Act
            var actual = queryStringProvider.DetermineRequestCulture();

            // Assert
            Assert.Equal(new string[] { "zh" }, actual);
        }

        [Fact]
        public void determine_request_culture_with_empty_query_string_test()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Query)
                .Returns(new QueryCollection(new Dictionary<string, StringValues>()));
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);
            var queryStringProvider = new QueryStringRequestCultureProvider(accessor.Object);

            // Act
            var actual = queryStringProvider.DetermineRequestCulture();

            // Assert
            Assert.Equal(new string[] { }, actual);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.Net.Http.Headers;
using Microsoft.Framework.Primitives;
using Xunit;
using Moq;

namespace CodeComb.AspNet.Localization.Tests
{
    public class CookieRequestCultureProviderTests
    {
        [Fact]
        public void determine_request_culture_without_cookies_test()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Headers)
                .Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "Accept-Language", new string[] { "zh,zh-CN;q=0.8,en;q=0.8" } } }));
            req.Setup(x => x.Cookies)
                .Returns(new RequestCookiesCollection());
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);
            var cookieProvider = new CookieRequestCultureProvider(accessor.Object);

            // Act
            var actual = cookieProvider.DetermineRequestCulture();

            // Assert
            Assert.Equal(new string[] { "zh", "zh-CN", "en" }, actual);
        }

        [Fact]
        public void determine_request_culture_with_cookies_test()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Headers)
                .Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "Accept-Language", new string[] { "en" } } }));
            req.Setup(x => x.Cookies)
                .Returns(new ReadableStringCollection(new Dictionary<string, StringValues> { { "ASPNET_LANG", "zh" } }));
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);
            var cookieProvider = new CookieRequestCultureProvider(accessor.Object);

            // Act
            var actual = cookieProvider.DetermineRequestCulture();

            // Assert
            Assert.Equal(new string[] { "zh" }, actual);
        }
    }
}

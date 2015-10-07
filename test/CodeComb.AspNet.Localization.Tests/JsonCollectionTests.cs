using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Dnx.Runtime;
using Microsoft.Dnx.Runtime.Infrastructure;
using Xunit;
using Moq;

namespace CodeComb.AspNet.Localization.Tests
{
    public class JsonCollectionTests
    {
        [Fact]
        public void json_collection_with_zh_test ()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Headers)
                .Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "Accept-Language", "zh" } }));
            req.Setup(x => x.Cookies)
                .Returns(new RequestCookiesCollection());
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            var collection = new ServiceCollection();
            collection.AddJsonLocalization()
                .AddCookieCulture()
                .AddInstance(accessor.Object)
                .AddInstance(CallContextServiceLocator.Locator.ServiceProvider.GetRequiredService<IApplicationEnvironment>());

            var service = collection.BuildServiceProvider();

            // Act
            var SR = service.GetService<ILocalizationStringCollection>();
            var actual_1 = SR["Hello world."];
            var actual_2 = SR["My name is {0}.", "Yuuko"];

            // Assert
            Assert.Equal("你好，世界。", actual_1);
            Assert.Equal("我的名字是Yuuko", actual_2);
        }

        [Fact]
        public void json_collection_with_en_test()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Headers)
                .Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "Accept-Language", "en-US" } }));
            req.Setup(x => x.Cookies)
                .Returns(new RequestCookiesCollection());
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            var collection = new ServiceCollection();
            collection.AddJsonLocalization()
                .AddCookieCulture()
                .AddInstance(accessor.Object)
                .AddInstance(CallContextServiceLocator.Locator.ServiceProvider.GetRequiredService<IApplicationEnvironment>());

            var service = collection.BuildServiceProvider();

            // Act
            var SR = service.GetService<ILocalizationStringCollection>();
            var actual_1 = SR["Hello world."];
            var actual_2 = SR["My name is {0}.", "Yuuko"];

            // Assert
            Assert.Equal("Hello world.", actual_1);
            Assert.Equal("My name is Yuuko.", actual_2);
        }

        [Fact]
        public void json_collection_with_default_culture_test()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Headers)
                .Returns(new HeaderDictionary(new Dictionary<string, StringValues> { }));
            req.Setup(x => x.Cookies)
                .Returns(new RequestCookiesCollection());
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            var collection = new ServiceCollection();
            collection.AddJsonLocalization()
                .AddCookieCulture()
                .AddInstance(accessor.Object)
                .AddInstance(CallContextServiceLocator.Locator.ServiceProvider.GetRequiredService<IApplicationEnvironment>());

            var service = collection.BuildServiceProvider();

            // Act
            var SR = service.GetService<ILocalizationStringCollection>();
            var actual_1 = SR["Hello world."];
            var actual_2 = SR["My name is {0}.", "Yuuko"];

            // Assert
            Assert.Equal("你好，世界。", actual_1);
            Assert.Equal("我的名字是Yuuko", actual_2);
        }
    }
}

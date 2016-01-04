using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.PlatformAbstractions;
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
                .Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "Accept-Language", new string[] { "zh" } } }));
            req.Setup(x => x.Cookies)
                .Returns(new RequestCookieCollection());
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            var collection = new ServiceCollection();
            collection.AddJsonLocalization()
                .AddCookieCulture()
                .AddSingleton(accessor.Object)
                .AddSingleton(PlatformServices.Default.Application);

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
                .Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "Accept-Language", new string[] { "en-US" } } }));
            req.Setup(x => x.Cookies)
                .Returns(new RequestCookieCollection());
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            var collection = new ServiceCollection();
            collection.AddJsonLocalization()
                .AddCookieCulture()
                .AddSingleton(accessor.Object)
                .AddSingleton(PlatformServices.Default.Application);

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
                .Returns(new RequestCookieCollection());
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            var collection = new ServiceCollection();
            collection.AddJsonLocalization()
                .AddCookieCulture()
                .AddSingleton(accessor.Object)
                .AddSingleton(PlatformServices.Default.Application);

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
        public void set_string_test()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Headers)
                .Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "Accept-Language", new string[] { "writing-test" } } }));
            req.Setup(x => x.Cookies)
                .Returns(new RequestCookieCollection());
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            var collection = new ServiceCollection();
            collection.AddJsonLocalization()
                .AddCookieCulture()
                .AddSingleton(accessor.Object)
                .AddSingleton(PlatformServices.Default.Application);

            var service = collection.BuildServiceProvider();

            // Act 1
            var SR = service.GetService<ILocalizationStringCollection>();
            SR.SetString("writing-test", "Hello world.", "Hi, I am CodeComb.AspNet.Localization");
            var actual_1 = SR["Hello world."];

            // Assert 1
            Assert.Equal("Hi, I am CodeComb.AspNet.Localization", actual_1);

            // Act 2
            SR.SetString("writing-test", "Hello world.", "你好，世界。");
            var actual_2 = SR["Hello world."];

            // Assert 2
            Assert.Equal("你好，世界。", actual_2);
        }

        [Fact]
        public void add_string_and_remove_test()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Headers)
                .Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "Accept-Language", new string[] { "writing-test" } } }));
            req.Setup(x => x.Cookies)
                .Returns(new RequestCookieCollection());
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            var collection = new ServiceCollection();
            collection.AddJsonLocalization()
                .AddCookieCulture()
                .AddSingleton(accessor.Object)
                .AddSingleton(PlatformServices.Default.Application);

            var service = collection.BuildServiceProvider();

            // Act 1
            var SR = service.GetService<ILocalizationStringCollection>();
            SR.SetString("writing-test", "Test", "I am xUnit.");
            var actual_1 = SR["Test"];

            // Assert 1
            Assert.Equal("I am xUnit.", actual_1);

            // Act 2
            SR.RemoveString("Test");
            var actual_2 = SR["Test"];

            // Assert 2
            Assert.Equal("Test", actual_2);
        }
    }
}

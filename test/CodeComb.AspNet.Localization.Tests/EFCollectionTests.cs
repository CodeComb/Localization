using System;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Data.Entity.Storage.Internal;
using CodeComb.AspNet.Localization.EntityFramework;
using Xunit;
using Moq;

namespace CodeComb.AspNet.Localization.Tests
{
    public class EFCollectionTests
    {
        private void BuildSampleData(IServiceProvider service)
        {
            service.GetRequiredService<IInMemoryStore>().Clear();
            var db = service.GetRequiredService<ILocalizationDbContext<Guid>>();
            if (db.Database != null && db.Database.EnsureCreated())
            {
                var info_1 = new CultureInfo<Guid>
                {
                    IsDefault = true,
                    Set = "zh-CN"
                };
                db.LocalizationCultureInfo.Add(info_1);
                db.LocalizationCulture.Add(new Culture<Guid> { CultureString = "zh", CultureInfoId = info_1.Id });
                db.LocalizationCulture.Add(new Culture<Guid> { CultureString = "zh-CN", CultureInfoId = info_1.Id });
                db.LocalizationCulture.Add(new Culture<Guid> { CultureString = "zh-Hans", CultureInfoId = info_1.Id });
                db.LocalizationString.Add(new LocalizedString<Guid> { CultureInfoId = info_1.Id, Key = "Hello world.", Value = "你好，世界。" });
                db.LocalizationString.Add(new LocalizedString<Guid> { CultureInfoId = info_1.Id, Key = "Code Comb Co., Ltd.", Value = "哈尔滨市码锋科技有限责任公司" });
                db.LocalizationString.Add(new LocalizedString<Guid> { CultureInfoId = info_1.Id, Key = "My name is {0}.", Value = "我的名字是{0}" });

                var info_2 = new CultureInfo<Guid>
                {
                    IsDefault = false,
                    Set = "en-US"
                };
                db.LocalizationCultureInfo.Add(info_2);
                db.LocalizationCulture.Add(new Culture<Guid> { CultureString = "en", CultureInfoId = info_2.Id });
                db.LocalizationCulture.Add(new Culture<Guid> { CultureString = "en-US", CultureInfoId = info_2.Id });
                db.LocalizationString.Add(new LocalizedString<Guid> { CultureInfoId = info_2.Id, Key = "Hello world.", Value = "Hello world." });
                db.LocalizationString.Add(new LocalizedString<Guid> { CultureInfoId = info_2.Id, Key = "Code Comb Co., Ltd.", Value = "Code Comb Co., Ltd." });
                db.LocalizationString.Add(new LocalizedString<Guid> { CultureInfoId = info_2.Id, Key = "My name is {0}.", Value = "My name is {0}." });

                var info_3 = new CultureInfo<Guid>
                {
                    IsDefault = false,
                    Set = "writing-test"
                };
                db.LocalizationCultureInfo.Add(info_3);
                db.LocalizationCulture.Add(new Culture<Guid> { CultureString = "writing-test", CultureInfoId = info_3.Id });
                db.LocalizationString.Add(new LocalizedString<Guid> { CultureInfoId = info_3.Id, Key = "Hello world.", Value = "你好，世界。" });
                db.LocalizationString.Add(new LocalizedString<Guid> { CultureInfoId = info_3.Id, Key = "Code Comb Co., Ltd.", Value = "哈尔滨市码锋科技有限责任公司" });
                db.LocalizationString.Add(new LocalizedString<Guid> { CultureInfoId = info_3.Id, Key = "My name is {0}.", Value = "我的名字是{0}" });

                db.SaveChanges();
            }
        }

        [Fact]
        public void ef_collection_with_zh_test()
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
            collection
                .AddEFLocalization<LocalizationContext, Guid>()
                .AddCookieCulture()
                .AddSingleton(accessor.Object)
                .AddSingleton(CallContextServiceLocator.Locator.ServiceProvider.GetRequiredService<IApplicationEnvironment>())
                .AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<LocalizationContext>();

            var service = collection.BuildServiceProvider();
            BuildSampleData(service);

            // Act
            var SR = service.GetService<ILocalizationStringCollection>();
            var actual_1 = SR["Hello world."];
            var actual_2 = SR["My name is {0}.", "Yuuko"];

            // Assert
            Assert.Equal("你好，世界。", actual_1);
            Assert.Equal("我的名字是Yuuko", actual_2);
        }

        [Fact]
        public void ef_collection_with_en_test()
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
            collection.AddEFLocalization<LocalizationContext, Guid>()
                .AddCookieCulture()
                .AddSingleton(accessor.Object)
                .AddSingleton(CallContextServiceLocator.Locator.ServiceProvider.GetRequiredService<IApplicationEnvironment>())
                .AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<LocalizationContext>();

            var service = collection.BuildServiceProvider();
            BuildSampleData(service);

            // Act
            var SR = service.GetService<ILocalizationStringCollection>();
            var actual_1 = SR["Hello world."];
            var actual_2 = SR["My name is {0}.", "Yuuko"];

            // Assert
            Assert.Equal("Hello world.", actual_1);
            Assert.Equal("My name is Yuuko.", actual_2);
        }

        [Fact]
        public void ef_collection_with_default_culture_test()
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
            collection.AddEFLocalization<LocalizationContext, Guid>()
                .AddCookieCulture()
                .AddSingleton(accessor.Object)
                .AddSingleton(CallContextServiceLocator.Locator.ServiceProvider.GetRequiredService<IApplicationEnvironment>())
                .AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<LocalizationContext>();

            var service = collection.BuildServiceProvider();
            BuildSampleData(service);

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
            collection.AddEFLocalization<LocalizationContext, Guid>()
                .AddCookieCulture()
                .AddSingleton(accessor.Object)
                .AddSingleton(CallContextServiceLocator.Locator.ServiceProvider.GetRequiredService<IApplicationEnvironment>())
                .AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<LocalizationContext>();

            var service = collection.BuildServiceProvider();
            BuildSampleData(service);

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
            collection.AddEFLocalization<LocalizationContext, Guid>()
                .AddCookieCulture()
                .AddSingleton(accessor.Object)
                .AddSingleton(CallContextServiceLocator.Locator.ServiceProvider.GetRequiredService<IApplicationEnvironment>())
                .AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<LocalizationContext>();

            var service = collection.BuildServiceProvider();
            BuildSampleData(service);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace CodeComb.AspNet.Localization.Tests
{
    public class LocalizedStringCollectionTests
    {
        [Fact]
        public void indexer_getter_test()
        {
            // Arrange
            var info = new List<CultureInfo>
            {
                new CultureInfo {
                    Set = "zh-CN",
                    Cultures = new List<string> { "zh", "zh-CN", "zh-Hans" },
                    IsDefault = true,
                    LocalizedStrings = new Dictionary<string, string>
                    {
                        { "Hello world.", "你好，世界。" },
                        { "Code Comb Co., Ltd.", "哈尔滨市码锋科技有限责任公司" },
                        { "My name is {0}.", "我的名字是{0}" }
                    }
                }
            };

            var cultureProvider = new Mock<IRequestCultureProvider>();
            cultureProvider.Setup(x => x.DetermineRequestCulture())
                .Returns(new string[] { "zh-CN" });

            var collection = new Mock<LocalizationStringCollection>();
            collection.Setup(x => x.Collection)
                .Returns(info);
            collection.Setup(x => x.CultureProvider)
                .Returns(cultureProvider.Object);

            // Act
            var actual_1 = collection.Object["Code Comb Co., Ltd."];
            var actual_2 = collection.Object["My name is {0}.", "Amamiya Yuuko"];

            // Assert
            Assert.Equal("哈尔滨市码锋科技有限责任公司", actual_1);
            Assert.Equal("我的名字是Amamiya Yuuko", actual_2);
        }
    }
}

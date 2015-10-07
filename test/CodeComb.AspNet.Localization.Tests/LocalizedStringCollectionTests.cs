using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeComb.AspNet.Localization.Internal;
using Xunit;
using Moq;

namespace CodeComb.AspNet.Localization.Tests
{
    public class LocalizedStringCollectionTests
    {
        [Fact]
        public void indexer_get_string_test()
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

            var collection = new DefaultLocalizationStringCollection(info, new Mock<IRequestCultureProvider>().Object);

            // Act
            var actual_1 = collection.GetString("zh", "Code Comb Co., Ltd.");
            var actual_2 = collection.GetString("zh", "My name is {0}.", "Amamiya Yuuko");

            // Assert
            Assert.Equal("哈尔滨市码锋科技有限责任公司", actual_1);
            Assert.Equal("我的名字是Amamiya Yuuko", actual_2);
        }

        [Fact]
        public void single_culture_with_not_existed_culture_test()
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
            var collection = new DefaultLocalizationStringCollection(info, new Mock<IRequestCultureProvider>().Object);

            // Act
            var actual = collection.SingleCulture(new string[] { "ja-JP", "en-US" });

            // Assert
            Assert.Equal("zh", actual);
        }

        [Fact]
        public void single_culture_with_empty_test()
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
            var collection = new DefaultLocalizationStringCollection(info, new Mock<IRequestCultureProvider>().Object);

            // Act
            var actual = collection.SingleCulture(new string[] { });

            // Assert
            Assert.Equal("zh", actual);
        }
    }
}

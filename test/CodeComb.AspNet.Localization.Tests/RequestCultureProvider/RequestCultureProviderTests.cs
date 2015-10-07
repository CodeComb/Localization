using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var theory = new string[] { "zh", "zh-CN", "zh-Hans" };

            var cultureProvider = new Mock<IRequestCultureProvider>();
            cultureProvider.Setup(x => x.DetermineRequestCulture())
                .Returns(theory);

            // Act
            var actual = cultureProvider.Object.DetermineRequestCulture();

            // Assert
            Assert.Equal(theory, actual);
        }
    }
}

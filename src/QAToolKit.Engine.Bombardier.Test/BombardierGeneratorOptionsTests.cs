using QAToolKit.Core.Models;
using System.Linq;
using Xunit;

namespace QAToolKit.Engine.Bombardier.Test
{
    public class BombardierGeneratorOptionsTests
    {
        [Fact]
        public void BombardierGeneratorOptionsAddApiKeyTest_Successful()
        {
            var options = new BombardierGeneratorOptions();
            options.AddApiKey("12345");

            Assert.Equal("12345", options.ApiKey);
        }

        [Fact]
        public void BombardierGeneratorOptionsAddBasicAuthenticationTest_Successful()
        {
            var options = new BombardierGeneratorOptions();
            options.AddBasicAuthentication("user", "password");

            Assert.Equal("user", options.UserName);
            Assert.Equal("password", options.Password);
        }

        [Fact]
        public void BombardierGeneratorOptionsAddOAuth2TokenTest_Successful()
        {
            var options = new BombardierGeneratorOptions();
            options.AddOAuth2Token("token", AuthenticationType.Administrator);

            Assert.Equal(AuthenticationType.Administrator, options.AccessTokens.FirstOrDefault(t => t.Key == AuthenticationType.Administrator).Key);
            Assert.Equal("token", options.AccessTokens.FirstOrDefault(t => t.Key == AuthenticationType.Administrator).Value);
        }

        [Fact]
        public void BombardierGeneratorOptionsTest_Successful()
        {
            var options = new BombardierGeneratorOptions
            {
                BombardierConcurrentUsers = 1,
                BombardierDuration = 12,
                BombardierRateLimit = 3,
                BombardierTimeout = 30,
                BombardierUseHttp2 = true
            };

            Assert.Equal(1, options.BombardierConcurrentUsers);
            Assert.Equal(12, options.BombardierDuration);
            Assert.Equal(3, options.BombardierRateLimit);
            Assert.Equal(30, options.BombardierTimeout);
            Assert.True(options.BombardierUseHttp2);
            Assert.False(options.BombardierInsecure);
        }
    }
}

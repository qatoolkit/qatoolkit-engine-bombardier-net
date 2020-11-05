using Xunit;

namespace QAToolKit.Engine.Bombardier.Test
{
    public class BombardierOutputOptionsTests
    {
        [Fact]
        public void BombardierOutputOptionsTrueTest_Successful()
        {
            var options = new BombardierOutputOptions
            {
                ObfuscateAuthenticationHeader = true
            };

            Assert.True(options.ObfuscateAuthenticationHeader);
        }

        [Fact]
        public void BombardierOutputOptionsFalseTest_Successful()
        {
            var options = new BombardierOutputOptions
            {
                ObfuscateAuthenticationHeader = false
            };

            Assert.False(options.ObfuscateAuthenticationHeader);
        }
    }
}

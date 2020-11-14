using QAToolKit.Engine.Bombardier.Exceptions;
using System;
using Xunit;

namespace QAToolKit.Engine.Bombardier.Test.Exceptions
{
    public class QAToolKitBombardierExceptionTests
    {
        [Fact]
        public void CreateExceptionTest_Successful()
        {
            var exception = new QAToolKitBombardierException("my error");

            Assert.Equal("my error", exception.Message);
        }

        [Fact]
        public void CreateExceptionWithInnerExceptionTest_Successful()
        {
            var innerException = new Exception("Inner");
            var exception = new QAToolKitBombardierException("my error", innerException);

            Assert.Equal("my error", exception.Message);
            Assert.Equal("Inner", innerException.Message);
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace QAToolKit.Engine.Bombardier.Exceptions
{
    internal class QAToolKitBombardierException : Exception
    {
        public QAToolKitBombardierException(string message) : base(message)
        {
        }

        public QAToolKitBombardierException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QAToolKitBombardierException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

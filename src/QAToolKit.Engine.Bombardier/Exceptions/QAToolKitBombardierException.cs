using System;
using System.Runtime.Serialization;

namespace QAToolKit.Engine.Bombardier.Exceptions
{
    /// <summary>
    /// QA Toolkit bombardier exception
    /// </summary>
    [Serializable]
    public class QAToolKitBombardierException : Exception
    {
        /// <summary>
        /// QA Toolkit bombardier exception
        /// </summary>
        public QAToolKitBombardierException(string message) : base(message)
        {
        }

        /// <summary>
        /// QA Toolkit bombardier exception
        /// </summary>
        public QAToolKitBombardierException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// QA Toolkit bombardier exception
        /// </summary>
        protected QAToolKitBombardierException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

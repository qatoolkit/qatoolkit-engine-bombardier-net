using QAToolKit.Core.Models;
using System;
using System.Linq;
using System.Net.Http;

namespace QAToolKit.Engine.Bombardier.Helpers
{
    /// <summary>
    /// Content type header helper
    /// </summary>
    public class ContentTypeHeaderHelper
    {
        /// <summary>
        /// Generate content type header
        /// </summary>
        /// <param name="request"></param>
        /// <param name="useContentType"></param>
        /// <returns></returns>
        internal static string GenerateContentTypeHeader(HttpRequest request, ContentType.Enumeration useContentType)
        {
            if (request.Method == HttpMethod.Get)
            {
                return String.Empty;
            }
            else
            {
                var contentType = request.RequestBodies.FirstOrDefault(content => content.ContentType == ContentType.Enumeration.Json);

                if (contentType != null)
                {
                    return $" -H \"Content-Type: {ContentType.From(contentType.ContentType).Value()}\"";
                }
                else
                {
                    throw new Exception($"Content type header '{useContentType}' not found in the HttpRequest.");
                }
            }
        }
    }
}

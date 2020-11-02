using QAToolKit.Core.HttpRequestTools;
using QAToolKit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;

namespace QAToolKit.Engine.Bombardier.Helpers
{
    /// <summary>
    /// Http Url helper
    /// </summary>
    public static class HttpUrlHelper
    {
        /// <summary>
        /// Generate and replace URL parameters with replacement values
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bombardierGeneratorOptions"></param>
        /// <returns></returns>
        internal static string GenerateUrlParameters(HttpRequest request, BombardierGeneratorOptions bombardierGeneratorOptions)
        {
            var queryParameters = new Dictionary<string, string>();

            //add query parameters
            foreach (var parameter in request.Parameters.Where(p => p.Value != null).Where(l => l.Location == Location.Query))
            {
                queryParameters.Add(parameter.Name, parameter.Value);
            }

            var baseUrl = new Uri($"{request.BasePath}{request.Path}").ToString();

            var url = AddQueryString(baseUrl, queryParameters).ToString();

            // var replacer = new HttpRequestDataReplacer(request, bombardierGeneratorOptions.ReplacementValues);

            //url = replacer.ReplaceUrlParameters();

            if (bombardierGeneratorOptions.ReplacementValues != null)
            {
                foreach (var replacementValue in bombardierGeneratorOptions.ReplacementValues)
                {
                    var type = replacementValue.Value.GetType();

                    if (url.Contains("{" + replacementValue.Key + "}") && type.Equals(typeof(string)))
                    {
                        url = url.Replace("{" + replacementValue.Key + "}", (string)replacementValue.Value);
                    }
                }
            }

            return url;
        }

        //Copied from AspNet HttpAbstraction library, to avoid that dependency
        private static string AddQueryString(string uri, IEnumerable<KeyValuePair<string, string>> queryString)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (queryString == null)
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";
            // If there is an anchor, then the query string must be inserted before its first occurence.
            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);
            foreach (var parameter in queryString)
            {
                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));
                hasQuery = true;
            }

            sb.Append(anchorText);
            return sb.ToString();
        }
    }
}

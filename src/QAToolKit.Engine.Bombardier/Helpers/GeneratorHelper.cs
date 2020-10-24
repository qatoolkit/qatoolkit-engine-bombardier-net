using Microsoft.AspNetCore.WebUtilities;
using QAToolKit.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace QAToolKit.Engine.Bombardier.Helpers
{
    internal static class GeneratorHelper
    {
        /// <summary>
        /// Generate Rate limit for request.
        /// </summary>
        /// <returns></returns>
        internal static string GenerateRateLimit(int rateLimit)
        {
            if (rateLimit > 0)
            {
                return $"--rate={rateLimit}";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Generate content type header
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal static string GenerateContentTypeHeader(HttpTestRequest request)
        {
            if (request.Method == HttpMethod.Get)
            {
                return "";
            }
            else
            {
                return "-H \"Content-Type: application/json\" ";
            }
        }

        /// <summary>
        /// Generate and replace URL parameters with replacement values
        /// </summary>
        /// <param name="request"></param>
        /// <param name="replacementValues"></param>
        /// <returns></returns>
        internal static string GenerateUrlParameters(HttpTestRequest request, ReplacementValue[] replacementValues)
        {
            var path = request.Path;
            foreach (var value in replacementValues)
            {
                path = path.Replace("{" + value.Key + "}", value.Value);
            }

            var queryParameters = new Dictionary<string, string>();

            //add query parameters
            foreach (var parameter in request.Parameters)
            {
                foreach (var value in replacementValues)
                {
                    if (parameter.Name == value.Key)
                    {
                        if (!queryParameters.ContainsKey(parameter.Name) && !path.Contains(value.Value))
                        {
                            queryParameters.Add(parameter.Name, value.Value);
                        }
                    }
                }
            }

            return new Uri(new Uri(request.BasePath), QueryHelpers.AddQueryString(path, queryParameters)).ToString();
        }

        /// <summary>
        /// Generate JSON body
        /// </summary>
        /// <param name="request"></param>
        /// <param name="replacementValues"></param>
        /// <returns></returns>
        internal static string GenerateJsonBody(HttpTestRequest request, ReplacementValue[] replacementValues)
        {
            if (request.Method == HttpMethod.Get)
            {
                return String.Empty;
            }
            else
            {
                var replacementValue = replacementValues.FirstOrDefault(r => r.Key == request.RequestBody.Name);

                File.WriteAllText($"{request.RequestBody.Name}.json", replacementValue.Value);

                if (replacementValue != null)
                {
                    return $"-f \"{request.RequestBody.Name}.json\" ";
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// Generate Authentication header for HTTP request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customerAccessToken"></param>
        /// <param name="administratorAccessToken"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        internal static string GenerateAuthHeader(HttpTestRequest request, string customerAccessToken, string administratorAccessToken, string apiKey)
        {
            //Check if Swagger operation description contains certain auth tags
            string authHeader;
            if (request.Description.Contains(AuthenticationType.Oauth2.Value()))
            {
                if (request.Description.Contains(AuthenticationType.Customer.Value()) && !request.Description.Contains(AuthenticationType.Administrator.Value()))
                {
                    authHeader = $"-H \"Authorization: Bearer {customerAccessToken}\" ";
                }
                else if (!request.Description.Contains(AuthenticationType.Customer.Value()) && request.Description.Contains(AuthenticationType.Administrator.Value()))
                {
                    authHeader = $"-H \"Authorization: Bearer {administratorAccessToken}\" ";
                }
                else
                {
                    authHeader = $"-H \"Authorization: Bearer {customerAccessToken}\" ";
                }
            }
            else if (request.Description.Contains(AuthenticationType.ApiKey.Value()))
            {
                authHeader = $"-H \"ApiKey: {apiKey}\" ";
            }
            else
            {
                authHeader = "";
            }

            return authHeader;
        }
    }
}

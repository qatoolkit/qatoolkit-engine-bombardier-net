using Microsoft.AspNetCore.WebUtilities;
using QAToolKit.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

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

            return String.Empty;
        }

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
                    return $"-H \"Content-Type: {ContentType.From(contentType.ContentType).Value()}\" ";
                }
                else
                {
                    throw new Exception($"Content type header '{useContentType}' not found in the HttpRequest.");
                }
            }
        }

        /// <summary>
        /// Generate and replace URL parameters with replacement values
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal static string GenerateUrlParameters(HttpRequest request)
        {
            var queryParameters = new Dictionary<string, string>();

            //add query parameters
            foreach (var parameter in request.Parameters.Where(p => p.Value != null))
            {
                queryParameters.Add(parameter.Name, parameter.Value);
            }

            var baseUrl = new Uri($"{request.BasePath}{request.Path}").ToString();
            var url = QueryHelpers.AddQueryString(baseUrl, queryParameters).ToString();

            return url;
        }

        /// <summary>
        /// Generate JSON body
        /// </summary>
        /// <param name="request"></param>
        /// <param name="useContentType"></param>
        /// <returns></returns>
        internal static string GenerateJsonBody(HttpRequest request, ContentType.Enumeration useContentType)
        {
            if (request.Method == HttpMethod.Get)
            {
                return String.Empty;
            }
            else
            {
                var useRequest = request.RequestBodies.FirstOrDefault(content => content.ContentType == ContentType.Enumeration.Json);

                if (useRequest == null)
                {
                    throw new Exception($"Request body content type '{useContentType}' not found in the HttpRequest.");
                }

                return $"-f \"{JsonSerializer.Serialize(useRequest.Properties)}\" ";


               /* var fileName = $"{Guid.NewGuid()}.json";

                if (useRequest.Properties.Count > 0)
                {
                    File.WriteAllText(fileName, JsonSerializer.Serialize(useRequest.Properties));

                    return $"-f \"{fileName}\" ";
                }
                else
                {
                    return String.Empty;
                }*/
            }
        }


        /// <summary>
        /// Generate Insecure Bombardier switch
        /// </summary>
        /// <param name="bombardierOptions"></param>
        /// <returns></returns>
        public static string GenerateInsecureSwitch(BombardierGeneratorOptions bombardierOptions)
        {
            if (bombardierOptions.BombardierInsecure)
            {
                return "--insecure";
            }

            return String.Empty;
        }

        /// <summary>
        /// Generate Authentication header for HTTP request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bombardierOptions"></param>
        /// <returns></returns>
        internal static string GenerateAuthHeader(HttpRequest request, BombardierGeneratorOptions bombardierOptions)
        {
            //Check if Swagger operation description contains certain auth tags
            string authHeader;
            if (request.Description.Contains(AuthenticationType.Oauth2.Value()))
            {
                if (request.Description.Contains(AuthenticationType.Customer.Value()) && !request.Description.Contains(AuthenticationType.Administrator.Value()))
                {
                    authHeader = GetOauth2AuthenticationHeader(bombardierOptions.AccessTokens, AuthenticationType.Customer);
                }
                else if (!request.Description.Contains(AuthenticationType.Customer.Value()) && request.Description.Contains(AuthenticationType.Administrator.Value()))
                {
                    authHeader = GetOauth2AuthenticationHeader(bombardierOptions.AccessTokens, AuthenticationType.Administrator);
                }
                else
                {
                    authHeader = GetOauth2AuthenticationHeader(bombardierOptions.AccessTokens, AuthenticationType.Customer);
                }
            }
            else if (request.Description.Contains(AuthenticationType.ApiKey.Value()))
            {
                authHeader = GetApiKeyAuthenticationHeader(bombardierOptions);
            }
            else if (request.Description.Contains(AuthenticationType.Basic.Value()))
            {
                authHeader = GetBasicAuthenticationHeader(bombardierOptions);
            }
            else
            {
                authHeader = String.Empty;
            }

            return authHeader;
        }

        private static string GetOauth2AuthenticationHeader(Dictionary<AuthenticationType, string> accessTokens, AuthenticationType authenticationType)
        {
            string authHeader;
            if (accessTokens.ContainsKey(authenticationType))
            {
                accessTokens.TryGetValue(authenticationType, out var value);
                authHeader = $"-H \"Authorization: Bearer {value}\" ";
            }
            else
            {
                throw new Exception($"One of the access token is missing (AuthenticationType {authenticationType.Value()} required).");
            }

            return authHeader;
        }

        private static string GetBasicAuthenticationHeader(BombardierGeneratorOptions bombardierOptions)
        {
            string authHeader;

            if (string.IsNullOrEmpty(bombardierOptions.UserName) && string.IsNullOrEmpty(bombardierOptions.Password))
            {
                string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(bombardierOptions.UserName + ":" + bombardierOptions.Password));

                authHeader = $"-H \"Authorization: Basic {credentials}\" ";
            }
            else
            {
                throw new Exception($"User name and password for basic authentication are missing and are required).");
            }

            return authHeader;
        }

        private static string GetApiKeyAuthenticationHeader(BombardierGeneratorOptions bombardierOptions)
        {
            string authHeader;

            if (string.IsNullOrEmpty(bombardierOptions.ApiKey))
            {
                authHeader = $"-H \"ApiKey: {bombardierOptions.ApiKey}\" ";
            }
            else
            {
                throw new Exception($"Api Key is missing and is required.");
            }

            return authHeader;
        }
    }
}

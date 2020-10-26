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
        internal static string GenerateUrlParameters(HttpTestRequest request)
        {
            var path = request.Path;

            var queryParameters = new Dictionary<string, string>();

            //add query parameters
            foreach (var parameter in request.Parameters.Where(p => p.Value != null))
            {
                queryParameters.Add(parameter.Name, parameter.Value);
            }

            return new Uri(new Uri(request.BasePath), QueryHelpers.AddQueryString(path, queryParameters)).ToString();
        }

        /// <summary>
        /// Generate JSON body
        /// </summary>
        /// <param name="request"></param>
        /// <param name="replacementValues"></param>
        /// <returns></returns>
        internal static string GenerateJsonBody(HttpTestRequest request)
        {
            if (request.Method == HttpMethod.Get)
            {
                return String.Empty;
            }
            else
            {
                if (request.RequestBody.Properties.Count > 0)
                {
                    File.WriteAllText($"{request.RequestBody.Name}.json", JsonSerializer.Serialize(request.RequestBody.Properties));

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
        internal static string GenerateAuthHeader(HttpTestRequest request, BombardierOptions bombardierOptions)
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

        internal static string GetOauth2AuthenticationHeader(Dictionary<AuthenticationType, string> accessTokens, AuthenticationType authenticationType)
        {
            string authHeader;
            if (accessTokens.ContainsKey(authenticationType))
            {
                accessTokens.TryGetValue(authenticationType, out var value);
                authHeader = $"-H \"Authorization: Bearer {value}\" ";
            }
            else
            {
                throw new Exception("One of the access token is missing.");
            }

            return authHeader;
        }

        internal static string GetBasicAuthenticationHeader(BombardierOptions bombardierOptions)
        {
            string authHeader;

            if (string.IsNullOrEmpty(bombardierOptions.UserName) && string.IsNullOrEmpty(bombardierOptions.Password))
            {
                string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(bombardierOptions.UserName + ":" + bombardierOptions.Password));

                authHeader = $"-H \"Authorization: Basic {credentials}\" ";
            }
            else
            {
                authHeader = String.Empty;
            }

            return authHeader;
        }

        internal static string GetApiKeyAuthenticationHeader(BombardierOptions bombardierOptions)
        {
            string authHeader;

            if (string.IsNullOrEmpty(bombardierOptions.ApiKey))
            {
                authHeader = $"-H \"ApiKey: {bombardierOptions.ApiKey}\" ";
            }
            else
            {
                authHeader = String.Empty;
            }

            return authHeader;
        }
    }
}

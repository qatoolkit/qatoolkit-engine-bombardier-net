using QAToolKit.Core.Models;
using QAToolKit.Engine.Bombardier.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QAToolKit.Engine.Bombardier.Helpers
{
    /// <summary>
    /// Authorization Header Helper
    /// </summary>
    public static class AuthorizationHeaderHelper
    {

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
            if (request.Description.Contains(AuthenticationType.Oauth2.Value()) || bombardierOptions.AccessTokens.Any())
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
            else if (request.Description.Contains(AuthenticationType.ApiKey.Value()) || bombardierOptions.ApiKey != null)
            {
                authHeader = GetApiKeyAuthenticationHeader(bombardierOptions);
            }
            else if (request.Description.Contains(AuthenticationType.Basic.Value()) || (bombardierOptions.UserName != null && bombardierOptions.Password != null))
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
                authHeader = $" -H \"Authorization: Bearer {value}\"";
            }
            else
            {
                throw new QAToolKitBombardierException($"One of the access token is missing (AuthenticationType {authenticationType.Value()} required).");
            }

            return authHeader;
        }

        private static string GetBasicAuthenticationHeader(BombardierGeneratorOptions bombardierOptions)
        {
            string authHeader;

            if (!string.IsNullOrEmpty(bombardierOptions.UserName) && !string.IsNullOrEmpty(bombardierOptions.Password))
            {
                string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(bombardierOptions.UserName + ":" + bombardierOptions.Password));

                authHeader = $" -H \"Authorization: Basic {credentials}\"";
            }
            else
            {
                throw new QAToolKitBombardierException($"User name and password for basic authentication are missing and are required).");
            }

            return authHeader;
        }

        private static string GetApiKeyAuthenticationHeader(BombardierGeneratorOptions bombardierOptions)
        {
            string authHeader;

            if (!string.IsNullOrEmpty(bombardierOptions.ApiKey))
            {
                authHeader = $" -H \"ApiKey: {bombardierOptions.ApiKey}\"";
            }
            else
            {
                throw new QAToolKitBombardierException($"Api Key is missing and is required.");
            }

            return authHeader;
        }
    }
}

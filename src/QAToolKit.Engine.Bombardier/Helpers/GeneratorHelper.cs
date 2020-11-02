using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QAToolKit.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;

namespace QAToolKit.Engine.Bombardier.Helpers
{
    internal static class GeneratorHelper
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



            var url = AddQueryString(baseUrl, queryParameters).ToString();

            return url;
        }

        private static string AddQueryString(
          string uri,
          IEnumerable<KeyValuePair<string, string>> queryString)
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
                    //throw new Exception($"Request body content type '{useContentType}' not found in the HttpRequest.");
                    return String.Empty;
                }

                //generate JSON body from HttpRequest object
                var body = GenerateBodyJsonString(useRequest);

                return $" -b \"{body.Replace(@"""", @"\""")}\"";
            }
        }


        private static string GenerateBodyJsonString(RequestBody requestBody)
        {
            JObject obj = new JObject();
            foreach (var property in requestBody.Properties)
            {
                var propertyType = GetSimplePropertyType(property);
                var propertyName = GetPropertyName(property);
                if (propertyType == null)
                {
                    //not supported yet
                }
                else
                {
                    obj.Add(new JProperty(propertyName, Convert.ChangeType(property.Value, propertyType)));
                }
            }

            return obj.ToString(Formatting.None);
        }

        private static string GetPropertyName(Property property)
        {
            return property.Name;
        }

        private static Type GetSimplePropertyType(Property property)
        {
            switch (property.Type)
            {
                case "integer":
                    if (property.Format == "int64")
                    {
                        return typeof(long);
                    }
                    else
                    {
                        return typeof(int);
                    }
                case "string":
                    return typeof(string);
                default:
                    return null;
            }
        }

        private static Type GetComplexPropertyType(Property property)
        {
            switch (property.Type)
            {
                case "object":
                    /*if (type.Equals(typeof(string)))
                    {
                        // if (property.Value != null)
                        //    property.Value = Faker.Lorem.Sentence(1);
                    }
                    */
                    break;
                case "array":
                    foreach (var prop in property.Properties)
                    {
                        // prop.Value = Faker.Lorem.Sentence(1);
                    }
                    break;
                case "enum":
                    foreach (var prop in property.Properties)
                    {
                        // prop.Value = Faker.Lorem.Sentence(1);
                    }
                    break;
                default:
                    throw new Exception($"{property.Type} not valid type.");
            }

            return null;
        }

        private static bool IsSimpleType(Property property)
        {
            switch (property.Type)
            {
                case "integer":
                    return true;
                case "string":
                    return true;
                case "object":
                    return false;
                case "array":
                    return true;
                case "enum":
                    return false;
                default:
                    throw new Exception($"{property.Type} not valid type.");
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
                return " --insecure";
            }

            return String.Empty;
        }

        /// <summary>
        /// Generate total number of requests
        /// </summary>
        /// <param name="bombardierOptions"></param>
        /// <returns></returns>
        public static string GenerateTotalRequestsSwitch(BombardierGeneratorOptions bombardierOptions)
        {
            if (bombardierOptions.BombardierNumberOfTotalRequests != null)
            {
                return $" --requests={bombardierOptions.BombardierNumberOfTotalRequests}";
            }

            return String.Empty;
        }

        /// <summary>
        /// Generate Rate limit for request.
        /// </summary>
        /// <returns></returns>
        internal static string GenerateRateLimitSwitch(BombardierGeneratorOptions bombardierOptions)
        {
            if (bombardierOptions.BombardierRateLimit > 0)
            {
                return $" --rate={bombardierOptions.BombardierRateLimit}";
            }

            return String.Empty;
        }

        /// <summary>
        /// Generate a Bombardier duration switch
        /// </summary>
        /// <param name="bombardierGeneratorOptions"></param>
        /// <returns></returns>
        internal static string GenerateDurationSwitch(BombardierGeneratorOptions bombardierGeneratorOptions)
        {
            return $" --duration={bombardierGeneratorOptions.BombardierDuration}s";
        }

        /// <summary>
        /// Generate timeout Bombardier switch
        /// </summary>
        /// <param name="bombardierGeneratorOptions"></param>
        /// <returns></returns>
        internal static object GenerateTimeoutSwitch(BombardierGeneratorOptions bombardierGeneratorOptions)
        {
            return $" --timeout={bombardierGeneratorOptions.BombardierTimeout}s";
        }

        /// <summary>
        /// Generate HTTP protocol switch
        /// </summary>
        /// <param name="bombardierGeneratorOptions"></param>
        /// <returns></returns>
        internal static object GenerateHttpProtocolSwitch(BombardierGeneratorOptions bombardierGeneratorOptions)
        {
            return $" --{(Convert.ToBoolean(bombardierGeneratorOptions.BombardierUseHttp2) ? "http2" : "http1")}";
        }

        /// <summary>
        /// Generate concurrent users bombardier switch
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bombardierGeneratorOptions"></param>
        /// <returns></returns>
        internal static object GenerateConcurrentSwitch(HttpRequest request, BombardierGeneratorOptions bombardierGeneratorOptions)
        {
            return $" -c {bombardierGeneratorOptions.BombardierConcurrentUsers}";
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
                throw new Exception($"One of the access token is missing (AuthenticationType {authenticationType.Value()} required).");
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
                throw new Exception($"User name and password for basic authentication are missing and are required).");
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
                throw new Exception($"Api Key is missing and is required.");
            }

            return authHeader;
        }
    }
}

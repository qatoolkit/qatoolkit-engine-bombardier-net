using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QAToolKit.Core.Models;
using System;
using System.Linq;
using System.Net.Http;

namespace QAToolKit.Engine.Bombardier.Helpers
{
    /// <summary>
    /// Http body helper
    /// </summary>
    public static class HttpBodyHelper
    {

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

        //TODO: extend to support arrays, enums, object arrays
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
                    var complextType = GetComplexPropertyType(property);


                    //      obj.Add(new JProperty(propertyName, Convert.ChangeType(property.Value, propertyType)));
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
    }
}

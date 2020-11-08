using QAToolKit.Core.HttpRequestTools;
using QAToolKit.Core.Models;

namespace QAToolKit.Engine.Bombardier.Helpers
{
    /// <summary>
    /// Http Url helper
    /// </summary>
    internal static class HttpUrlHelper
    {
        /// <summary>
        /// Generate and replace URL parameters with replacement values
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bombardierGeneratorOptions"></param>
        /// <returns></returns>
        internal static string GenerateUrlParameters(HttpRequest request, BombardierGeneratorOptions bombardierGeneratorOptions)
        {
            var generator = new HttpRequestUrlGenerator(request, options =>
            {
                options.AddReplacementValues(bombardierGeneratorOptions.ReplacementValues);
            });

            var url = generator.GetUrl();

            return url;
        }
    }
}

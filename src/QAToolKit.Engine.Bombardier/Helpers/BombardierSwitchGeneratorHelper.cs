using QAToolKit.Core.Models;
using System;

namespace QAToolKit.Engine.Bombardier.Helpers
{
    /// <summary>
    /// Bombardier switch generation helper
    /// </summary>
    internal static class BombardierSwitchGeneratorHelper

    {
        /// <summary>
        /// Generate Insecure Bombardier switch
        /// </summary>
        /// <param name="bombardierOptions"></param>
        /// <returns></returns>
        internal static string GenerateInsecureSwitch(BombardierGeneratorOptions bombardierOptions)
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
        internal static string GenerateTotalRequestsSwitch(BombardierGeneratorOptions bombardierOptions)
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
            if (bombardierOptions.BombardierRateLimit != null)
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
        /// Generate bombardier payload body switch
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bombardierGeneratorOptions"></param>
        /// <returns></returns>
        internal static object GenerateBodySwitch(HttpRequest request, BombardierGeneratorOptions bombardierGeneratorOptions)
        {
            var body = HttpBodyHelper.GenerateHttpRequestBody(request, bombardierGeneratorOptions.BombardierBodyContentType, bombardierGeneratorOptions.ReplacementValues);

            if (string.IsNullOrEmpty(body))
            {
                return String.Empty;
            }

            return $" -b \"{body.Replace(@"""", @"\""")}\"";
        }
    }
}

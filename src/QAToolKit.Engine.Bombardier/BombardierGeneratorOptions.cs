using QAToolKit.Core.Models;
using System.Collections.Generic;

namespace QAToolKit.Engine.Bombardier
{
    /// <summary>
    /// Bombardier generator options
    /// </summary>
    public class BombardierGeneratorOptions
    {
        /// <summary>
        /// A list of Oauth2 access tokens
        /// </summary>
        internal Dictionary<AuthenticationType, string> AccessTokens { get; private set; } = new Dictionary<AuthenticationType, string>();
        /// <summary>
        /// Custom ApiKey sent as a ApiKey HTTP Header
        /// </summary>
        internal string ApiKey { get; private set; }
        /// <summary>
        /// Basic Authentication user name
        /// </summary>
        internal string UserName { get; private set; }
        /// <summary>
        /// Basic Authentication password
        /// </summary>
        internal string Password { get; private set; }
        /// <summary>
        /// Bombardier concurrent users options
        /// </summary>
        public int BombardierConcurrentUsers { get; set; }
        /// <summary>
        /// Bombardier request timeout
        /// </summary>
        public int BombardierTimeout { get; set; }
        /// <summary>
        /// Bombardier test runner duration
        /// </summary>
        public int BombardierDuration { get; set; }
        /// <summary>
        /// Bombardier rate limiting per second
        /// </summary>
        public int BombardierRateLimit { get; set; }
        /// <summary>
        /// Bombardier use HTTP2 protocol
        /// </summary>
        public bool BombardierUseHttp2 { get; set; }
        /// <summary>
        /// Use http or https protocols, default is false.
        /// </summary>
        public bool BombardierInsecure { get; set; } = false;
        /// <summary>
        /// Set request body content type for Bombardier tests, default is 'application/json'
        /// </summary>
        public string BombardierBodyContentType { get; set; } = "application/json";
        /// <summary>
        /// What is the type of the test
        /// </summary>
        internal TestType TestType { get; } = TestType.LoadTest;

        /// <summary>
        /// Add Oauth2 token to the bombardier generator
        /// </summary>
        /// <param name="token"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        public BombardierGeneratorOptions AddOAuth2Token(string token, AuthenticationType authenticationType)
        {
            AccessTokens.Add(authenticationType, token);
            return this;
        }

        /// <summary>
        /// Add api key to the bombardier generator
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public BombardierGeneratorOptions AddApiKey(string apiKey)
        {
            ApiKey = apiKey;
            return this;
        }

        /// <summary>
        /// Add basic authentication to Bombardier generator
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public BombardierGeneratorOptions AddBasicAuthentication(string userName, string password)
        {
            UserName = userName;
            Password = password;
            return this;
        }
    }
}

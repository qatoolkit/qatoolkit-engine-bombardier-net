using QAToolKit.Core.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("QAToolKit.Engine.Bombardier.Test")]
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
        public int BombardierConcurrentUsers { get; set; } = 3;
        /// <summary>
        /// Bombardier request timeout
        /// </summary>
        public int BombardierTimeout { get; set; } = 30;
        /// <summary>
        /// Bombardier test runner duration
        /// </summary>
        public int BombardierDuration { get; set; } = 5;
        /// <summary>
        /// Bombardier rate limiting per second
        /// </summary>
        public int? BombardierRateLimit { get; set; } = null;
        /// <summary>
        /// Bombardier use HTTP2 protocol
        /// </summary>
        public bool BombardierUseHttp2 { get; set; } = true;
        /// <summary>
        /// Use http or https protocols, default is false.
        /// </summary>
        public bool BombardierInsecure { get; set; } = false;
        /// <summary>
        /// Cap the test with number of requests
        /// </summary>
        public int? BombardierNumberOfTotalRequests { get; set; } = null;
        /// <summary>
        /// Set request body content type for Bombardier tests, default is 'application/json'
        /// </summary>
        public ContentType.Enumeration BombardierBodyContentType { get; set; } = ContentType.Enumeration.Json;
        /// <summary>
        /// What is the type of the test
        /// </summary>
        internal TestType TestType { get; } = TestType.LoadTest;
        /// <summary>
        /// Key/value pairs of replacement values
        /// </summary>
        internal ReplacementValue[] ReplacementValues { get; private set; }

        /// <summary>
        /// Use replacement values
        /// </summary>
        /// <param name="replacementValues"></param>
        /// <returns></returns>
        public BombardierGeneratorOptions AddReplacementValues(ReplacementValue[] replacementValues)
        {
            if (replacementValues == null)
                throw new ArgumentException(nameof(replacementValues));

            ReplacementValues = replacementValues;
            return this;
        }

        /// <summary>
        /// Add Oauth2 token to the bombardier generator
        /// </summary>
        /// <param name="token"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        public BombardierGeneratorOptions AddOAuth2Token(string token, AuthenticationType authenticationType)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException(nameof(token));
            if (authenticationType == null)
                throw new ArgumentException(nameof(authenticationType));

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
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException(nameof(apiKey));

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
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException(nameof(userName));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password));

            UserName = userName;
            Password = password;
            return this;
        }
    }
}

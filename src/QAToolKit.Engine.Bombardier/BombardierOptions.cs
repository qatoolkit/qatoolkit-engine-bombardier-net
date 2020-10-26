using QAToolKit.Core.Models;
using System.Collections.Generic;

namespace QAToolKit.Engine.Bombardier
{
    public class BombardierOptions
    {
        internal Dictionary<AuthenticationType, string> AccessTokens { get; private set; } = new Dictionary<AuthenticationType, string>();
        internal string ApiKey { get; private set; }
        internal string UserName { get; private set; }
        internal string Password { get; private set; }
        public int BombardierConcurrentUsers { get; set; }
        public int BombardierTimeout { get; set; }
        public int BombardierDuration { get; set; }
        public int BombardierRateLimit { get; set; }
        public bool BombardierUseHttp2 { get; set; }
        internal TestType TestType { get; } = TestType.LoadTest;

        /// <summary>
        /// Add Oauth2 token to the bombardier generator
        /// </summary>
        /// <param name="token"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        public BombardierOptions AddOAuth2Token(string token, AuthenticationType authenticationType)
        {
            AccessTokens.Add(authenticationType, token);
            return this;
        }

        /// <summary>
        /// Add api key to the bombardier generator
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public BombardierOptions AddApiKey(string apiKey)
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
        public BombardierOptions AddBasicAuthentication(string userName, string password)
        {
            UserName = userName;
            Password = password;
            return this;
        }
    }
}

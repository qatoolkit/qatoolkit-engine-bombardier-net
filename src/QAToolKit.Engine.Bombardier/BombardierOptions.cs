using QAToolKit.Core.Models;

namespace QAToolKit.Engine.Bombardier
{
    public class BombardierOptions
    {
        internal string CustomerAccessToken { get; set; }
        internal string AdministratorAccessToken { get; private set; }
        internal string ApiKey { get; private set; }
        internal ReplacementValue[] ReplacementValues { get; private set; }
        public int BombardierConcurrentUsers { get; set; }
        public int BombardierTimeout { get; set; }
        public int BombardierDuration { get; set; }
        public int BombardierRateLimit { get; set; }
        public bool BombardierUseHttp2 { get; set; }

        public BombardierOptions AddReplacementValues(ReplacementValue[] replacementValues)
        {
            ReplacementValues = replacementValues;
            return this;
        }

        public BombardierOptions AddTokensAndApiKeys(string customerToken, string adminToken, string apiKey)
        {
            CustomerAccessToken = customerToken;
            AdministratorAccessToken = adminToken;
            ApiKey = apiKey;
            return this;
        }
    }
}

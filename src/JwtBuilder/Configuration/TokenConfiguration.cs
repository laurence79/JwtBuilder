using System;

namespace JwtBuilder
{
    public class TokenConfiguration : ITokenConfiguration
    {
        public TokenConfiguration() { }

        public string Audience { get; set; }
        public string Issuer { get; set; }
        public TimeSpan? RefreshTokenLength { get; set; }
        public string SigningKey { get; set; }
        public TimeSpan? BearerTokenLength { get; set; }

        string ITokenConfiguration.GetAudience()
        {
            return this.Audience;
        }

        string ITokenConfiguration.GetIssuer()
        {
            return this.Issuer;
        }

        TimeSpan? ITokenConfiguration.GetBearerTokenLength()
        {
            return this.BearerTokenLength;
        }

        string ITokenConfiguration.GetSigningKey()
        {
            return this.SigningKey;
        }

        TimeSpan? ITokenConfiguration.GetRefreshTokenLength()
        {
            return this.RefreshTokenLength;
        }
    }
}

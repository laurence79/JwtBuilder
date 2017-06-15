using System.Collections.Generic;

using EnsureThat;

namespace JwtBuilder
{
    public class BearerTokenSpec : ITokenSpec
    {
        public AutomaticValueResult GetAutomaticValueForClaim(ClaimKey claim, TokenBuilderContext context)
        {
            Ensure.That(claim).IsNotNull();
            Ensure.That(context).IsNotNull();

            if (claim==ClaimKey.Audience)
            {
                if (context.Configuration?.GetAudience() != null)
                    return AutomaticValueResult.CreateSuccess(claim, context.Configuration.GetAudience());
                else
                    return AutomaticValueResult.CreateFail(claim,
                        "Audience value not found in configuration.");
            }
            else if (claim == ClaimKey.Issuer)
            {
                if (context.Configuration?.GetIssuer() != null)
                    return AutomaticValueResult.CreateSuccess(claim, context.Configuration.GetIssuer());
                else
                    return AutomaticValueResult.CreateFail(claim,
                        "Issuer value not found in configuration.");
            }
            else if (claim == ClaimKey.NotBefore || claim == ClaimKey.IssuedAt)
            {
                return AutomaticValueResult.CreateSuccess(claim,
                    context.ReferenceTime.ToUnixTimestamp());
            }
            else if (claim == ClaimKey.Expires)
            {
                if (context.Configuration?.GetBearerTokenLength() != null)
                {
                    var expiry = context.ReferenceTime
                        .Add(context.Configuration.GetBearerTokenLength().Value)
                        .ToUnixTimestamp();

                    return AutomaticValueResult.CreateSuccess(claim, expiry);
                }
                else
                    return AutomaticValueResult.CreateFail(claim, 
                        "Bearer token length value not found in configuration.");
            }
            else
            {
                return AutomaticValueResult.CreateFail(claim, 
                    $"{claim} can not be automatically derived.");
            }
        }

        public IEnumerable<string> FilterClaims(IEnumerable<string> claims, TokenBuilderContext context)
        {
            return claims;
        }

        public IDictionary<string, object> GetHeader(TokenBuilderContext context)
        {
            return new Dictionary<string, object>() {
                { "alg", "HS256" },
                { "typ", "JWT" }
            };
        }
    }
}

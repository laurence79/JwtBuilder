using System.Collections.Generic;
using System.Linq;
using EnsureThat;

namespace JwtBuilder
{
    public class RefreshTokenSpec : ITokenSpec
    {
        public AutomaticValueResult GetAutomaticValueForClaim(ClaimKey claim, TokenBuilderContext context)
        {
            Ensure.That(claim).IsNotNull();
            Ensure.That(context).IsNotNull();

            if (claim == ClaimKey.Expires)
            {
                if (context.Configuration?.GetRefreshTokenLength() != null)
                {
                    var expiry = context.ReferenceTime
                        .Add(context.Configuration.GetRefreshTokenLength().Value)
                        .ToUnixTimestamp();

                    return AutomaticValueResult.CreateSuccess(claim, expiry);
                }
                else
                    return AutomaticValueResult.CreateFail(claim, 
                        "Refresh token length value not found in configuration.");
            }
            else
            {
                return AutomaticValueResult.CreateFail(claim,
                    $"{claim} can not be automatically derived.");
            }
        }

        public IEnumerable<string> FilterClaims(IEnumerable<string> claims, TokenBuilderContext context)
        {
            return claims.Intersect(new string[]
            {
                ClaimKey.Expires,
                ClaimKey.Subject
            });
        }

        public IDictionary<string, object> GetHeader(TokenBuilderContext context)
        {
            return null;
        }
    }
}

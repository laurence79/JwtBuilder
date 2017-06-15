using System.Collections.Generic;

namespace JwtBuilder
{
    public interface ITokenSpec
    {
        AutomaticValueResult GetAutomaticValueForClaim(ClaimKey claim, TokenBuilderContext context);

        IEnumerable<string> FilterClaims(IEnumerable<string> claims, TokenBuilderContext context);

        IDictionary<string, object> GetHeader(TokenBuilderContext context);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtBuilder
{
    public interface ITokenBuilder
    {
        ITokenBuilder WithClaim(ClaimKey claimKey);
        ITokenBuilder WithClaims(params ClaimKey[] claimKey);
        ITokenBuilder WithClaim<T>(ClaimKey claimKey, T value);
        ITokenBuilder WithCustomClaim<T>(string name, T value);
        Result<Token> GenerateToken(ITokenSpec tokenSpec);
        T GetClaimValue<T>(string name, ITokenSpec tokenSpec);
    }
}

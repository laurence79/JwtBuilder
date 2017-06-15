using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtBuilder
{
    public interface ITokenConfiguration
    {
        string GetIssuer();
        string GetAudience();
        string GetSigningKey();
        TimeSpan? GetBearerTokenLength();
        TimeSpan? GetRefreshTokenLength();

    }
}

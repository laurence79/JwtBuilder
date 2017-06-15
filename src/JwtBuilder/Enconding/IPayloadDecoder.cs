using System.Collections.Generic;

namespace JwtBuilder
{
    public interface IPayloadDecoder
    {
        IDictionary<string, object> Decode(string token);
    }
}

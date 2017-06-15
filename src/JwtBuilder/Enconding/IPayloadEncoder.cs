using System.Collections.Generic;

namespace JwtBuilder
{
    public interface IPayloadEncoder
    {
        string Encode(IDictionary<string, object> payload);
    }
}

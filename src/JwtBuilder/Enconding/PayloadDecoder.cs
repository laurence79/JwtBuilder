using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace JwtBuilder
{
    public class PayloadDecoder : IPayloadDecoder
    {
        public IDictionary<string, object> Decode(string token)
        {
            var json = new Base64UrlEncoder().Decode(token);

            return JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
        }
    }
}

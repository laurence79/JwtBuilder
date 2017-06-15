using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using EnsureThat;

namespace JwtBuilder
{
    public class PayloadEncoder : IPayloadEncoder
    {
        public string Encode(IDictionary<string, object> payload)
        {
            Ensure.That(payload).IsNotNull();

            var payloadJson = JsonConvert.SerializeObject(payload);

            return new Base64UrlEncoder().Encode(payloadJson);
        }
    }
}

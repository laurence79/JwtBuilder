﻿﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using Xunit;

namespace JwtBuilder.UnitTests
{
    public class PayloadEncoderTests
    {
        [Fact]
        public void Encoder_Returns_Expected()
        {
            // source : http://jwt.io sample

            var payload = new Dictionary<string, object>()
            {
                ["sub"] = "1234567890" ,
                ["name"] = "John Doe" ,
                ["admin"] = true 
            };

            IPayloadEncoder e = new PayloadEncoder();

            var output = e.Encode(payload);

            Assert.Equal(output, "eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9");
        }

        [Fact]
        public void Encoder_Throws_ArgumentNullException_On_Null_Payload()
        {
            IPayloadEncoder e = new PayloadEncoder();

            Assert.Throws<ArgumentNullException>(() =>
            {
                e.Encode(null);
                return false;
            });
        }
    }
}

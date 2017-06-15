﻿﻿﻿﻿﻿﻿﻿using System;
using Xunit;

namespace JwtBuilder.UnitTests
{
    public class JwsTokenSignerTests
    {
        [Fact]
        public void Signers_Returns_Correct_Value()
        {
            // source : http://jwt.io

            var input = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9";

            IJwsTokenSigner signer = new JwsTokenSigner(
                signingKey: "c2VjcmV0"); // base64("secret")

            var signature = signer.SignToken(input);

			Assert.Equal("TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ", signature);
		}

        [Fact]
        public void Signer_Throws_ArgumentException_On_Null_SigningKey()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                IJwsTokenSigner signer = new JwsTokenSigner(null);
                return false;
            });
        }

        [Fact]
        public void Signer_Throws_ArgumentException_On_Null_Token()
		{
            Assert.Throws<ArgumentNullException>(() =>
            {
                IJwsTokenSigner signer = new JwsTokenSigner(
                    signingKey: "c2VjcmV0"); // base64("secret")

                var signature = signer.SignToken(null);
                return false;
            });
        }
    }
}

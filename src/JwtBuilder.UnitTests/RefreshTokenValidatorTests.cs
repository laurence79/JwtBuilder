﻿﻿﻿﻿﻿using Xunit;

namespace JwtBuilder.UnitTests
{
    public class RefreshTokenValidatorTests
    {
        [Fact]
        public void Validates_Token()
        {
            const string expenseAidToken = "eyJ1c2VybmFtZSI6IkVBQUFBS1V1M1ZQMmdKdGFURFFSME1FV01rOFVmZHV3YlBQbjkwcTIrdVp2RHdGZ2dHZ0dOQkJ3NlBlZ255bnRDRWxHMmc9PSIsInNlc3Npb25JZCI6IjQwOTk0IiwiZXhwIjoxNDc0MzU5MDk4LjU2NDgwNjd9.zZy_0Ms86E4prSRNG6O1CERzgeC1q30rkXMrTzt-EAI";
            const string expenseAidSigningKey = "eWHWsyQ0mCXcm9H0PB1R3ilu4aToXlud2yR6JmD5";

            var validator = new RefreshTokenValidator(
                tokenSigner: new JwsTokenSigner(
                    signingKey: expenseAidSigningKey),
                payloadDecoder: new PayloadDecoder());

            var tokenResult = Token.Parse(expenseAidToken);

            var result = validator.Validate(tokenResult.Value);

            Assert.Null(result.FailureReason);
            Assert.True(result.IsValid);

        }
    }
}

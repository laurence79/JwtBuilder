﻿﻿﻿﻿﻿﻿using System.Linq;
using Xunit;

namespace JwtBuilder.UnitTests
{
    public class TokenTests
    {
        [Fact]
        public void Parses_Token()
        {
            const string tokenStr = "1.2.3";

            var result = Token.Parse(tokenStr);

            Assert.True(result.IsSuccess);

            Assert.Equal(result.Value.Parts.Count(), 3);
        }

        [Fact]
        public void Appends_Part()
        {
            var token = new Token();

            token = token.AppendPart("1").Value;
            token = token.AppendPart("2").Value;
            token = token.AppendPart("3").Value;

            Assert.Equal(token.Parts.Count(), 3);
        }

    }
}

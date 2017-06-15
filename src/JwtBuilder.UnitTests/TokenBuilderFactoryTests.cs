﻿﻿﻿﻿﻿using System;
using Xunit;
using Moq;

namespace JwtBuilder.UnitTests
{
    public class TokenBuilderFactoryTests
    {
        [Fact]
        public void Creates_Builder_With_Correct_Config()
        {
            var config = Mock.Of<ITokenConfiguration>();

            var factory = new TokenBuilderFactory(config);

            var builder = factory.CreateBuilder() as TokenBuilder;

            Assert.Equal(builder.configuration, config);   
        }

        [Fact]
        public void Throws_Exception_On_Null_Config()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new TokenBuilderFactory(null);
            });
        }
    }
}

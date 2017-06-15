using System;

namespace JwtBuilder
{
    public class TokenBuilderContext
    {
        public TokenBuilderContext(
            DateTime referenceTime,
            ITokenConfiguration configuration)
        {
            this.ReferenceTime = referenceTime;
            this.Configuration = configuration;
        }

        public DateTime ReferenceTime { get; private set; }

        public ITokenConfiguration Configuration { get; private set; }
        
    }
}

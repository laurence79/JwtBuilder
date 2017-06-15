using EnsureThat;

namespace JwtBuilder
{
    public class TokenBuilderFactory : ITokenBuilderFactory
    {
        public TokenBuilderFactory(
            ITokenConfiguration configuration)
        {
            Ensure.That(configuration).IsNotNull();

            this.configuration = configuration;
        }

        internal ITokenConfiguration configuration { get; set; }

        public ITokenBuilder CreateBuilder()
        {
            return new TokenBuilder(
                configuration: configuration);
        }
    }
}

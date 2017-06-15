using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;

namespace JwtBuilder
{
    public class TokenBuilder : ITokenBuilder
    {
        internal TokenBuilder(
            ITokenConfiguration configuration,
            IJwsTokenSigner signer = null,
            IPayloadEncoder payloadEncoder = null)
        {
            Ensure.That(configuration).IsNotNull();

            this.configuration = configuration;
            this.signer = signer;
            this.payloadEncoder = payloadEncoder;

            this.referenceTime = DateTime.UtcNow;
        }

        internal ITokenConfiguration configuration { get; set; }
        internal IJwsTokenSigner signer { get; set; }
        internal IPayloadEncoder payloadEncoder { get; set; }
        internal DateTime referenceTime { get; set; }
        internal Dictionary<string, object> payload { get; set; } 
            = new Dictionary<string, object>();
        internal List<string> automaticClaims { get; set; }
            = new List<string>();
        

        public T GetClaimValue<T>(string key, ITokenSpec tokenSpec)
        {
            T value = default(T);

            if (this.automaticClaims.Contains(key))
            {
                var context = this.createContext();

                var result = tokenSpec
                    .GetAutomaticValueForClaim(key, context)
                    .ThrowOnFail();

                value = (T)result.Value;
            }
            else if (this.payload.Keys.Contains(key))
            {
                value = (T)this.payload[key];
            }

            return value;
        }

        public ITokenBuilder WithClaim(ClaimKey claimKey)
        {
            this.throwOnDuplicateClaim(claimKey);

            this.automaticClaims.Add(claimKey);

            return this;
        }

        public ITokenBuilder WithClaims(params ClaimKey[] claimKeys)
        {
            foreach (var claim in claimKeys)
            {
                this.throwOnDuplicateClaim(claim);
                this.automaticClaims.Add(claim);
            }
            
            return this;
        }

        public ITokenBuilder WithClaim<T>(ClaimKey claimKey, T value)
        {
            this.throwOnDuplicateClaim(claimKey);

            this.payload[claimKey] = value;

            return this;
        }

        public ITokenBuilder WithCustomClaim<T>(string name, T value)
        {
            this.throwOnDuplicateClaim(name);

            this.payload[name] = value;

            return this;
        }
        
        public Result<Token> GenerateToken(ITokenSpec tokenSpec)
        {
            if (tokenSpec == null)
                return Result.Fail<Token>("No token spec supplied.");
            
            var context = createContext();

            var encoder = this.payloadEncoder ?? new PayloadEncoder();

            var signer = getSigner();
            if (!signer.IsSuccess)
                return Result.Fail<Token>(signer.Error);
            
            var header = tokenSpec.GetHeader(context);

            return ((Result<Token>)new Token())
                .OnSuccess(t =>
                {
                    if (header != null)
                        return t.AppendPart(encoder.Encode(header));
                    else
                        return t;
                })
                .OnSuccess(t =>
                {
                    var allClaimsInBuilder =
                        this.payload.Keys.Concat(this.automaticClaims);

                    var filteredClaims = tokenSpec.FilterClaims(
                        claims: allClaimsInBuilder, 
                        context: context);

                    var compiledClaims = this.payload
                        .Where(kvp => filteredClaims.Contains(kvp.Key))
                        .Concat(this.deriveAutomaticClaims(
                            tokenSpec: tokenSpec,
                            automaticClaims: this.automaticClaims.Intersect(filteredClaims),
                            context: context))
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                    return t.AppendPart(
                        tokenPart: encoder.Encode(compiledClaims));
                })
                .OnSuccess(t =>
                {
                    return t.AppendPart(
                        tokenPart: signer.Value.SignToken(t.ToString()));
                });
        }

        private Result<IJwsTokenSigner> getSigner()
        {
            if (this.signer != null)
            {
                return Result.Ok(this.signer);
            }
            else if (this.configuration.GetSigningKey() != null)
            {
                return new JwsTokenSigner(
                    signingKey: this.configuration.GetSigningKey());
            }
            else
                return Result.Fail<IJwsTokenSigner>(
                    error: "No suitable signing method could be found.");           
        }

        private TokenBuilderContext createContext()
        {
            return new TokenBuilderContext(
                referenceTime: this.referenceTime,
                configuration: this.configuration);
        }

        private IDictionary<string, object> deriveAutomaticClaims(
            ITokenSpec tokenSpec, 
            IEnumerable<string> automaticClaims,
            TokenBuilderContext context)
        {
            Dictionary<string, object> answer = new Dictionary<string, object>();

            foreach (var claim in automaticClaims)
            {
                var result = tokenSpec
                    .GetAutomaticValueForClaim(claim, context)
                    .ThrowOnFail();

                if (result.Value != null)
                    answer[claim] = result.Value;
            }

            return answer;
        }

        private void throwOnDuplicateClaim(string name)
        {
            if (this.payload.Keys
                .Concat(this.automaticClaims)
                .Contains(name))
                throw new InvalidOperationException(
                    message: $"Claim {name} is already present and can not be specified twice.");
        }
    }
}

using EnsureThat;

namespace JwtBuilder
{
    public class RefreshTokenValidator : ITokenValidator
    {
        public RefreshTokenValidator(
            IJwsTokenSigner tokenSigner,
            IPayloadDecoder payloadDecoder)
        {
            Ensure.That(tokenSigner).IsNotNull();
            Ensure.That(payloadDecoder).IsNotNull();

            this.tokenSigner = tokenSigner;
            this.payloadDecoder = payloadDecoder;
        }

        public RefreshTokenValidator(
            ITokenConfiguration configuration)
        {
            Ensure.That(configuration).IsNotNull();

            this.tokenSigner = new JwsTokenSigner(
                signingKey: configuration.GetSigningKey());

            this.payloadDecoder = new PayloadDecoder();
        }

        internal IJwsTokenSigner tokenSigner { get; set; }
        internal IPayloadDecoder payloadDecoder { get; set; }

        public ValidationResult Validate(Token token)
        {
            if (token==null)
            {
                return ValidationResult.CreateInvalid(
                    token: null,
                    failureReason: "Token is null");
            }

            if (token.Parts.Count != 2)
                return ValidationResult.CreateInvalid(
                    token: token,
                    failureReason: "Invalid number of segments for a refresh token.");

            var expectedSignature = tokenSigner.SignToken(token.Parts[0]);
            var actualSignature = token.Parts[1];

            if (expectedSignature != actualSignature)
                return ValidationResult.CreateInvalid(
                    token: token,
                    failureReason: $"Signature of refresh token not valid. Expected {expectedSignature} got {actualSignature}.");

            var payload = payloadDecoder.Decode(token.Parts[0]);

            return ValidationResult.CreateValid(
                token: token,
                payload: payload);
        }
    }
}

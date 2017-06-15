using System.Collections.Generic;

namespace JwtBuilder
{
    public class ValidationResult
    {
        internal ValidationResult()
        { }

        public bool IsValid { get; private set; }
        public string FailureReason { get; private set; }
        public Token Token { get; private set; }
        public IDictionary<string, object> Payload { get; private set; }

        internal static ValidationResult CreateInvalid(
            Token token,
            string failureReason)
        {
            return new ValidationResult()
            {
                IsValid = false,
                Token = token,
                FailureReason = failureReason
            };
        }

        internal static ValidationResult CreateValid(
            Token token,
            IDictionary<string, object> payload)
        {
            return new ValidationResult()
            {
                IsValid = true,
                Token = token,
                Payload = payload
            };
        }
    }
}

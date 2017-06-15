using System;

namespace JwtBuilder
{
    public class AutomaticValueResult
    {
        private AutomaticValueResult()
        { }

        public string Claim { get; private set; }

        public object Value { get; private set; }

        public bool Success { get; private set; }

        public string FailureReason { get; private set; }

        public static AutomaticValueResult CreateSuccess(string claim, object value)
        {
            return new AutomaticValueResult()
            {
                Claim = claim,
                Success = true,
                Value = value
            };
        }

        public static AutomaticValueResult CreateFail(string claim, string reason)
        {
            return new AutomaticValueResult()
            {
                Claim = claim,
                Success = false,
                FailureReason = reason
            };
        }

        public AutomaticValueResult ThrowOnFail()
        {
            if (!this.Success)
                throw new Exception(
                    message: $"Error deriving automatic claim {this.Claim}. {this.FailureReason}");

            return this;
        }
    }
}

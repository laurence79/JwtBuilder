using System.Security.Cryptography;
using EnsureThat;

namespace JwtBuilder
{
    public class JwsTokenSigner : IJwsTokenSigner
    {
        public JwsTokenSigner(string signingKey)
        {
            Ensure.That(signingKey).IsNotNullOrEmpty();

            this.signingKey = new Base64UrlEncoder().DecodeBytes(signingKey);
        }

        internal byte[] signingKey;

        public string SignToken(string token)
        {
            Ensure.That(token).IsNotNullOrEmpty();

            var signer = new HMACSHA256(this.signingKey);

            byte[] signatureByte = signer.ComputeHash(System.Text.Encoding.UTF8.GetBytes(token));

            return new Base64UrlEncoder().Encode(signatureByte);
        }
    }
}

using EnsureThat;

namespace JwtBuilder
{
    public class ClaimKey
    {
        internal ClaimKey(string key)
        {
            Ensure.That(key).IsNotNull();

            this.Key = key;
        }

        public string Key { get; private set; }

        public static ClaimKey Audience = new ClaimKey("aud");
        public static ClaimKey Issuer = new ClaimKey("iss");
        public static ClaimKey IssuedAt = new ClaimKey("iat");
        public static ClaimKey NotBefore = new ClaimKey("nbf");
        public static ClaimKey Expires = new ClaimKey("exp");
        public static ClaimKey Subject = new ClaimKey("sub");
        public static ClaimKey GroupMembership = new ClaimKey("grp");

        public static implicit operator string(ClaimKey value)
        {
            return value.Key;
        }

        public static implicit operator ClaimKey(string value)
        {
            return new ClaimKey(value);
        }

        public static bool operator !=(ClaimKey a, ClaimKey b)
        {
            return a.Key != b.Key;
        }

        public static bool operator ==(ClaimKey a, ClaimKey b)
        {
            return a.Key == b.Key;
        }

        public override string ToString()
        {
            return Key;
        }

        public override int GetHashCode()
        {
            if (this.Key != null)
                return this.Key.GetHashCode();

            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is ClaimKey && (obj as ClaimKey).Key == this.Key;
        }

    }
}

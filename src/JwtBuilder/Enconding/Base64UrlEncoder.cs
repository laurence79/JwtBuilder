using System;
using System.Text;

namespace JwtBuilder
{
    internal class Base64UrlEncoder
    {
        static readonly char[] padding = { '=' };

        public string Encode(string original)
        {
            return Encode(Encoding.ASCII.GetBytes(original));
        }

        public string Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd(padding).Replace('+', '-').Replace('/', '_');
        }

        public string Decode(string encodedString)
        {
			return Encoding.ASCII.GetString(DecodeBytes(encodedString));
        }

        public byte[] DecodeBytes(string encodedString)
		{
			string incoming = encodedString
				.Replace('_', '/').Replace('-', '+');

			switch (encodedString.Length % 4)
			{
				case 2: incoming += "=="; break;
				case 3: incoming += "="; break;
			}

			return Convert.FromBase64String(incoming);
            
        }
    }
}

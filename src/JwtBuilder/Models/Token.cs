using System;
using System.Collections.Generic;

namespace JwtBuilder
{
    public class Token
    {
        public Token()
        { }

        public IReadOnlyList<string> Parts { get; private set; }
            = new List<string>();

        public static Result<Token> Parse(string token)
        {
            if (token == null)
                return Result.Fail<Token>("Token is null");

            return new Token()
                {
                    Parts = token.Split('.')
                };
        }

        public static Result<Token> Create(string firstPart)
        {
            if (String.IsNullOrEmpty(firstPart))
                return Result.Fail<Token>(
                    error: "Token part is null or empty");

            return new Token()
                {
                    Parts = new List<string>(new[] { firstPart })
                };
        }

        public Result<Token> AppendPart(string tokenPart)
        {
            if (tokenPart == null)
                return Result.Fail<Token>("Token part is null");

            var newParts = new List<string>();

            newParts.AddRange(this.Parts);
            newParts.Add(tokenPart);

            return new Token()
                {
                    Parts = newParts
                };
        }

        public override string ToString()
        {
            return String.Join(".", this.Parts);
        }
    }
}

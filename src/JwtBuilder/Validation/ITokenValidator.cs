namespace JwtBuilder
{
    public interface ITokenValidator
    {
        ValidationResult Validate(Token token);
    }
}

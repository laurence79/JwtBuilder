namespace JwtBuilder
{
    public interface IJwsTokenSigner
    {
        string SignToken(string token);
    }
}

namespace message.Options;


public class AuthOptions
{
    public JwtOptions Jwt { get; set; }

    public class JwtOptions
    {
        public string SigningKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public TimeSpan Expiration { get; set; }
    }
}
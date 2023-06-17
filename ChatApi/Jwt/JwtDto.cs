namespace IdentityApi.Jwt
{
    public class JwtDto
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string SigningKey { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}

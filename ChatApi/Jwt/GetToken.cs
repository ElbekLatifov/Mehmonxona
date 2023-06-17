using IdentityApi.Entities;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace IdentityApi.Jwt
{
    public class GetToken
    {
        private readonly JwtDto _token;

        public GetToken(IOptions<JwtDto> token)
        {
            _token = token.Value;
        }

        public string Token(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var singkey = System.Text.Encoding.UTF32.GetBytes(_token.SigningKey);
            var security = new JwtSecurityToken(
                issuer: _token.ValidIssuer,
                audience: _token.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_token.ExpiresInSeconds),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(singkey), algorithm: SecurityAlgorithms.HmacSha256)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(security);

            return token;
        }
    }
}

using System;
using System.IdentityModel.Tokens.Jwt;

namespace Assets
{
    public class Token
    {
        public virtual string AccessToken { get; set; }

        public virtual string RefreshToken { get; set; }

        public virtual string IdentityToken { get; set; }

        public virtual int ExpiresIn { get; set; }

        public virtual DateTimeOffset AccessTokenExpiration { get; internal set; }

        public bool IsTokenValid(string accessToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            return !(jwtSecurityTokenHandler.ReadJwtToken(accessToken).ValidTo < DateTime.UtcNow);
        }
    }
}

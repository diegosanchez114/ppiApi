using System.IdentityModel.Tokens.Jwt;

namespace UPIT.API.Services
{
    public interface ITokenService
    {
        string GetToken();
        string GetClaimValue(string claimType);
    }

    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken()
        {
            var token = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            return token!;
        }

        public string GetClaimValue(string claimType)
        {
            var token = GetToken();
            if (token == null)
                return null!;

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

          
            var claimValue = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
            return claimValue!;          
        }
    }

}

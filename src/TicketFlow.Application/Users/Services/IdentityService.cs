using System.Threading.Tasks;
using TicketFlow.Application.Users.Interfaces;
using TicketFlow.Application.Users.DTO;
using Microsoft.AspNetCore.Http;

namespace TicketFlow.Application.Users.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenManager _tokenManager;

        private string _tokenCookie = "token";

        public IdentityService(IHttpContextAccessor httpContextAccessor, ITokenManager tokenManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenManager = tokenManager;
        }

        public void SignIn(JwtDto dto)
            => _httpContextAccessor.HttpContext.Response.Cookies.Append(
                _tokenCookie,
                dto.AccessToken,
                new CookieOptions()
                {
                    HttpOnly = true
                }
            );

        public void SignOut()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(_tokenCookie);

            _tokenManager.DeactiveCurrentToken();
        }

        public bool IsSignedIn()
        {
            var token = _tokenManager.GetCurrentToken();

            return !string.IsNullOrWhiteSpace(token);
        }
    }
}
using System;
using System.Threading.Tasks;
using System.Linq;
using TicketFlow.Application.Users.Interfaces;
using TicketFlow.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;

namespace TicketFlow.Application.Users.Services
{
    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtOptions> _jwtOptions;

        public TokenManager(IDistributedCache cache, IHttpContextAccessor httpContextAccessor, IOptions<JwtOptions> jwtOptions)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
        }

        public string GetCurrentToken()
            => _httpContextAccessor.HttpContext.Request.Cookies["token"];

        public async Task<bool> IsCurrentTokenActive()
            => await IsActiveToken(GetCurrentToken());

        public async Task DeactiveCurrentToken()
            => await DeactiveToken(GetCurrentToken());

        public async Task<bool> IsActiveToken(string token)
            => await _cache.GetStringAsync(GetKey(token)) == null;

        private async Task DeactiveToken(string token)
            => await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(_jwtOptions.Value.ExpiryMinutes)
                });        

        private string GetKey(string token)
            => $"tokens:{token}:deactivated";
    }
}
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Application.Users.DTO;
using TicketFlow.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;

namespace TicketFlow.Application.Users.Services
{
    public interface IUserManager
    {
        Task<UserDto> GetCurrentUser();
    }

    public class UserManager : IUserManager
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenManager _tokenManager;

        public UserManager(ApplicationDbContext context, ITokenManager tokenManager)
        {
            _context = context;
            _tokenManager = tokenManager;
        }

        public async Task<UserDto> GetCurrentUser()
        {
            var token = _tokenManager.GetCurrentToken();

            var tokenS = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            var sub = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            var user = await _context.Users
                .Where(user => user.Id == new Guid(sub))
                .FirstOrDefaultAsync();

            return user is null ? null : new UserDto(user);
        }
    }
}
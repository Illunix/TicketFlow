using TicketFlow.Application.Users.DTO;

namespace TicketFlow.Application.Users.Interfaces
{
    public interface IJwtHandler
    {
         JwtDto CreateToken(string userId, string role);
    }
}
using System.Threading.Tasks;
using TicketFlow.Application.Users.DTO;

namespace TicketFlow.Application.Users.Interfaces
{
    public interface IUserManager
    {
         Task<UserDto> GetCurrentUser();
    }
}
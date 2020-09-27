using System.Threading.Tasks;
using TicketFlow.Application.Users.DTO;

namespace TicketFlow.Application.Users.Interfaces
{
    public interface IIdentityService
    {
         void SignIn(JwtDto dto);
         void SignOut();
         bool IsSignedIn();
    }
}
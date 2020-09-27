using System.Threading.Tasks;
using TicketFlow.Application.Users.DTO;

namespace TicketFlow.Application.Users.Interfaces
{
    public interface ITokenManager
    {
        string GetCurrentToken();
        Task<bool> IsCurrentTokenActive();
        Task DeactiveCurrentToken();
    }
}
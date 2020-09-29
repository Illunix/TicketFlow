using TicketFlow.Application.Users.Services;

namespace TicketFlow.Application.Users.Services
{
    public interface ISignInManager
    {
        bool IsSignedIn();
    }

    public class SignInManager : ISignInManager
    {
        private readonly ITokenManager _tokenManager;

        public SignInManager(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public bool IsSignedIn()
        {
            var token = _tokenManager.GetCurrentToken();

            return !string.IsNullOrWhiteSpace(token);
        }
    }
}
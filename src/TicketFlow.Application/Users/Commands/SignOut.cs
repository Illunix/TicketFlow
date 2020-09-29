using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using TicketFlow.Application.Users.Services;

namespace TicketFlow.Application.Users.Commands
{
    public class SignOut
    {
        public class Command : IRequest
        {
        }

        private class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ITokenManager _tokenManager;

            public CommandHandler(IHttpContextAccessor httpContextAccessor, ITokenManager tokenManager)
            {
                _httpContextAccessor = httpContextAccessor;
                _tokenManager = tokenManager;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("token");

                await _tokenManager.DeactiveCurrentToken();
            }
        }
    }
}
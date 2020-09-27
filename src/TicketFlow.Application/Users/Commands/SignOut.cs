using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TicketFlow.Application.Users.Interfaces;

namespace TicketFlow.Application.Users.Commands
{
    public class SignOut
    {
        public class Command : IRequest
        {
        }

        private class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly IIdentityService _identityService;

            public CommandHandler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                _identityService.SignOut();
            }
        }
    }
}
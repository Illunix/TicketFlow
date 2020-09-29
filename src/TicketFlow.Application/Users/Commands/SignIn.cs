using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using TicketFlow.Application.Users.Services;
using TicketFlow.Infrastructure.Data;

namespace TicketFlow.Application.Users.Commands
{
    public class SignIn
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string RememberMe { get; set;}
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IPasswordService passwordService, ApplicationDbContext context)
            {
                RuleFor(user => user.Email)
                    .NotEmpty().WithMessage("Please enter your email.")
                    .EmailAddress().WithMessage("Please enter a valid email address.");

                RuleFor(user => user.Password)
                    .NotEmpty().WithMessage("Please enter your password.");

                RuleFor(user => user.Password)
                    .MustAsync(async (request, password, cancellationToken) =>
                    { 
                        var user = await context.Users
                            .Where(user => user.Email == request.Email)
                            .FirstOrDefaultAsync();

                        return !(user is null || !passwordService.IsValid(user.Password, request.Password));
                    }).WithMessage("Check the information you entered and try again.").When(user => !string.IsNullOrWhiteSpace(user.Password));
            }
        }

        private class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IJwtHandler _jwtHandler;
            private readonly ITokenManager _tokenManager;

            public CommandHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IJwtHandler jwtHandler, ITokenManager tokenManager)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
                _jwtHandler = jwtHandler;
                _tokenManager = tokenManager;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Where(user => user.Email == request.Email)
                    .FirstOrDefaultAsync();

                var jwt = _jwtHandler.CreateToken(user.Id.ToString(), user.Role);

                _httpContextAccessor.HttpContext.Response.Cookies.Append(
                    "token",
                    jwt.AccessToken,
                    new CookieOptions()
                    {
                        HttpOnly = true
                    });
            }
        }
    }
}
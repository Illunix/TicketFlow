using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Core.Entities;
using TicketFlow.Infrastructure.Data;
using TicketFlow.Application.Users.Services;

namespace TicketFlow.Application.Users.Commands
{
    public class SignUp
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
            public string Role { get; set; } 
            public DateTime CreatedAt { get; set; }
        }   

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(ApplicationDbContext context)
            {
                RuleFor(user => user.Email)
                    .NotEmpty().WithMessage("Please enter email address.")
                    .EmailAddress().WithMessage("Please enter a valid email address.")
                    .MustAsync(async (email, cancellationToken) =>
                    {
                        var exists = await context.Users
                            .AnyAsync(user => user.Email == email);

                        return !exists;
                    }).WithMessage("This email is already in taken.");

                RuleFor(user => user.Password)
                    .NotEmpty().WithMessage("Please enter your password.")
                    .MinimumLength(5).WithMessage("Your password must be at  least 5 characters long")
                    .MaximumLength(125).WithMessage("Your password must not exceced 125 characters long.");

                RuleFor(user => user.ConfirmPassword)
                    .NotEmpty().WithMessage("Please confirm your password.")
                    .Equal(user => user.Password).WithMessage("Passwords does not match");
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IPasswordService _passwordService;

            public CommandHandler(ApplicationDbContext context, IMapper mapper, IPasswordService passwordService)
            {
                _context = context;
                _mapper = mapper;
                _passwordService = passwordService;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var password = _passwordService.Hash(request.Password);

                request.Password = password;

                var user = _mapper.Map<Command, User>(request);

                _context.Users
                    .Add(user);

                await _context.SaveChangesAsync();
            }
        }
    }
}
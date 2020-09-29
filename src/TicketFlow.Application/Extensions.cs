using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using MediatR;
using TicketFlow.Core.Models;
using TicketFlow.Application.Users.Commands;
using TicketFlow.Application.Users.Services;

namespace TicketFlow.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(SignIn.Command).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(SignUp.Command).GetTypeInfo().Assembly);
            return services;
        }

        public static IServiceCollection AddJwt(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var jwtSection = configuration.GetSection("jwt");
            services.Configure<JwtOptions>(jwtSection);
            var jwtOptions = new JwtOptions();
            jwtSection.Bind(jwtOptions);
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });

            services.AddHttpContextAccessor();
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<IPasswordHasher<IPasswordService>, PasswordHasher<IPasswordService>>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<ISignInManager, SignInManager>();

            return services;
        }
    }
}
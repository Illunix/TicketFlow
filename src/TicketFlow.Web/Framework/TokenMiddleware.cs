using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TicketFlow.Application.Users.Services;

namespace TicketFlow.Web.Framework
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenManager _tokenManager;

        public TokenMiddleware(RequestDelegate next, ITokenManager tokenManager)
        {
            _next = next;
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (await _tokenManager.IsCurrentTokenActive())
            {
                await _next(context);
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
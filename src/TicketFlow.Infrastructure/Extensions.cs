using System;  
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketFlow.Infrastructure.Data;

namespace TicketFlow.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services)
        {
            IConfiguration configuration;
            using var serviceProvider = services.BuildServiceProvider();
            configuration = serviceProvider.GetService<IConfiguration>();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}

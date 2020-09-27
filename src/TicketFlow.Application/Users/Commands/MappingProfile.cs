using AutoMapper;
using TicketFlow.Core.Entities;

namespace TicketFlow.Application.Users.Commands
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUp.Command, User>(MemberList.Source);
            CreateMap<SignIn.Command, User>();
        }
    }
}
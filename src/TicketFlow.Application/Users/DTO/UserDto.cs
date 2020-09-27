using TicketFlow.Core.Entities; 

namespace TicketFlow.Application.Users.DTO
{
    public class UserDto
    {
        public string Email { get; set; }

        public UserDto(User user)
        {
            Email = user.Email;
        }
    }
}
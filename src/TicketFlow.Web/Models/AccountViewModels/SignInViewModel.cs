using System.ComponentModel.DataAnnotations;

namespace TicketFlow.Web.Models.AccountViewModels
{
    public class SignInViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string Return_To { get; set; }
    }
}
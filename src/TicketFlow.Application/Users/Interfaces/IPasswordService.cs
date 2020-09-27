namespace TicketFlow.Application.Users.Interfaces
{
    public interface IPasswordService
    {
        bool IsValid(string hash, string password);
        string Hash(string pasword);
    }
}
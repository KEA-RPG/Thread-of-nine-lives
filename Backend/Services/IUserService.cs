using Domain.Entities;

namespace Backend.Services
{
    public interface IUserService
    {
        User GetUserByUsername(string username);
        void CreateUser(User user);

        bool ValidateUserCredentials(string username, string password);
    }
}

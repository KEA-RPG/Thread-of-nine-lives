using Backend.Models;
using Domain.Entities;

namespace Backend.Services
{
    public interface IUserService
    {
        User GetUserByUsername(string username);
        void CreateUser(Credentials credentials);

        bool ValidateUserCredentials(string username, string password);
    }
}

using Backend.Models;
using Domain.DTOs;
using Domain.Entities;

namespace Backend.Services
{
    public interface IUserService
    {
        UserDTO GetUserByUsername(string username);
        void CreateUser(Credentials credentials);
        int GetUserIdByUserName(string username);

        bool ValidateUserCredentials(string username, string password);
    }
}

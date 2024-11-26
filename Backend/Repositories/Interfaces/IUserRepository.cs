using Domain.DTOs;
using Domain.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserDTO GetUserByUsername(string username);
        void CreateUser(UserDTO user);
        UserDTO GetUserById(int id);
    }
}

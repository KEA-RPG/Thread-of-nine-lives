using Domain.DTOs;
using Domain.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
        void CreateUser(User user);
        UserDTO GetUserById(int id);
    }
}

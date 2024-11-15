using Domain.DTOs;
using Domain.Entities;

namespace Backend.Repositories
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
        void CreateUser(User user);
        UserDTO GetUserById(int id);
    }
}

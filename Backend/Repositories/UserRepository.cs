using Domain.Entities;
using Domain.DTOs;
using Infrastructure.Persistance.Relational;

namespace Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RelationalContext _context;

        public UserRepository(RelationalContext context)
        {
            _context = context;
        }

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public UserDTO GetUserById(int id)
        {
            var dbUser = _context.Users.FirstOrDefault(u => u.Id == id);
            var user = UserDTO.FromEntity(dbUser);
            return user;
        }
    }
}


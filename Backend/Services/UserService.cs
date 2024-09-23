using Domain.Entities;
using Backend.Repositories;

namespace Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }

        public void CreateUser(User user)
        {
            // Hash the user's password before saving
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            _userRepository.CreateUser(user);
        }

        // Implement the method to validate user credentials
        public bool ValidateUserCredentials(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);

            // Check if user exists and if the password matches
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return true;
            }

            return false;
        }
    }
}

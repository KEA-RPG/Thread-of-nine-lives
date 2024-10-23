using Domain.Entities;
using Backend.Repositories;
using Backend.Models;

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

        public void CreateUser(Credentials credentials)
        {
            var user = new User
            {
                Username = credentials.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(credentials.Password),
                Role = "Player" // Automatically set to "Player"; admin roles are assigned in the database.
            };

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


            //Fjern callback i tests og find user i repo ud fra username. 
        }
    }
}

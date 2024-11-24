using Domain.Entities;
using Backend.Models;
using Domain.DTOs;
using Backend.Repositories.Interfaces;

namespace Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTO GetUserByUsername(string username)
        {
            var user = _userRepository.GetUserByUsername(username);
            return user;
        }

        public int GetUserIdByUserName(string username)
        {
            return _userRepository.GetUserByUsername(username).Id;
        }

        public void CreateUser(Credentials credentials)
        {
            var user = new UserDTO
            {
                Username = credentials.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(credentials.Password),
                Role = "Player" // Automatically set to "Player"; admin roles are assigned in the database.
            };

            _userRepository.CreateUser(user);
        }

        // Implement the method to validate user credentials
        public bool ValidateUserCredentials(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);

            // Check if user exists and if the password matches
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return true;
            }

            return false;



        }
    }
}

namespace Domain.DTOs
{
    public class UserDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }

        public UserDTO() { }

        // Static method to create UserDTO from a User object
        public static UserDTO FromUser(Domain.Entities.User user)
        {
            return new UserDTO
            {
                Username = user.Username,
                Password = user.PasswordHash,
                Role = user.Role
            };
        }
    }
}

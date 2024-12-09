using Domain.DTOs;
using Domain.Entities.Neo4j;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Neo4J
{
    public class User : Neo4jBase
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Fight> Fights { get; set; }

        public User() { }
        public static User ToEntity(UserDTO user)
        {
            return new User()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                PasswordHash = user.Password //assume its been hashed here
            };
        }
        public static UserDTO FromEntity(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.PasswordHash,
                Role = user.Role
            };
        }


    }
}

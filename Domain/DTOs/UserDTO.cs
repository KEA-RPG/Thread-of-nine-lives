using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }

        public UserDTO() { }

    }
}

﻿using Domain.Entities;
using Domain.DTOs;
using Infrastructure.Persistance.Relational;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories.Graph
{
    public class GraphUserRepository : IUserRepository
    {
        private readonly RelationalContext _context;

        public GraphUserRepository(RelationalContext context)
        {
            _context = context;
        }

        public void CreateUser(UserDTO user)
        {
            var userDb = User.ToEntity(user);
            _context.Users.Add(userDb);
            _context.SaveChanges();
        }

        public void DeleteUser(UserDTO user)
        {
            var userDB = _context.Users.First(u => u.Id == user.Id);
            _context.Users.Remove(userDB);
            _context.SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public UserDTO GetUserByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            return UserDTO.FromEntity(user);
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


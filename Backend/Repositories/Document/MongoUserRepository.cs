using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Persistance.Document;
using MongoDB.Driver;

namespace Backend.Repositories.Document
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly DocumentContext _context;

        public MongoUserRepository(DocumentContext context)
        {
            _context = context;
        }
        public void CreateUser(UserDTO user)
        {
            var id = _context.GetAutoIncrementedId("users");
            user.Id = id;

            _context.Users().InsertOne(user);
        }

        public UserDTO GetUserById(int id)
        {
            return _context.Users().Find(x=> x.Id == id).FirstOrDefault();
        }

        public UserDTO GetUserByUsername(string username)
        {
            return _context.Users().Find(x => x.Username == username).FirstOrDefault();
        }
    }
}

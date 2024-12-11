using Domain.DTOs;
using Infrastructure.Persistance.Relational;
using Backend.Repositories.Interfaces;
using Domain.Entities.Neo4J;
using Infrastructure.Persistance.Graph;

namespace Backend.Repositories.Graph
{
    public class GraphUserRepository : IUserRepository
    {
        private readonly GraphContext _context;

        public GraphUserRepository(GraphContext context)
        {
            _context = context;
        }

        public void CreateUser(UserDTO user)
        {
            var dbUser = User.ToEntity(user);
            dbUser.Id = _context.GetAutoIncrementedId<User>().Result;
            _context.Insert(dbUser).Wait();
        }

        public void DeleteUser(UserDTO user)
        {
            _context.Delete<User>(user.Id).Wait();
        }

        public List<UserDTO> GetAllUsers()
        {
            return _context
                .ExecuteQueryWithMap<User>()
                .Result
                .Select(x => User.FromEntity(x))
                .ToList();
        }

        public UserDTO GetUserByUsername(string username)
        {
            return _context
                .ExecuteQueryWithWhere<User>(x => x.Username == username)
                .Result
                .Select(x => User.FromEntity(x))
                .FirstOrDefault();
        }

        public void UpdateUser(UserDTO user)
        {
            var dbUser = User.ToEntity(user);
            _context.UpdateNode(dbUser).Wait();
        }

        public UserDTO GetUserById(int id)
        {
            return _context
                .ExecuteQueryWithWhere<User>(x => x.Id == id)
                .Result
                .Select(x => User.FromEntity(x))
                .FirstOrDefault();
        }
    }
}


using Domain.Entities;
using Infrastructure.Persistance.Relational;

namespace Backend.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly RelationalContext _context;

        public PlayerRepository(RelationalContext context)
        {
            _context = context;
        }

        public void AddPlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        public void DeletePlayer(Player player)
        {
            _context.Players.Remove(player);
            _context.SaveChanges();
        }

        public List<Player> GetAllPlayers()
        {
            return _context.Players.ToList();
        }

        public Player GetPlayerById(int id)
        {
            return _context.Players.Find(id);
        }

        public void UpdatePlayer(Player player)
        {
            _context.Players.Update(player);
            _context.SaveChanges();
        }
    }
}

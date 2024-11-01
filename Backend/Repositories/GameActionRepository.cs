using Domain.Entities;
using Infrastructure.Persistance.Relational;

namespace Backend.Repositories
{
    public class GameActionRepository : IGameActionRepository
    {
        private readonly RelationalContext _context;

        public GameActionRepository(RelationalContext context)
        {
            _context = context;
        }

        public void AddGameAction(GameAction gameAction)
        {
            _context.GameActions.Add(gameAction);
            _context.SaveChanges();
        }

        public void DeleteGameAction(GameAction gameAction)
        {
            _context.GameActions.Remove(gameAction);
            _context.SaveChanges();
        }

        public List<GameAction> GetAllGameActions()
        {
            return _context.GameActions.ToList();
        }

        public GameAction GetGameActionById(int id)
        {
            return _context.GameActions.Find(id);
        }

        public void UpdateGameAction(GameAction gameAction)
        {
            _context.GameActions.Update(gameAction);
            _context.SaveChanges();
        }
    }
}

using Infrastructure.Persistance.Relational;
using Domain.Entities;


namespace Backend.Repositories
{
    public class CombatRepository : ICombatRepository
    {
        private readonly RelationalContext _context;

        public CombatRepository(RelationalContext context)
        {
            _context = context;
        }

        public Fight GetFightById(int id)
        {
            return _context.Fights.Find(id);
        }

        public void AddFight(Fight fight)
        {
            _context.Fights.Add(fight);
            _context.SaveChanges();
        }

        public void InsertAction(GameAction gameAction)
        {
            _context.GameActions.Add(gameAction);
            _context.SaveChanges();
        }
    }
}

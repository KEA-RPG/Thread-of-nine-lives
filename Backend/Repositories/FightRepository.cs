using Infrastructure.Persistance.Relational;
using Domain.Entities;

namespace Backend.Repositories
{
    public class FightRepository : IFightRepository
    {
        private readonly RelationalContext _context;

        public FightRepository(RelationalContext context)
        {
            _context = context;
        }

        public void AddFight(Fight fight)
        {
            _context.Fights.Add(fight);
            _context.SaveChanges();
        }

        public void DeleteFight(Fight fight)
        {
            _context.Fights.Remove(fight);
            _context.SaveChanges();
        }

        public List<Fight> GetAllFights()
        {
            return _context.Fights.ToList();
        }

        public Fight GetFightById(int id)
        {
            return _context.Fights.Find(id);
        }

        public void UpdateFight(Fight fight)
        {
            _context.Fights.Update(fight);
            _context.SaveChanges();
        }
    }
}

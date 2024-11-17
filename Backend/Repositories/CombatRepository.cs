using Infrastructure.Persistance.Relational;
using Domain.DTOs;
using Domain.Entities;
using System.Data.Entity;

namespace Backend.Repositories
{
    public class CombatRepository : ICombatRepository
    {
        private readonly RelationalContext _context;

        public CombatRepository(RelationalContext context)
        {
            _context = context;
        }

        public FightDTO GetFightById(int id)
        {
            
            var dbFight = _context.Fights.Include(g => g.GameActions).FirstOrDefault(f => f.Id == id);

            var dbGameActions = _context.GameActions.Where(g => g.FightId == dbFight.Id).ToList();
            dbFight.GameActions = dbGameActions;

            var dbEnemy = _context.Enemies.FirstOrDefault(e => e.Id == dbFight.EnemyId);
            dbFight.Enemy = dbEnemy;

            var fight = FightDTO.FromEntity(dbFight);

            return fight;
        }

        public FightDTO AddFight(FightDTO fight)
        {
            var dbFight = Fight.FromDTO(fight);
            _context.Fights.Add(dbFight);
            _context.SaveChanges();

            return GetFightById(dbFight.Id);
        }

        public void InsertAction(GameActionDTO gameAction)
        {
            var dbGameAction = GameAction.FromDTO(gameAction);
            _context.GameActions.Add(dbGameAction);
            _context.SaveChanges();
        }
    }
}

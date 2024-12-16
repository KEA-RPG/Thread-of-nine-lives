using Infrastructure.Persistance.Relational;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories.Relational
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
            var dbFight = _context.Fights
                .Include(x => x.Enemy)
                .Include(x => x.GameActions)
                .FirstOrDefault(x => x.Id == id);

            var fight = Fight.FromEntity(dbFight);

            return fight;
        }

        public FightDTO AddFight(FightDTO fight)
        {
            var dbFight = Fight.ToEntity(fight);
            _context.Fights.Add(dbFight);
            _context.SaveChanges();

            return GetFightById(dbFight.Id);
        }

        public void InsertAction(GameActionDTO gameAction)
        {
            var dbGameAction = GameAction.ToEntity(gameAction);
            _context.GameActions.Add(dbGameAction);
            _context.SaveChanges();
        }
    }
}

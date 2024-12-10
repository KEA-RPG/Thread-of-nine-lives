using Infrastructure.Persistance.Relational;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using Backend.Repositories.Interfaces;
using Infrastructure.Persistance.Graph;
using Domain.Entities.Neo4J;

namespace Backend.Repositories.Graph
{
    public class GraphCombatRepository : ICombatRepository
    {
        private readonly GraphContext _context;

        public GraphCombatRepository(GraphContext context)
        {
            _context = context;
        }

        public FightDTO GetFightById(int id)
        {
            var result = _context.GetClient().Cypher
                .Match("(f:Fight) - [] - (g:GameAction)")
                .Where((Fight f) => f.Id == id)
                .Return((f, g) => new
                {
                    Fight = f,
                    GameActions = g,
                }).ResultsAsync.Result;
            //var data = new FightDTO()
            //{
            //    Enemy = Enemy.FromEntity(result.Enemy),
            //    EnemyId = result.Enemy.Id,
            //    Id = result.Fight.Id,
            //    UserId = result.Fight.UserId,
            //    GameActions = result.GameActions.Select(x => GameAction.FromEntity(x)).ToList()
            //};
            return null;

        }

        public FightDTO AddFight(FightDTO fight)
        {
            var dbFight = Fight.ToEntity(fight);
            dbFight.Id = _context.GetAutoIncrementedId<Fight>().Result;
            _context.Insert(dbFight).Wait();
            return GetFightById(dbFight.Id);
        }

        public void InsertAction(GameActionDTO gameAction)
        {
            var dbGameAction = GameAction.ToEntity(gameAction);
            dbGameAction.Id = _context.GetAutoIncrementedId<Fight>().Result;
            _context.Insert(dbGameAction).Wait();
        }
    }
}

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
                .Match("(f:Fight) -[]- (g:GameAction)")
                .Match("(f) - [] - (e:Enemy)")
                .Where((Fight f) => f.Id == id)
                .Return((f, g, e) => new FightDTO
                {
                    Enemy = e.As<EnemyDTO>(),
                    EnemyId = e.As<EnemyDTO>().Id,
                    Id = id,
                    GameActions = g.CollectAs<GameActionDTO>().ToList()
                }).ResultsAsync.Result.First();

            return result;

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

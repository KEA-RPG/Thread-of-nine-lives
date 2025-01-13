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
                .Match("(f:Fight) - [] - (e:Enemy)")
                .Where((Fight f) => f.Id == id)
                .OptionalMatch("(f:Fight) - [] - (g:GameAction)")
                .Return((f, g, e) => new
                {
                    Fight = f.As<Fight>(),
                    GameActions = g.CollectAs<GameAction>(),
                    Enemy = e.As<Enemy>(),
                }).ResultsAsync.Result.FirstOrDefault();
            
            if(result == null)
            {
                return default(FightDTO);
            }
            var data = new FightDTO()
            {
                Enemy = Enemy.FromEntity(result.Enemy),
                EnemyId = result.Enemy.Id,
                Id = result.Fight.Id,
                UserId = result.Fight.UserId,
                GameActions = result.GameActions.Select(x => GameAction.FromEntity(x)).ToList()
            };
            return data;

        }

        public FightDTO AddFight(FightDTO fight)
        {
            var dbFight = Fight.ToEntity(fight);
            dbFight.Id = _context.GetAutoIncrementedId<Fight>().Result;
            _context.Insert(dbFight).Wait();
            _context.MapNodes<User,Fight>(fight.UserId, dbFight.Id, "FIGHTS_IN").Wait();
            _context.MapNodes<Enemy, Fight>(fight.EnemyId, dbFight.Id, "FIGHTS_IN").Wait();

            return GetFightById(dbFight.Id);
        }

        public void InsertAction(GameActionDTO gameAction)
        {
            var dbGameAction = GameAction.ToEntity(gameAction);
            dbGameAction.Id = _context.GetAutoIncrementedId<GameAction>().Result;
            _context.Insert(dbGameAction).Wait();
            _context.MapNodes<GameAction, Fight>(dbGameAction.Id, gameAction.FightId, "PART_OF").Wait();
        }
    }
}

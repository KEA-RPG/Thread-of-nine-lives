using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Persistance.Document;
using MongoDB.Driver;

namespace Backend.Repositories.Document
{
    public class MongoCombatRepository : ICombatRepository
    {
        private readonly DocumentContext _context;

        public MongoCombatRepository(DocumentContext context)
        {
            _context = context;
        }
        public FightDTO AddFight(FightDTO fight)
        {

            var id = _context.GetAutoIncrementedId("fights");
            fight.Id = id;

            _context.Fights().InsertOne(fight);
            return GetFightById(id);
        }

        public FightDTO GetFightById(int id)
        {
            return _context.Fights().Find(x => x.Id == id).FirstOrDefault();
        }

        public void InsertAction(GameActionDTO gameAction)
        {
            var fight =  _context.Fights().Find(x => x.Id == gameAction.FightId).FirstOrDefault();
            fight.GameActions.Add(gameAction);
            var update = Builders<FightDTO>.Update.Push(x => x.GameActions, gameAction);
            var filter = Builders<FightDTO>.Filter.Eq(c => c.Id, gameAction.FightId);
            _context.Fights().UpdateOne(filter, update);
        }
    }
}

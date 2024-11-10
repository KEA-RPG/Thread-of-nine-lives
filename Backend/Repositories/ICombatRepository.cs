using Domain.Entities;

namespace Backend.Repositories
{
    public interface ICombatRepository
    {
        public Fight GetFightById(int id);
        public void AddFight(Fight fight);
        public void InsertAction(GameAction gameAction);
    }
}

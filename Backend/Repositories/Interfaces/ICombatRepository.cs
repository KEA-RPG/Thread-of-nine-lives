using Domain.DTOs;

namespace Backend.Repositories.Interfaces
{
    public interface ICombatRepository
    {
        public FightDTO GetFightById(int id);
        public FightDTO AddFight(FightDTO fight);
        public void InsertAction(GameActionDTO gameAction);
    }
}

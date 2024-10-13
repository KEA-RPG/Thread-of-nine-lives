using Domain.Entities;

namespace Backend.Repositories
{
    public interface IFightRepository
    {
        public void AddFight(Fight fight);
        public void DeleteFight(Fight fight);
        public void UpdateFight(Fight fight);
        public List<Fight> GetAllFights();
        public Fight GetFightById(int id);

    }
}

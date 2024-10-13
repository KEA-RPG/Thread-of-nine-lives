using Domain.Entities;

namespace Backend.Services
{
    public interface IFightService
    {
        Fight GetFightById(int id);
        List<Fight> GetAllFights();
        Fight CreateFight(Fight fight);
        Fight UpdateFight(Fight fight);
        void DeleteFight(int id);
    }
}
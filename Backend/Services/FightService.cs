using Domain.Entities;
using Backend.Repositories;

namespace Backend.Services
{
    public class FightService : IFightService
    {
        private readonly IFightRepository _fightRepository;

        public FightService(IFightRepository fightRepository)
        {
            _fightRepository = fightRepository;
        }

        public Fight CreateFight(Fight fight)
        {
            _fightRepository.AddFight(fight);
            return fight;
        }

        public void DeleteFight(int id)
        {
            var fight = _fightRepository.GetFightById(id);
            _fightRepository.DeleteFight(fight);
        }

        public List<Fight> GetAllFights()
        {
            return _fightRepository.GetAllFights();
        }

        public Fight GetFightById(int id)
        {
            return _fightRepository.GetFightById(id);
        }

        public Fight UpdateFight(Fight fight)
        {
            _fightRepository.UpdateFight(fight);
            return _fightRepository.GetFightById(fight.Id);
        }
    }
}

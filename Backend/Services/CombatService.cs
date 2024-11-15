using Backend.Models;
using Domain.DTOs;
using Backend.Repositories;
using Backend.Helpers;

namespace Backend.Services
{
    public class CombatService : ICombatService
    {
        private readonly ICombatRepository _combatRepository;

        private readonly IEnemyRepository _enemyRepository;

        private readonly IUserRepository _userRepository;

        private readonly StateFactory stateFactory = new StateFactory();

        public CombatService(ICombatRepository combatRepository)
        {
            _combatRepository = combatRepository;
        }

        public State GetInitState(StateGameInit stateGameInit)
        {
            var enemy  = _enemyRepository.GetEnemyById(stateGameInit.EnemyId);
            var user = _userRepository.GetUserById(stateGameInit.UserId);

            if (enemy == null || user == null)
            {
                return null;
            }

            FightDTO fight = new FightDTO
            {
                UserId = stateGameInit.UserId,
                Enemy = EnemyDTO.FromEntity(enemy)
            };

            var addedFight = _combatRepository.AddFight(fight);

            var state = stateFactory.CreateInitState();

            return state;
        }

        public State ProcessAction(GameActionDTO gameAction, State state)
        {
            _combatRepository.InsertAction(gameAction);
            return stateFactory.ProcessAction(gameAction, state);
        }

    }

}

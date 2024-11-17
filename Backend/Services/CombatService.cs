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

        public CombatService(ICombatRepository combatRepository, IEnemyRepository enemyRepository, IUserRepository userRepository)
        {
            _combatRepository = combatRepository;
            _enemyRepository = enemyRepository;
            _userRepository = userRepository;
        }

        public State GetInitState(StateGameInit stateGameInit)
        {
            if (stateGameInit == null || stateGameInit.EnemyId == null || stateGameInit.UserId == null)
            {
                throw new ArgumentNullException("stateGameInit, EnemyId, or UserId is null");
            }

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

            var state = stateFactory.CreateInitState(addedFight);

            return state;
        }

        public State ProcessAction(GameActionDTO gameAction)
        {
            if (gameAction != null)
            {
                _combatRepository.InsertAction(gameAction);
            }

            var fight = _combatRepository.GetFightById(gameAction.FightId);
            var state = stateFactory.CreateInitState(fight);

            foreach (var action in state.GameActions)
            {
                state = stateFactory.ProcessAction(state);
            }

            return state;
        }

    }

}

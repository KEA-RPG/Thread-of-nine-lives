using Backend.Models;
using Domain.DTOs;
using Backend.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Backend.Repositories.Interfaces;

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

        public State GetInitialState(StateGameInit stateGameInit)
        {
            if (stateGameInit == null)
            {
                throw new ArgumentNullException(nameof(stateGameInit), "StateGameInit cannot be null.");
            }

            var enemy = _enemyRepository.GetEnemyById(stateGameInit.EnemyId);
            var user = _userRepository.GetUserById(stateGameInit.UserId);

            if (enemy == null)
            {
                throw new KeyNotFoundException($"Enemy with ID {stateGameInit.EnemyId} not found.");
            }

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {stateGameInit.UserId} not found.");
            }

            var fight = new FightDTO
            {
                UserId = stateGameInit.UserId,
                EnemyId = stateGameInit.EnemyId
            };

            var addedFight = _combatRepository.AddFight(fight);
            var state = stateFactory.CreateState(addedFight);

            return state;

        }

        public State GetProcessedState(int fightID, GameActionDTO action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            var fight = _combatRepository.GetFightById(fightID);
            if (fight == null)
            {
                throw new KeyNotFoundException($"Fight with ID {fightID} not found.");
            }

            _combatRepository.InsertAction(action);

            //getting refreshed fight
            fight = _combatRepository.GetFightById(fightID);
            var state = stateFactory.CreateState(fight);
            return state;
        }

    }

}

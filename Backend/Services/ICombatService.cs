using Backend.Models;
using Domain.DTOs;
using Domain.Entities;

namespace Backend.Services
{
    public interface ICombatService
    {
        State GetInitialState(StateGameInit stateGameInit);
        State GetProcessedState(int fightId, GameActionDTO action);
        State GetStateByFightId(int fightId);
    }
}

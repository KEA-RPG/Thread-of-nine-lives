using Backend.Models;
using Domain.DTOs;
using Domain.Entities;

namespace Backend.Services
{
    public interface ICombatService
    {
        State GetInitState(StateGameInit stateGameInit);
        State ProcessAction(GameActionDTO gameAction, State state);
    }
}

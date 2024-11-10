using Domain.Entities;

namespace Backend.Services
{
    public interface ICombatService
    {
        void ProcessAction(GameAction gameAction);
        void UpdateGameState(GameAction gameAction);
    }
}

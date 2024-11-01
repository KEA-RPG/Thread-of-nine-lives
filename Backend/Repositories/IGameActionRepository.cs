using Domain.Entities;

namespace Backend.Repositories
{
    public interface IGameActionRepository
    {
        public void AddGameAction(GameAction gameAction);
        public void DeleteGameAction(GameAction gameAction);
        public void UpdateGameAction(GameAction gameAction);
        public List<GameAction> GetAllGameActions();
        public GameAction GetGameActionById(int id);
    }
}

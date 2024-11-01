using Domain.DTOs;

namespace Backend.Services
{
    public interface IGameActionService
    {
        GameActionDTO GetGameActionById(int id);
        List<GameActionDTO> GetAllGameActions();
        GameActionDTO CreateGameAction(GameActionDTO gameActionDTO);
        GameActionDTO UpdateGameAction(GameActionDTO gameActionDTO);
        void DeleteGameAction(int id);
    }
}

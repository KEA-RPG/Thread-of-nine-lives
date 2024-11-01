using Domain.Entities;
using Domain.DTOs;
using Backend.Repositories;

namespace Backend.Services
{
    public class GameActionService : IGameActionService
    {
        private readonly IGameActionRepository _gameActionRepository;

        public GameActionService(IGameActionRepository gameActionRepository)
        {
            _gameActionRepository = gameActionRepository;
        }

        public GameActionDTO GetGameActionById(int id)
        {
            var gameAction = _gameActionRepository.GetGameActionById(id);
            if (gameAction != null)
            {
                return GameActionDTO.FromEntity(gameAction);
            }
            else
            {
                return null;
            }
        }

        public List<GameActionDTO> GetAllGameActions()
        {
            var gameActions = _gameActionRepository.GetAllGameActions();
            return gameActions.Select(GameActionDTO.FromEntity).ToList();
        }

        public GameActionDTO CreateGameAction(GameActionDTO gameActionDTO)
        {
            var gameAction = GameAction.FromDTO(gameActionDTO);
            _gameActionRepository.AddGameAction(gameAction);

            // Update the DTO with the generated Id from the entity
            gameActionDTO.Id = gameAction.Id;

            return gameActionDTO;
        }

        public GameActionDTO UpdateGameAction(GameActionDTO gameActionDTO)
        {
            var existingGameAction = _gameActionRepository.GetGameActionById(gameActionDTO.Id);
            if (existingGameAction == null)
            {
                return null;
            }

            // Update properties
            existingGameAction.Type = gameActionDTO.Type;

            _gameActionRepository.UpdateGameAction(existingGameAction);

            return GameActionDTO.FromEntity(existingGameAction);
        }

        public void DeleteGameAction(int id)
        {
            var gameAction = _gameActionRepository.GetGameActionById(id);
            if (gameAction != null)
            {
                _gameActionRepository.DeleteGameAction(gameAction);
            }
        }
    }
}

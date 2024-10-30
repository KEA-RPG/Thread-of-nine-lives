using Domain.Entities;
using Domain.DTOs;
using Backend.Repositories;

namespace Backend.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public PlayerDTO GetPlayerById(int id)
        {
            var player = _playerRepository.GetPlayerById(id);
            if (player != null)
            {
                return PlayerDTO.FromEntity(player);
            }
            else
            {
                return null;
            }
        }

        public List<PlayerDTO> GetAllPlayers()
        {
            var players = _playerRepository.GetAllPlayers();
            return players.Select(PlayerDTO.FromEntity).ToList();
        }

        public PlayerDTO CreatePlayer(PlayerDTO playerDTO)
        {
            var player = Player.FromDTO(playerDTO);
            _playerRepository.AddPlayer(player);

            // Update the DTO with the generated Id from the entity
            playerDTO.Id = player.Id;

            return playerDTO;
        }

        public PlayerDTO UpdatePlayer(PlayerDTO playerDTO)
        {
            var existingPlayer = _playerRepository.GetPlayerById(playerDTO.Id);
            if (existingPlayer == null)
            {
                return null;
            }

            // Update properties
            existingPlayer.Health = playerDTO.Health;

            _playerRepository.UpdatePlayer(existingPlayer);

            return PlayerDTO.FromEntity(existingPlayer);
        }

        public void DeletePlayer(int id)
        {
            var player = _playerRepository.GetPlayerById(id);
            if (player != null)
            {
                _playerRepository.DeletePlayer(player);
            }
        }
    }
}

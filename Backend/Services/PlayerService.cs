using Domain.Entities;
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

        public Player CreatePlayer(Player player)
        {
            _playerRepository.CreatePlayer(player);
            return player;
        }

        public void DeletePlayer(int id)
        {
            var player = _playerRepository.GetPlayerById(id);
            _playerRepository.DeletePlayer(player);
        }

        public List<Player> GetAllPlayers()
        {
            return _playerRepository.GetAllPlayers();
        }

        public Player GetPlayerById(int id)
        {
            return _playerRepository.GetPlayerById(id);
        }

        public Player UpdatePlayer(Player player)
        {
            _playerRepository.UpdatePlayer(player);
            return _playerRepository.GetPlayerById(player.Id);
        }
    }
}

using Backend.Services;
using Domain.Entities;

namespace Backend.Controllers
{
    public class PlayerController
    {
        private IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public Player GetPlayerById(int id)
        {
            return _playerService.GetPlayerById(id);
        }

        public List<Player> GetAllPlayers()
        {
            return _playerService.GetAllPlayers();
        }

        public Player CreatePlayer(Player player)
        {
            return _playerService.CreatePlayer(player);
        }

        public Player UpdatePlayer(Player player)
        {
            return _playerService.UpdatePlayer(player);
        }

        public void DeletePlayer(int id)
        {
            _playerService.DeletePlayer(id);
        }
    }
}

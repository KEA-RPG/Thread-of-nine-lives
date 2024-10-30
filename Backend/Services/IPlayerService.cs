using Domain.Entities;

namespace Backend.Services
{
    public interface IPlayerService
    {
        Player GetPlayerById(int id);
        List<Player> GetAllPlayers();
        Player CreatePlayer(Player player);
        Player UpdatePlayer(Player player);
        void DeletePlayer(int id);
    }
}

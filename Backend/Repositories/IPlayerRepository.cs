using Domain.Entities;

namespace Backend.Repositories
{
    public interface IPlayerRepository
    {
        public void AddPlayer(Player player);
        public void DeletePlayer(Player player);
        public void UpdatePlayer(Player player);
        public List<Player> GetAllPlayers();
        public Player GetPlayerById(int id);

    }
}

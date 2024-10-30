using Domain.DTOs;

namespace Backend.Services
{
    public interface IPlayerService
    {
        PlayerDTO GetPlayerById(int id);
        List<PlayerDTO> GetAllPlayers();
        PlayerDTO CreatePlayer(PlayerDTO playerDTO);
        PlayerDTO UpdatePlayer(PlayerDTO playerDTO);
        void DeletePlayer(int id);
    }
}

using Domain.DTOs;

namespace Backend.Services
{
    public interface IDeckService
    {
        DeckDTO GetDeckById(int id);
        List<DeckDTO> GetUserDecks(string userName);
        DeckDTO CreateDeck(DeckDTO deck);
        DeckDTO UpdateDeck(DeckDTO deck);
        List<DeckDTO> GetPublicDecks();
        void DeleteDeck(int id);
    }
}

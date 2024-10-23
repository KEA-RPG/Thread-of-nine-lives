using Domain.DTOs;

namespace Backend.Services
{
    public interface IDeckService
    {
        DeckDTO GetDeckById(int id);
        List<DeckDTO> GetUserDecks();
        DeckDTO CreateDeck(DeckDTO deck);
        DeckDTO UpdateDeck(DeckDTO deck);

        void DeleteDeck(int id);
    }
}

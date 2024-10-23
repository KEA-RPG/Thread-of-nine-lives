using Domain.DTOs;

namespace Backend.Repositories
{
    public interface IDeckRepository
    {
        DeckDTO AddDeck(DeckDTO deck);
        void DeleteDeck(DeckDTO deck);
        List<DeckDTO> GetUserDecks();
        DeckDTO GetDeckById(int id);
        void UpdateDeck(DeckDTO deck);
    }
}

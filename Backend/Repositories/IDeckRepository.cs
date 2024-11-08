using Domain.DTOs;
using Domain.Entities;

namespace Backend.Repositories
{
    public interface IDeckRepository
    {
        DeckDTO AddDeck(DeckDTO deck);
        void DeleteDeck(int deckId);
        List<DeckDTO> GetUserDecks(string userName);
        List<DeckDTO> GetPublicDecks();
        DeckDTO GetDeckById(int id);
        void UpdateDeck(DeckDTO deck);
    }
}

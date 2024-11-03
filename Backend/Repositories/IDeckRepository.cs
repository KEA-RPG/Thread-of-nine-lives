using Domain.DTOs;
using Domain.Entities;

namespace Backend.Repositories
{
    public interface IDeckRepository
    {
        DeckDTO AddDeck(Deck deck);
        void DeleteDeck(Deck deck);
        List<DeckDTO> GetUserDecks(string userName);
        List<DeckDTO> GetPublicDecks();
        Deck GetDeckById(int id, string? UserName = null);
        void UpdateDeck(Deck deck);
    }
}

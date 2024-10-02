using Domain.Entities;

namespace Backend.Repositories
{
    public interface IDeckRepository
    {
        void AddDeck(Deck deck);
        void DeleteDeck(Deck deck);
        List<Deck> GetUserDecks();
        Deck GetDeckById(int id);
        void UpdateDeck(Deck deck);
    }
}

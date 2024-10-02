using Domain.Entities;

namespace Backend.Services
{
    public interface IDeckService
    {
        Deck GetDeckById(int id);
        List<Deck> GetUserDecks();
        Deck CreateDeck(Deck deck);
        Deck UpdateDeck(Deck deck);

        void DeleteDeck(int id);
    }
}

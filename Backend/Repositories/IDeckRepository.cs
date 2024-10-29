using Domain.DTOs;

namespace Backend.Repositories
{
    public interface IDeckRepository
    {
        DeckDTO AddDeck(DeckDTO deck);
        IResult DeleteDeck(DeckDTO deck);
        List<DeckDTO> GetUserDecks();
        DeckDTO GetDeckById(int id);
        IResult UpdateDeck(DeckDTO deck);
    }
}

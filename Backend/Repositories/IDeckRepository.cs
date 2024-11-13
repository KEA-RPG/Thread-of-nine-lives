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

        void AddComment(Comment comment);
        List<Comment> GetCommentsByDeckId(int deckId);
        DeckDTO GetDeckById(int id);

        void UpdateDeck(DeckDTO deck);
    }
}

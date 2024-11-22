using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Persistance.Document;
using MongoDB.Driver;

namespace Backend.Repositories.Document
{
    public class MongoDeckRepository : IDeckRepository
    {
        private readonly DocumentContext _context;

        public MongoDeckRepository(DocumentContext context)
        {
            _context = context;
        }

        public void AddComment(CommentDTO comment)
        {
            var deck = _context.Decks().Find(x => x.Id == comment.DeckId).FirstOrDefault();
            deck.Comments.Add(comment);
            UpdateDeck(deck);
        }

        public DeckDTO AddDeck(DeckDTO deck)
        {
            var id = _context.GetAutoIncrementedId("decks");
            deck.Id = id;

            _context.Decks().InsertOne(deck);
            return GetDeckById(id);

        }

        public void DeleteDeck(int deckId)
        {
            _context.Decks().DeleteOne(x => x.Id == deckId);
        }

        public List<CommentDTO> GetCommentsByDeckId(int deckId)
        {
            var deck = GetDeckById(deckId);
            return deck.Comments;
        }

        public DeckDTO GetDeckById(int id)
        {
            return _context.Decks().Find(x => x.Id == id).FirstOrDefault();
        }

        public List<DeckDTO> GetPublicDecks()
        {
            return _context.Decks().Find(x => x.IsPublic).ToList();
        }

        public List<DeckDTO> GetUserDecks(string userName)
        {
            return _context.Decks().Find(x => x.UserName == userName).ToList();

        }

        public void UpdateDeck(DeckDTO deck)
        {
            var dbDeck = GetDeckById(deck.Id);

            //Comments arent sent on request so we ensure that the comments arent removed
            deck.Comments = dbDeck.Comments;

            var filter = Builders<DeckDTO>.Filter.Eq(c => c.Id, deck.Id);
            var update = Builders<DeckDTO>.Update
                .Set(c => c, deck);

            _context.Decks().UpdateOne(filter, update);
        }
    }
}

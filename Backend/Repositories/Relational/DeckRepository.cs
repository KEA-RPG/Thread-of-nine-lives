﻿using Domain.Entities;
using Infrastructure.Persistance.Relational;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using Backend.Repositories.Interfaces;
using Backend.SecurityLogic;


namespace Backend.Repositories.Relational
{
    //Recieves DTO looks for Entities
    //Sends DTO's back
    public class DeckRepository : IDeckRepository
    {
        private readonly RelationalContext _context;

        public DeckRepository(RelationalContext context)
        {
            _context = context;
        }

        public DeckDTO AddDeck(DeckDTO deck)
        {
            var dbDeck = Deck.ToEntity(deck);
            dbDeck.Comments = new List<Comment>(); //TODO: ensuring that comments are not pre-set in the deck create
            _context.Decks.Add(dbDeck);
            _context.SaveChanges();

            return GetDeckById(dbDeck.Id);
        }

        public void DeleteDeck(int deckId)
        {
            var dbDeck = _context.Decks.Find(deckId);
            if (dbDeck != null)
            {
                var deleteDeckEntry = new DeletedDeck()
                {
                    DeckId = dbDeck.Id,
                    DeleteTime = DateTime.UtcNow,
                };
                _context.DeletedDecks.Add(deleteDeckEntry);
                _context.SaveChanges();
            }
        }

        public DeckDTO GetDeckById(int id)
        {
            var dbDeck = _context.Decks
                .Include(deck => deck.DeckCards)
                .ThenInclude(deckCard => deckCard.Card)
                .Include(deck => deck.User)
                .Include(deck => deck.DeletedDeck)
                .FirstOrDefault(deck => deck.Id == id && deck.DeletedDeck == null);
            
            if (dbDeck == null)
            {
                return default(DeckDTO);
            }
            var deck = Deck.FromEntity(dbDeck);

            return deck;
        }

        public void UpdateDeck(DeckDTO deckToUpdate)
        {
            var dbDeck = _context.Decks.Find(deckToUpdate.Id);

            if (dbDeck != null)
            {
                // Map the properties from the DTO to the entity
                dbDeck.Name = deckToUpdate.Name;
                dbDeck.Id = deckToUpdate.Id;
                dbDeck.DeckCards = deckToUpdate.Cards.Select(card => new DeckCard
                {
                    CardId = card.Id,
                    DeckId = deckToUpdate.Id
                }).ToList();
                dbDeck.IsPublic = deckToUpdate.IsPublic;


                _context.Decks.Update(dbDeck);
                _context.SaveChanges();
            }
        }

        public List<DeckDTO> GetPublicDecks()
        {
            return _context.Decks
                .Include(deck => deck.DeckCards)
                .ThenInclude(deckCard => deckCard.Card)
                .Include(deck => deck.User)
                .Include(deck => deck.Comments)
                .ThenInclude(comment => comment.User)
                .Where(deck => deck.IsPublic && deck.DeletedDeck == null)
                .Select(deck => Deck.FromEntity(deck)).ToList();
        }

        public List<DeckDTO> GetUserDecks(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);

            if (user == null)
            {
                return new List<DeckDTO>();
            }

            return _context.Decks
                .Include(x => x.User)
                .Include(x => x.DeckCards)
                .ThenInclude(x => x.Card)
                .Where(deck => deck.User == user && deck.DeletedDeck == null)
                .Select(deck => Deck.FromEntity(deck)).ToList();
        }
        public void AddComment(CommentDTO comment)
        {
            var commentDB =Comment.ToEntity(comment);
            _context.Comments.Add(commentDB);
            _context.SaveChanges();
        }

        public List<CommentDTO> GetCommentsByDeckId(int deckId)
        {
            return _context.Comments
                .Include(x=> x.Deck)
                .ThenInclude(x=> x.DeletedDeck)
                .Where(comment => comment.DeckId == deckId && comment.Deck.DeletedDeck == null)
                .Select(x=> Comment.FromEntity(x))
                .ToList();
        }

    }
}

﻿using Domain.Entities;
using Infrastructure.Persistance.Relational;
using Domain.DTOs;

namespace Backend.Repositories
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

        public DeckDTO AddDeck(DeckDTO deckDto)
        {
            var deck = new Deck
            {
                Name = deckDto.Name,
                UserId = deckDto.UserId,
                DeckCards = new List<DeckCard>()
            };

            foreach (var card in deckDto.Cards)
            {
                var deckCard = new DeckCard
                {
                    CardId = card.Id,
                    Deck = deck
                };
                deck.DeckCards.Add(deckCard);
            }

            _context.Decks.Add(deck);
            _context.SaveChanges();

            return GetDeckById(deck.Id);
        }

        public void DeleteDeck(DeckDTO deckDto)
        {
            var deck = _context.Decks.Find(deckDto.Id);
            if (deck != null)
            {
                var deckCards = _context.DeckCards.Where(dc => dc.DeckId == deck.Id);
                foreach (var deckCard in deckCards)
                {
                    _context.DeckCards.Remove(deckCard);
                }
                _context.Decks.Remove(deck);
                _context.SaveChanges();
            }
        }

        public DeckDTO GetDeckById(int id)
        {
            var deck = _context.Decks.Find(id);
            if (deck == null) return null;

            return new DeckDTO
            {
                // Map properties from deck to deckDto
            };
        }

        public void UpdateDeck(DeckDTO deckDto)
        {
            var deck = _context.Decks.Find(deckDto.Id);
            if (deck != null)
            {
                // Map properties from deckDto to deck
                _context.Decks.Update(deck);
                _context.SaveChanges();
            }
        }

        public List<DeckDTO> GetUserDecks()
        {
            return _context.Decks.Select(deck => new DeckDTO
            {
                // Map properties from deck to deckDto
            }).ToList();
        }
    }
}

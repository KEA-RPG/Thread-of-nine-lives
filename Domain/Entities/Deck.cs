using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Deck
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<DeckCard> DeckCards { get; set; }
        public bool IsPublic { get; set; }
        public User User { get; set; }

        // New field to store comments
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public static Deck FromDTO(DeckDTO deckDto)
        {
            var deck = new Deck
            {
                Id = deckDto.Id,
                UserId = deckDto.UserId,
                Name = deckDto.Name,
                IsPublic = deckDto.IsPublic,
                Comments = deckDto.Comments.Select(commentDto => CommentDTO.ToEntity(commentDto)).ToList()
            };
            deck.DeckCards = deckDto.Cards.Select(card => new DeckCard
            {
                CardId = card.Id,
                Deck = deck
            }).ToList();

            return deck;
        }
    }
}

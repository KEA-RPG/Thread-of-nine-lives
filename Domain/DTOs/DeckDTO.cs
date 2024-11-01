using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DeckDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<CardDTO> Cards { get; set; }
        public bool IsPublic { get; set; }


        public static DeckDTO FromEntity(Deck deck)
        {
            return new DeckDTO
            {
                Id = deck.Id,
                UserId = deck.UserId,
                Name = deck.Name,
                Cards = deck.DeckCards.Select(dc => CardDTO.FromEntity(dc.Card)).ToList(),
                IsPublic = deck.IsPublic
            };
        }



    }
}

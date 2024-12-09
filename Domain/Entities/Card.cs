using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Card
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Attack { get; set; }

        public int Defence { get; set; }

        public int Cost { get; set; }

        public string ImagePath { get; set; }

        public List<DeckCard> DeckCards { get; set; }


        public Card()
        {
        }

        public static Card ToEntity(CardDTO cardDTO)
        {
            return new Card
            {
                Id = cardDTO.Id,
                Name = cardDTO.Name,
                Description = cardDTO.Description,
                Attack = cardDTO.Attack,
                Defence = cardDTO.Defence,
                Cost = cardDTO.Cost,
                ImagePath = cardDTO.ImagePath
            };
        }
        public static CardDTO FromEntity(Card card)
        {
            return new CardDTO
            {
                Id = card.Id,
                Name = card.Name,
                Description = card.Description,
                Attack = card.Attack,
                Defence = card.Defence,
                Cost = card.Cost,
                ImagePath = card.ImagePath
            };
        }


    }
}

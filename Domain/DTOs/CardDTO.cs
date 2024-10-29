using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CardDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int Attack { get; set; }
        public required int Defense { get; set; }
        public required int Cost { get; set; }
        public required string ImagePath { get; set; }

        public CardDTO() { }

        //Borrowed from EnemyDTO.cs
        public static CardDTO FromEntity(Card card)
        {
            return new CardDTO
            {
                Id = card.Id,
                Name = card.Name,
                Description = card.Description,
                Attack = card.Attack,
                Defense = card.Defense,
                Cost = card.Cost,
                ImagePath = card.ImagePath
            };
        }
    }
}

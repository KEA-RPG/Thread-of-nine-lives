﻿using Domain.DTOs;
using Domain.Entities.Neo4j;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Neo4J
{
    public class Deck : Neo4jBase
    {
        public override int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<Card> Cards { get; set; }
        public bool IsPublic { get; set; }
        public List<User> Users { get; set; } //there should be only one
        public List<Comment> Comments { get; set; }
        public static Deck ToEntity(DeckDTO deckDto)
        {
            //assuming that we only need the ID of the card for insert, we dont need to map a whole Card entity for the insert.
            //If used somewhere else this need to be redone
            var deck = new Deck
            {
                Id = deckDto.Id,
                UserId = deckDto.UserId,
                Name = deckDto.Name,
                IsPublic = deckDto.IsPublic,
            };
            deck.Cards = deckDto.Cards.Select(card => new Card
            {
                Id = card.Id,
            }).ToList();

            return deck;
        }

        public static DeckDTO FromEntity(Deck deck)
        {
            return new DeckDTO
            {
                Id = deck.Id,
                UserId = deck.UserId,
                UserName = deck.Users?.First().Username,
                Name = deck.Name,
                Cards = deck.Cards?.Select(dc => Card.FromEntity(dc)).ToList(),
                IsPublic = deck.IsPublic,
                Comments = deck.Comments?.Select(comment => Comment.FromEntity(comment)).ToList()
            };
        }

    }
}

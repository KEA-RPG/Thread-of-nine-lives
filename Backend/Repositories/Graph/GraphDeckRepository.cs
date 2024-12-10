using Infrastructure.Persistance.Relational;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using Backend.Repositories.Interfaces;
using Infrastructure.Persistance.Graph;
using Domain.Entities.Neo4J;
using Neo4jClient.Transactions;
using System;

namespace Backend.Repositories.Graph
{
    //Recieves DTO looks for Entities
    //Sends DTO's back
    public class GraphDeckRepository : IDeckRepository
    {
        private readonly GraphContext _context;

        public GraphDeckRepository(GraphContext context)
        {
            _context = context;
        }

        public DeckDTO AddDeck(DeckDTO deck)
        {
            var dbDeck = Deck.ToEntity(deck);
            var cards = deck.Cards;
            dbDeck.Id = _context.GetAutoIncrementedId<Deck>().Result;
            dbDeck.Comments = null;
            dbDeck.Cards = null;
            _context.Insert(dbDeck).Wait();

            foreach (var card in cards)
            {
                _context.MapNodes<Deck, Card>(dbDeck.Id, card.Id, "CONTAINS").Wait();
            }
            _context.MapNodes<User, Deck>(deck.UserId, dbDeck.Id, "OWNS").Wait();

            return GetDeckById(dbDeck.Id);
        }

        public void DeleteDeck(int deckId)
        {
            var deck = GetDeckById(deckId);
            if (deck != null)
            {
                _context.Delete<Deck>(deckId).Wait();
            }
        }

        public DeckDTO GetDeckById(int id)
        {
            var result = _context.GetClient().Cypher
                .Match("(d:Deck) - [] - (c:Card)")
                .Match("(d:Deck) - [] - (u:User)")
                .Where((Deck d) => d.Id == id)
                .Return((d, c, u) => new
                {
                    Deck = d.As<Deck>(),
                    Cards = c.CollectAs<Card>(),
                    User = u.As<User>(),
                }).ResultsAsync.Result.First();
            return  new DeckDTO()
            {
                UserName = result.User.Username,
                Id = result.Deck.Id,
                UserId = result.Deck.UserId,
                Cards = result.Cards.Select(c => Card.FromEntity(c)).ToList(),
                IsPublic = result.Deck.IsPublic,
                Name = result.Deck.Name,
            };

        }

        public void UpdateDeck(DeckDTO deckToUpdate)
        {
            var dbDeck = GetDeckById(deckToUpdate.Id);

            if (dbDeck != null)
            {
                var client = _context.GetClient();
                var cards = deckToUpdate.Cards;
                deckToUpdate.Comments = null;
                deckToUpdate.Cards = null;
                using (ITransaction tx = client.BeginTransaction())
                {
                    //removing the old connections
                    client.Cypher
                        .Match("(x:Deck)")
                        .Where((Deck x) => x.Id == deckToUpdate.Id)
                        .Set("x = $y")
                        .WithParam("y", deckToUpdate)
                        .ExecuteWithoutResultsAsync().Wait();

                    client.Cypher
                        .Match("(d:Deck)-[r]-(c:Card)")
                        .Where((Deck d) => d.Id == deckToUpdate.Id)
                        .Delete("r").ExecuteWithoutResultsAsync().Wait();

                    foreach (var card in cards)
                    {
                        _context.MapNodes<Deck, Card>(dbDeck.Id, card.Id, "CONTAINS").Wait();

                    }
                    tx.CommitAsync().Wait();
                }

            }

        }

        public List<DeckDTO> GetPublicDecks()
        {
            return _context
                .ExecuteQueryWithWhere<Deck>(x => x.IsPublic).Result
                .Select(x => Deck.FromEntity(x))
                .ToList();
        }

        public List<DeckDTO> GetUserDecks(string userName)
        {
            var user = _context
                .ExecuteQueryWithMap<User>()
                .Result
                .Select(x => x)
                .FirstOrDefault();

            if (user == null)
            {
                return null;
            }


            var result = _context.GetClient().Cypher
                .Match("(d:Deck) - [] - (c:Card)",
                "(d:Deck) - [] - (u:User)")
                .Where((User u) => u.Username == userName)
                .Return((d, c, u) => new
                {
                    Deck = d.As<Deck>(),
                    Cards = c.CollectAs<Card>(),

                }).ResultsAsync.Result;
            return result.Select(x => new DeckDTO()
            {
                UserName = userName,
                Id = x.Deck.Id,
                UserId = x.Deck.UserId,
                Cards = x.Cards.Select(c => Card.FromEntity(c)).ToList(),
                IsPublic = x.Deck.IsPublic,
                Name = x.Deck.Name,
            }).ToList();

        }
        public void AddComment(CommentDTO comment)
        {
            var dbComment = Comment.ToEntity(comment);
            dbComment.Id = _context.GetAutoIncrementedId<Comment>().Result;
            _context.Insert(dbComment).Wait();
            _context.MapNodes<Comment, Deck>(comment.Id, comment.DeckId, "IS_IN").Wait();
        }

        public List<CommentDTO> GetCommentsByDeckId(int deckId)
        {
            return _context
                .ExecuteQueryWithWhere<Comment>(x => x.DeckId == deckId).Result
                .Select(x => Comment.FromEntity(x))
                .ToList();
        }

    }
}

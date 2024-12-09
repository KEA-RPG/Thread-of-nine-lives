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
    public class DeckRepository : IDeckRepository
    {
        private readonly GraphContext _context;

        public DeckRepository(GraphContext context)
        {
            _context = context;
        }

        public DeckDTO AddDeck(DeckDTO deck)
        {
            var dbDeck = Deck.ToEntity(deck);
            dbDeck.Id = _context.GetAutoIncrementedId<Deck>().Result;
            dbDeck.Comments = new List<Comment> { };
            _context.Insert(dbDeck).Wait();

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
            return _context
                .ExecuteQueryWithMap<Deck>()
                .Result
                .Select(x => Deck.FromEntity(x))
                .FirstOrDefault();
        }

        public void UpdateDeck(DeckDTO deckToUpdate)
        {
            var dbDeck = GetDeckById(deckToUpdate.Id);

            if (dbDeck != null)
            {
                var client = _context.GetClient();
                using (ITransaction tx = client.BeginTransaction())
                {
                    //removing the old connections
                    client.Cypher
                        .Match("(x:Deck)")
                        .Where((Deck x) => x.Id == deckToUpdate.Id)
                        .Set("x = {y}")
                        .WithParam("y", deckToUpdate)
                        .ExecuteWithoutResultsAsync().Wait();

                    client.Cypher
                        .Match("(d:Deck)-[r]-(c:Card)")
                        .Where((Deck d) => d.Id == deckToUpdate.Id)
                        .Delete("r").ExecuteWithoutResultsAsync().Wait();

                    foreach (var card in deckToUpdate.Cards)
                    {
                        //attaching new connections
                        client.Cypher
                            .Match("(x:deck)", "(y:Card)")
                            .Where((Deck x) => x.Id == deckToUpdate.Id)
                            .AndWhere((Card y) => y.Id == card.Id)
                            .Create($"(x)-[:CONTAINS]->(y)")
                            .ExecuteWithoutResultsAsync().Wait();
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

            return _context
                .ExecuteQueryWithWhere<Deck>(x => x.UserId == user.Id).Result
                .Select(x => Deck.FromEntity(x))
                .ToList();
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

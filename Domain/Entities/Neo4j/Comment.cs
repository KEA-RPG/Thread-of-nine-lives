using Domain.Entities.Neo4j;
using System;

namespace Domain.Entities.Neo4J
{
    public class Comment : Neo4jBase
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public Deck Deck { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } 
        public int UserId { get; set; } 
        public User User { get; set; } 
    }
}
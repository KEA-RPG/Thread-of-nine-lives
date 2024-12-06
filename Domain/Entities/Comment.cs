using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public Deck Deck { get; set; }

        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } 
        public int UserId { get; set; } 
        public User User { get; set; } 
    }
}
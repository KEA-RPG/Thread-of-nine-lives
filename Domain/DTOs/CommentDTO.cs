using Domain.Entities;
using System;

namespace Domain.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Username { get; set; }


    }
}

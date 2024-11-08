using Domain.Entities;
using System;

namespace Domain.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public static CommentDTO FromEntity(Comment comment)
        {
            return new CommentDTO
            {
                Id = comment.Id,
                DeckId = comment.DeckId,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt
            };
        }

        public static Comment ToEntity(CommentDTO commentDto)
        {
            return new Comment
            {
                Id = commentDto.Id,
                DeckId = commentDto.DeckId,
                Text = commentDto.Text,
                CreatedAt = commentDto.CreatedAt
            };
        }
    }
}
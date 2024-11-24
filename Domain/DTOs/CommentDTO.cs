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

        public string Username { get; set; } //I service klassen vil denne blive sat på ud til frontend, så vi har username. På vej mod databasen skal sercice
        //klassen finde username of populere UserId så vi kan korrekt mappe til en Comment. 

        public static CommentDTO FromEntity(Comment comment)
        {
            return new CommentDTO
            {
                Id = comment.Id,
                DeckId = comment.DeckId,
                UserId = comment.UserId,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                Username = comment.User.Username
            };
        }

        public static Comment ToEntity(CommentDTO commentDto)
        {
            return new Comment
            {
                Id = commentDto.Id,
                DeckId = commentDto.DeckId,
                UserId = commentDto.UserId,
                Text = commentDto.Text,
                CreatedAt = DateTime.Now
            };
        }
    }
}

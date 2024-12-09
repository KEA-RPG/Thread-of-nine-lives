using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.DTOs
{
    public class DeckDTO
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<CardDTO> Cards { get; set; }
        public string UserName { get; set; }
        public bool IsPublic { get; set; }
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();

    }
}
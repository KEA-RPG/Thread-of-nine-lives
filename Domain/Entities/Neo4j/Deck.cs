using Domain.DTOs;
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
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<Card> Cards { get; set; }
        public bool IsPublic { get; set; }
        public User Users { get; set; }
        public List<Comment> Comments { get; set; }
    }
}

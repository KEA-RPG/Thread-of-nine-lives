using Domain.DTOs;
using Domain.Entities.Neo4j;

namespace Domain.Entities.Neo4J
{
    public class Enemy : Neo4jBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public string ImagePath { get; set; }
        public List<Fight> Fights { get; set; }

        public Enemy() { }

    }
}

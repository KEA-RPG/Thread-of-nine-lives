using Domain.DTOs;

namespace Domain.Entities.Neo4J
{
    public class Enemy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public string ImagePath { get; set; }
        public List<Fight> Fights { get; set; }

        public Enemy() { }

    }
}

﻿using Domain.DTOs;

namespace Domain.Entities.Neo4J
{
    public class Fight
    {
        public int Id { get; set; }
        public int EnemyId { get; set; }
        public Enemy Enemy { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<GameAction> GameActions { get; set; } = new List<GameAction>();
    }
}

using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CardDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int Attack { get; set; }
        public required int Defence { get; set; }
        public required int Cost { get; set; }
        public required string ImagePath { get; set; }

        public CardDTO() { }
    }
}

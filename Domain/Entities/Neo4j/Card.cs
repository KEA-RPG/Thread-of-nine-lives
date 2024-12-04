using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Neo4J
{
    public class Card
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Attack { get; set; }

        public int Defence { get; set; }

        public int Cost { get; set; }

        public string ImagePath { get; set; }

        public List<Deck> Deck{ get; set; }


        public Card()
        {
        }

    }
}

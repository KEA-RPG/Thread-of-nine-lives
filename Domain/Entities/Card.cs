﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Card
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int Cost { get; set; }

        public string ImagePath { get; set; }

        public List<DeckCard> DeckCards { get; set; }


        public Card()
        {
        }

    }
}

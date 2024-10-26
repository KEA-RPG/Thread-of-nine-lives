﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Deck
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Name { get; set; }
        public List<DeckCard> DeckCards { get; set; }

        public User User { get; set; }
    }
}

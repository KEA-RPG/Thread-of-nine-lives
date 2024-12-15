using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DeletedDeck
    {
        public int Id { get; set; }
        public int DeckId { get; set;}
        public DateTime DeleteTime { get; set; }
    }
}

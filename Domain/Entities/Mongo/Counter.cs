using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Mongo
{
    public class Counter
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Identifier {  get; set; }
        public int Count { get; set; }
    }
}

using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
namespace Infrastructure.Persistance.Document
{
    public class DatabaseSnapshotHelper
    {
        private readonly DocumentContext _context;
        private Dictionary<string, List<BsonDocument>> _snapshot;

        public DatabaseSnapshotHelper(DocumentContext database)
        {
            _context = database;
            _snapshot = new Dictionary<string, List<BsonDocument>>();
        }

        public void TakeSnapshot()
        {
            _snapshot.Clear();
            var collections = _context.GetAllCollections().ToList();
            foreach ( var collectionName in collections)
            {
                var collection = _context.GetCollection<BsonDocument>(collectionName);
                var documents = collection.Find(FilterDefinition<BsonDocument>.Empty).ToList();
                _snapshot[collectionName] = documents;
            }
        }

        // Restore the database to the state of the snapshot
        public void RestoreSnapshot()
        {
            foreach (var (collectionName, documents) in _snapshot)
            {
                var collection = _context.GetCollection<BsonDocument>(collectionName);
                collection.DeleteMany(FilterDefinition<BsonDocument>.Empty);
                if (documents.Any())
                {
                    collection.InsertMany(documents);
                }
            }
        }
    }
}

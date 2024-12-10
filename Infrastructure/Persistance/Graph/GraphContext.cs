using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver;
using System.Xml.Linq;
using Domain.Entities.Neo4J;
using Neo4j.Driver.Mapping;
using Neo4jClient;
using Neo4jClient.Transactions;
using System.Net.Sockets;
using Domain.Entities.Neo4j;
using MongoDB.Driver;
using Neo4jClient.Cypher;
using System.Linq.Expressions;
namespace Infrastructure.Persistance.Graph
{
    public class GraphContext
    {
        private readonly IDriver _driver;
        private readonly QueryConfig _dbConfig;
        private IAuthToken _auth;
        private BoltGraphClient _client;

        public GraphContext(string connectionString, string user, string password, string databaseName)
        {
            _auth = AuthTokens.Basic(user, password);
            _driver = GraphDatabase.Driver(connectionString, _auth);
            _dbConfig = new QueryConfig(database: databaseName);
            _client = new BoltGraphClient(connectionString, user, password);
            _client.ConnectAsync().Wait();

        }
        public async Task<IEnumerable<T>> ExecuteQueryWithMap<T>()
        {
            var type = typeof(T).Name;

            var query = await _client.Cypher
            .Match($"(x:{type})")
            .Return(x => x.As<T>())
            .ResultsAsync;
            return query;
        }

        public async Task<IEnumerable<T1>> ExecuteQueryWithMapAndRelations<T1,T2>()
        {
            var type = typeof(T1).Name;

            var query = await _client.Cypher
            .Match($"(x:{type})")
            .Return(x => x.As<T1>())
            .ResultsAsync;
            return query;
        }
        public async Task<T> ExecuteQueryWithMapSingle<T>(int id)
            where T : Neo4jBase
        {
            var type = typeof(T).Name;

            var query = await _client.Cypher
            .Match($"(x:{type})")
            .Where((T x) => x.Id == id)
            .Return(x => x.As<T>())
            .ResultsAsync;
            return query.FirstOrDefault();
        }

        public async Task Insert<T>(T entity)
        {
            var type = typeof(T).Name;

            var cyper = _client.Cypher
                .Create($"(x:{type} $newItem)")
                .WithParam("newItem", entity)
                .ExecuteWithoutResultsAsync();
            await cyper;
        }

        public async Task Delete<T>(int id)
            where T : Neo4jBase
        {
            var type = typeof(T).Name;
            await _client.Cypher
                .Match($"(x:{type})")
                .Where((T x) => x.Id == id)
                .DetachDelete("x")
                .ExecuteWithoutResultsAsync();
        }
        public async Task<Counter> InitCounter<T>(int startVal = 0)
        {
            var typeName = typeof(T).Name;
            var query = await _client.Cypher
                .Match($"(x:Counter)")
                .Where((Counter x) => x.Name == typeName)
                .Return(x => x.As<Counter>()).ResultsAsync;

            var node = query.FirstOrDefault();
            if (node == null)
            {
                node = new Counter()
                {
                    Count = startVal,
                    Name = typeName
                };
                await Insert(node);
            }
            return node;
        }
        public async Task<int> GetAutoIncrementedId<T>()
        {
            var typeName = typeof(T).Name;
            var node = await InitCounter<T>();

            node.Count++;
            await _client.Cypher
                .Match("(x:Counter)")
                .Where((Counter x) => x.Name == typeName)
                .Set("x.Count = $Count")
                .WithParam("Count", node.Count)
                .ExecuteWithoutResultsAsync();
            return node.Count;
        }

        public async Task InsertManyNodes<T>(IEnumerable<T> list)
        {
            var type = typeof(T).Name;
            using (ITransaction tx = _client.BeginTransaction())
            {
                foreach (var item in list)
                {

                    await _client.Cypher
                        .Create($"(x:{type} $newItem)")
                        .WithParam("newItem", item)
                        .ExecuteWithoutResultsAsync();
                }
                await tx.CommitAsync();
            }
        }
        public async Task MapNodes<TFrom,TTo>(int fromID, int toId, string relationType)
            where TFrom : Neo4jBase 
            where TTo : Neo4jBase
        {
            var typeFrom = typeof(TFrom).Name;
            var typeTo = typeof(TTo).Name;

            await _client.Cypher
                .Match($"(x:{typeFrom})", $"(y:{typeTo})")
                .Where((TFrom x) => x.Id == fromID)
                .AndWhere((TTo y) => y.Id == toId)
                .Create($"(x)-[:{relationType}]->(y)")
                .ExecuteWithoutResultsAsync();
        }
        public async Task UpdateNode<T>(T node)
            where T : Neo4jBase
        {
            var type = typeof(T).Name;
            await _client.Cypher
                .Match($"(x:{type})")
                .Where((T x) => x.Id == node.Id)
                .Set("x = $y")
                .WithParam("y", node)
                .ExecuteWithoutResultsAsync();


        }
        public async Task<IEnumerable<T>> ExecuteQueryWithWhere<T>(Expression<Func<T, bool>> whereClause)
        {
            var type = typeof(T).Name;

            var query = await _client.Cypher
            .Match($"(x:{type})")
            .Where(whereClause)
            .Return(x => x.As<T>())
            .ResultsAsync;
            return query;
        }
        public BoltGraphClient GetClient()
        {
            return _client;
        }
    }
}
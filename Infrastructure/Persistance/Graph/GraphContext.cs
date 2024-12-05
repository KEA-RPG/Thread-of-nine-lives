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

        public async Task Insert<T>(T entity, string labelName)
        {
            await _client.Cypher
                .Create($"(x:{labelName} {{newItem}})")
                .WithParam("newItem", entity)
                .ExecuteWithoutResultsAsync();
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
    }
}
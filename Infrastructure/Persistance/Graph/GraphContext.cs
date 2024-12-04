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
        public async Task<IEnumerable<Person>> ExecuteQueryWithMap()
        {
            var query = await _client.Cypher
            .Match("(person:Person)")
            .Return(person => person.As<Person>())
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

        public async Task InsertMany<T>(IEnumerable<T> list, string labelName)
        {
            using (ITransaction tx = _client.BeginTransaction())
            {
                foreach (var item in list)
                {

                    await _client.Cypher
                        .Create("(x:Card $newItem)")
                        .WithParam("newItem", item)
                        .ExecuteWithoutResultsAsync();
                }
                await tx.CommitAsync();
            }
        }

        public async Task<IReadOnlyList<Person>> ExecuteQueryWithMap2(string query)
        {
            using (var session = _driver.AsyncSession())
            {
                var result = await _driver
                .ExecutableQuery(query)
                .ExecuteAsync();

                var result2 = await _driver
                .ExecutableQuery(query)
                .ExecuteAsync()
                .AsObjectsAsync<Person>();



                return result2;
            }
        }

    }
    public class Person
    {
        public string? name { get; set; }
        public string? twitter { get; set; } = "";
    }
}
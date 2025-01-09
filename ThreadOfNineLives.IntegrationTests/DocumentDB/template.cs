using Backend.Repositories.Document;
using Domain.DTOs;
using Infrastructure.Persistance.Document;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadOfNineLives.IntegrationTests.DocumentDB
{
    public class tt : IDisposable
    {
        private readonly DocumentContext _context;
        private readonly MongoCombatRepository _mongoCombatRepository;
        private readonly DatabaseSnapshotHelper _snapshotHelper;
        public tt()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("dbsettings.json")
                .Build();

            var settings = configuration.GetSection("ConnectionStrings:MongoDB");
            var connectionString = settings.GetSection("Connectionstring").Value;
            var databaseName = settings.GetSection("DatabaseName").Value;

            _context = new DocumentContext(connectionString, databaseName);
            _mongoCombatRepository = new MongoCombatRepository(_context);
            _snapshotHelper = new DatabaseSnapshotHelper(_context);

            _snapshotHelper.TakeSnapshot();
        }

        public void Dispose()
        {
            _snapshotHelper.RestoreSnapshot();
        }
    }
}

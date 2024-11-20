using Moq;
using System.Data;
using Infrastructure.Persistance.Relational;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using MongoDB.Driver.Core.Configuration;
using Infrastructure.Persistance;

namespace ThreadOfNineLives.IntegrationTests
{
    public class AuditLogTriggerTests
    {
        //private readonly string _connectionString;
        //// Create the RelationalContext instance using the options
        //private readonly RelationalContext _db;

        //public AuditLogTriggerTests()
        //{
        //    _connectionString = PersistanceConfiguration.GetConnectionString(dbtype.Relational);

        //    var optionsBuilder = new DbContextOptionsBuilder<RelationalContext>();
        //    optionsBuilder.UseSqlServer(_connectionString,
        //        b => b.MigrationsAssembly("Infrastructure"));       
        //    _db = new RelationalContext(optionsBuilder.Options);
        //}

        [Fact]
        public async Task Insert_ShouldLogToAuditLog()
        {
            // Arrange: Define the SQL to insert a new enemy
            string insertCommand = @"
                INSERT INTO [KeaRpg].[dbo].[Enemies] ([Name], [Health], [ImagePath])
                VALUES ('Cat', 100, 'images/cat.png');";

            string selectAuditLogQuery = @"
                SELECT TOP 1 * 
                FROM [KeaRpg].[dbo].[AuditLog] 
                WHERE TableName = 'Enemies' 
                  AND OperationType = 'INSERT' 
                ORDER BY ChangeDateTime DESC;";

            // Act: Use Entity Framework to execute the insert command
            await _db.Database.ExecuteSqlRawAsync(insertCommand);

            // Wait for trigger to execute (optional, depending on SQL Server's async behavior)
            await Task.Delay(1000);  // Consider adjusting delay time based on your trigger's execution time

            // Query the AuditLog to ensure the insert operation was logged
            var auditLogEntry = await _db.AuditLogs
                                          .Where(x => x.TableName == "Enemies" && x.OperationType == "INSERT")
                                          .OrderByDescending(x => x.ChangeDateTime)
                                          .FirstOrDefaultAsync();

            // Assert: Verify the AuditLog contains the expected values
            Assert.NotNull(auditLogEntry);
            Assert.Equal("Enemies", auditLogEntry.TableName);
            Assert.Equal("INSERT", auditLogEntry.OperationType);
            Assert.Contains("Cat", auditLogEntry.NewValues);  // Ensure 'Cat' is part of the new values

        }
    }
}

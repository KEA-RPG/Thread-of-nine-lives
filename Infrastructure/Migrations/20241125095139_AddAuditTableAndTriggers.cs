using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditTableAndTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryKeyValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            // ============================================
            // Trigger for Cards table
            // ============================================

            // Trigger for INSERT operation on the Cards table
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_Audit_Cards_Insert
                ON [KeaRpg].[dbo].[Cards]
                AFTER INSERT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @PrimaryKeyValue VARCHAR(100);
                    DECLARE @NewValues NVARCHAR(MAX);

                    -- Capture the primary key value
                    SELECT @PrimaryKeyValue = CONVERT(VARCHAR, i.Id)
                    FROM INSERTED i;

                    -- Collect new values as JSON
                    SELECT @NewValues = (
                        SELECT Id, Name, Description, Attack, Defence, Cost, ImagePath
                        FROM INSERTED
                        FOR JSON AUTO
                    );

                    -- Insert the audit record
                    INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                    VALUES ('Cards', 'INSERT', @PrimaryKeyValue, NULL, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for UPDATE operation on the Cards table
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_Audit_Cards_Update
                ON [KeaRpg].[dbo].[Cards]
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @PrimaryKeyValue VARCHAR(100);
                    DECLARE @OldValues NVARCHAR(MAX);
                    DECLARE @NewValues NVARCHAR(MAX);

                    -- Capture the primary key value
                    SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                    FROM DELETED d;

                    -- Collect old values as JSON
                    SELECT @OldValues = (
                        SELECT Id, Name, Description, Attack, Defence, Cost, ImagePath
                        FROM DELETED
                        FOR JSON AUTO
                    );

                    -- Collect new values as JSON
                    SELECT @NewValues = (
                        SELECT Id, Name, Description, Attack, Defence, Cost, ImagePath
                        FROM INSERTED
                        FOR JSON AUTO
                    );

                    -- Insert the audit record
                    INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                    VALUES ('Cards', 'UPDATE', @PrimaryKeyValue, @OldValues, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for DELETE operation on the Cards table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Cards_Delete
            ON [KeaRpg].[dbo].[Cards]
            AFTER DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, Name, Description, Attack, Defence, Cost, ImagePath
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
	            VALUES ('Cards', 'DELETE', @PrimaryKeyValue, @OldValues, NULL, GETDATE(), SYSTEM_USER);
                END;
            ");

            // ============================================
            // Trigger for Comments table
            // ============================================

            // Trigger for INSERT operation on the Comments table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Comments_Insert
            ON [KeaRpg].[dbo].[Comments]
            AFTER INSERT
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, i.Id)
                FROM INSERTED i;

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, DeckId, Text, CreatedAt, UserId
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('Comments', 'INSERT', @PrimaryKeyValue, NULL, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for UPDATE operation on the Comments table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Comments_Update
            ON [KeaRpg].[dbo].[Comments]
            AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, DeckId, Text, CreatedAt, UserId
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, DeckId, Text, CreatedAt, UserId
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('Comments', 'UPDATE', @PrimaryKeyValue, @OldValues, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for DELETE operation on the Comments table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Comments_Delete
            ON [KeaRpg].[dbo].[Comments]
            AFTER DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, DeckId, Text, CreatedAt, UserId
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
	            VALUES ('Comments', 'DELETE', @PrimaryKeyValue, @OldValues, NULL, GETDATE(), SYSTEM_USER);
                END;
            ");

            // ============================================
            // Trigger for DeckCards table
            // ============================================

            // Trigger for INSERT operation on the DeckCards table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_DeckCards_Insert
            ON [KeaRpg].[dbo].[DeckCards]
            AFTER INSERT
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key values (DeckId, CardId)
                SELECT @PrimaryKeyValue = CONCAT(i.DeckId, '-', i.CardId)
                FROM INSERTED i;

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT DeckId, CardId
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('DeckCards', 'INSERT', @PrimaryKeyValue, NULL, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for UPDATE operation on the DeckCards table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_DeckCards_Update
            ON [KeaRpg].[dbo].[DeckCards]
            AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key values (DeckId, CardId)
                SELECT @PrimaryKeyValue = CONCAT(d.DeckId, '-', d.CardId)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT DeckId, CardId
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT DeckId, CardId
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('DeckCards', 'UPDATE', @PrimaryKeyValue, @OldValues, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for DELETE operation on the DeckCards table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_DeckCards_Delete
            ON [KeaRpg].[dbo].[DeckCards]
            AFTER DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);

                -- Capture the primary key values (DeckId, CardId)
                SELECT @PrimaryKeyValue = CONCAT(d.DeckId, '-', d.CardId)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT DeckId, CardId
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
	            VALUES ('DeckCards', 'DELETE', @PrimaryKeyValue, @OldValues, NULL, GETDATE(), SYSTEM_USER);
                END;
            ");

            // ============================================
            // Trigger for Decks table
            // ============================================

            // Trigger for INSERT operation on the Decks table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Decks_Insert
            ON [KeaRpg].[dbo].[Decks]
            AFTER INSERT
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, i.Id)
                FROM INSERTED i;

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, UserId, Name, IsPublic
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('Decks', 'INSERT', @PrimaryKeyValue, NULL, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for UPDATE operation on the Decks table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Decks_Update
            ON [KeaRpg].[dbo].[Decks]
            AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, UserId, Name, IsPublic
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, UserId, Name, IsPublic
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('Decks', 'UPDATE', @PrimaryKeyValue, @OldValues, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for DELETE operation on the Decks table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Decks_Delete
            ON [KeaRpg].[dbo].[Decks]
            AFTER DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, UserId, Name, IsPublic
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
	            VALUES ('Decks', 'DELETE', @PrimaryKeyValue, @OldValues, NULL, GETDATE(), SYSTEM_USER);
                END;
            ");

            // ============================================
            // Trigger for Enemies table
            // ============================================

            // Trigger for INSERT operation on the Enemies table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Enemies_Insert
            ON [KeaRpg].[dbo].[Enemies]
            AFTER INSERT
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, i.Id)
                FROM INSERTED i;

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, Name, Health, ImagePath
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('Enemies', 'INSERT', @PrimaryKeyValue, NULL, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for UPDATE operation on the Enemies table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Enemies_Update
            ON [KeaRpg].[dbo].[Enemies]
            AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, Name, Health, ImagePath
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, Name, Health, ImagePath
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('Enemies', 'UPDATE', @PrimaryKeyValue, @OldValues, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for DELETE operation on the Enemies table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Enemies_Delete
            ON [KeaRpg].[dbo].[Enemies]
            AFTER DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, Name, Health, ImagePath
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
	            VALUES ('Enemies', 'DELETE', @PrimaryKeyValue, @OldValues, NULL, GETDATE(), SYSTEM_USER);
                END;
            ");

            // ============================================
            // Trigger for Fights table
            // ============================================

            // Trigger for INSERT operation on the Fights table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Fights_Insert
            ON [KeaRpg].[dbo].[Fights]
            AFTER INSERT
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, i.Id)
                FROM INSERTED i;

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, EnemyId, UserId
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('Fights', 'INSERT', @PrimaryKeyValue, NULL, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for UPDATE operation on the Fights table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Fights_Update
            ON [KeaRpg].[dbo].[Fights]
            AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, EnemyId, UserId
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, EnemyId, UserId
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('Fights', 'UPDATE', @PrimaryKeyValue, @OldValues, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for DELETE operation on the Fights table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Fights_Delete
            ON [KeaRpg].[dbo].[Fights]
            AFTER DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, EnemyId, UserId
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
	            VALUES ('Fights', 'DELETE', @PrimaryKeyValue, @OldValues, NULL, GETDATE(), SYSTEM_USER);
                END;
            ");

            // ============================================
            // Trigger for GameActions table
            // ============================================

            // Trigger for INSERT operation on the GameActions table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_GameActions_Insert
            ON [KeaRpg].[dbo].[GameActions]
            AFTER INSERT
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, i.Id)
                FROM INSERTED i;

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, FightId, Type, Value
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('GameActions', 'INSERT', @PrimaryKeyValue, NULL, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for UPDATE operation on the GameActions table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_GameActions_Update
            ON [KeaRpg].[dbo].[GameActions]
            AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, FightId, Type, Value
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, FightId, Type, Value
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('GameActions', 'UPDATE', @PrimaryKeyValue, @OldValues, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for DELETE operation on the GameActions table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_GameActions_Delete
            ON [KeaRpg].[dbo].[GameActions]
            AFTER DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, FightId, Type, Value
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
	            VALUES ('GameActions', 'DELETE', @PrimaryKeyValue, @OldValues, NULL, GETDATE(), SYSTEM_USER);
                END;
            ");

            // ============================================
            // Trigger for Users table
            // ============================================

            // Trigger for INSERT operation on the Users table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Users_Insert
            ON [KeaRpg].[dbo].[Users]
            AFTER INSERT
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, i.Id)
                FROM INSERTED i;

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, Username, PasswordHash, Role
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('Users', 'INSERT', @PrimaryKeyValue, NULL, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for UPDATE operation on the Users table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Users_Update
            ON [KeaRpg].[dbo].[Users]
            AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);
                DECLARE @NewValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, Username, PasswordHash, Role
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Collect new values as JSON
                SELECT @NewValues = (
                    SELECT Id, Username, PasswordHash, Role
                    FROM INSERTED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
                VALUES ('Users', 'UPDATE', @PrimaryKeyValue, @OldValues, @NewValues, GETDATE(), SYSTEM_USER);
                END;
            ");

            // Trigger for DELETE operation on the Users table
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_Audit_Users_Delete
            ON [KeaRpg].[dbo].[Users]
            AFTER DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @PrimaryKeyValue VARCHAR(100);
                DECLARE @OldValues NVARCHAR(MAX);

                -- Capture the primary key value
                SELECT @PrimaryKeyValue = CONVERT(VARCHAR, d.Id)
                FROM DELETED d;

                -- Collect old values as JSON
                SELECT @OldValues = (
                    SELECT Id, Username, PasswordHash, Role
                    FROM DELETED
                    FOR JSON AUTO
                );

                -- Insert the audit record
                INSERT INTO AuditLogs (TableName, OperationType, PrimaryKeyValue, OldValues, NewValues, ChangeDateTime, ChangedBy)
	            VALUES ('Users', 'DELETE', @PrimaryKeyValue, @OldValues, NULL, GETDATE(), SYSTEM_USER);
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Cards_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Cards_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Cards_Delete;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Comments_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Comments_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Comments_Delete;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_DeckCards_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_DeckCards_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_DeckCards_Delete;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Decks_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Decks_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Decks_Delete;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Enemies_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Enemies_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Enemies_Delete;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Fights_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Fights_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Fights_Delete;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_GameActions_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_GameActions_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_GameActions_Delete;");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Users_Insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Users_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Audit_Users_Delete;");
        }
    }
}

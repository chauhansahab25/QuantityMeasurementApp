using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementModelLayer.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementRepositoryLayer.Repositories
{
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly string connectionString;

        public QuantityMeasurementDatabaseRepository(IConfiguration configuration)
        {
            // Reads the connection string from appsettings.json
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                throw new InvalidOperationException("DefaultConnection not found in configuration");
                
            // Ensure proper IDENTITY configuration on initialization
            try
            {
                ResetIdentity();
            }
            catch
            {
            }
        }

        public bool TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                
                // Actually test the connection by running a simple query
                using var command = new SqlCommand("SELECT 1", connection);
                command.CommandTimeout = 5; // 5 second timeout for the command
                var result = command.ExecuteScalar();
                
                return result != null;
            }
            catch
            {
                return false;
            }
        }

        public void ResetIdentity()
        {
            using var connection = GetConnection();
            connection.Open();

            // Get the maximum ID currently in the table
            string getMaxIdQuery = "SELECT ISNULL(MAX(Id), 0) FROM QuantityMeasurements";
            using var getMaxIdCommand = new SqlCommand(getMaxIdQuery, connection);
            int maxId = (int)getMaxIdCommand.ExecuteScalar();

            // Reset identity to the next proper value
            string resetQuery = $"DBCC CHECKIDENT ('QuantityMeasurements', RESEED, {maxId})";
            using var resetCommand = new SqlCommand(resetQuery, connection);
            resetCommand.ExecuteNonQuery();

            Console.WriteLine($"🔧 IDENTITY reset to continue from ID {maxId + 1}");
        }

        private SqlConnection GetConnection() => new SqlConnection(connectionString);

        public void Save(QuantityMeasurementEntity entity)
        {
            using var connection = GetConnection();
            connection.Open();

            // Ensure table exists
            EnsureTableExists(connection);

            string query =
                @"INSERT INTO QuantityMeasurements
                (FirstValue, FirstUnit, SecondValue, SecondUnit, Operation, Result, MeasurementType)
                VALUES
                (@FirstValue, @FirstUnit, @SecondValue, @SecondUnit, @Operation, @Result, @MeasurementType)";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstValue", entity.FirstValue);
            command.Parameters.AddWithValue("@FirstUnit", entity.FirstUnit ?? string.Empty);
            command.Parameters.AddWithValue("@SecondValue", entity.SecondValue);
            command.Parameters.AddWithValue("@SecondUnit", entity.SecondUnit ?? string.Empty);
            command.Parameters.AddWithValue("@Operation", entity.Operation ?? string.Empty);
            command.Parameters.AddWithValue("@Result", entity.Result);
            command.Parameters.AddWithValue("@MeasurementType", entity.MeasurementType ?? string.Empty);

            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine($"✓ Operation saved to database. Rows affected: {rowsAffected}");
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            var list = new List<QuantityMeasurementEntity>();

            using var connection = GetConnection();
            connection.Open();

            // Ensure table exists
            EnsureTableExists(connection);

            string query = "SELECT * FROM QuantityMeasurements ORDER BY Id ASC";

            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(ReadEntity(reader));
            }

            Console.WriteLine($"Retrieved {list.Count} records from database");
            return list;
        }

        public List<QuantityMeasurementEntity> GetByOperation(string operation)
        {
            var list = new List<QuantityMeasurementEntity>();

            using var connection = GetConnection();
            connection.Open();

            string query = "SELECT * FROM QuantityMeasurements WHERE Operation=@Operation ORDER BY Id ASC";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Operation", operation ?? string.Empty);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(ReadEntity(reader));
            }

            return list;
        }

        public List<QuantityMeasurementEntity> GetByMeasurementType(string type)
        {
            var list = new List<QuantityMeasurementEntity>();

            using var connection = GetConnection();
            connection.Open();

            string query = "SELECT * FROM QuantityMeasurements WHERE MeasurementType=@Type ORDER BY Id ASC";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Type", type ?? string.Empty);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(ReadEntity(reader));
            }

            return list;
        }

        public int GetTotalCount()
        {
            using var connection = GetConnection();
            connection.Open();

            string query = "SELECT COUNT(*) FROM QuantityMeasurements";
            using var command = new SqlCommand(query, connection);

            return (int)command.ExecuteScalar();
        }

        public bool OperationExists(double firstValue, string firstUnit, double secondValue, string secondUnit, string operation)
        {
            using var connection = GetConnection();
            connection.Open();

            EnsureTableExists(connection);

            string query = @"SELECT COUNT(*) FROM QuantityMeasurements 
                           WHERE FirstValue = @FirstValue 
                           AND FirstUnit = @FirstUnit 
                           AND SecondValue = @SecondValue 
                           AND SecondUnit = @SecondUnit 
                           AND Operation = @Operation";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstValue", firstValue);
            command.Parameters.AddWithValue("@FirstUnit", firstUnit ?? string.Empty);
            command.Parameters.AddWithValue("@SecondValue", secondValue);
            command.Parameters.AddWithValue("@SecondUnit", secondUnit ?? string.Empty);
            command.Parameters.AddWithValue("@Operation", operation ?? string.Empty);

            int count = (int)command.ExecuteScalar();
            return count > 0;
        }

        public QuantityMeasurementEntity GetLastSavedOperation()
        {
            using var connection = GetConnection();
            connection.Open();

            EnsureTableExists(connection);

            string query = "SELECT TOP 1 * FROM QuantityMeasurements ORDER BY Id DESC";

            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return ReadEntity(reader);
            }

            return new QuantityMeasurementEntity();
        }

        public void DeleteAll()
        {
            using var connection = GetConnection();
            connection.Open();

            string query = "DELETE FROM QuantityMeasurements";
            using var command = new SqlCommand(query, connection);

            command.ExecuteNonQuery();
        }
        private void EnsureTableExists(SqlConnection connection)
        {
            // Check if table exists
            string checkTableExistsQuery = @"
                SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_NAME = 'QuantityMeasurements'";

            using var checkTableCommand = new SqlCommand(checkTableExistsQuery, connection);
            int tableExists = (int)checkTableCommand.ExecuteScalar();

            if (tableExists == 0)
            {
                // Create table with correct schema
                string createTableQuery = @"
                    CREATE TABLE QuantityMeasurements (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        FirstValue FLOAT NOT NULL,
                        FirstUnit NVARCHAR(50) NOT NULL,
                        SecondValue FLOAT NOT NULL,
                        SecondUnit NVARCHAR(50) NOT NULL,
                        Operation NVARCHAR(50) NOT NULL,
                        Result FLOAT NOT NULL,
                        MeasurementType NVARCHAR(50) NOT NULL,
                        CreatedAt DATETIME2 DEFAULT GETDATE()
                    )";

                using var command = new SqlCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                Console.WriteLine("Database table created with correct schema.");
                return;
            }

            // Check if table has correct schema
            string checkSchemaQuery = @"
                SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = 'QuantityMeasurements' 
                AND COLUMN_NAME IN ('FirstValue', 'FirstUnit', 'SecondValue', 'SecondUnit', 'Operation', 'Result', 'MeasurementType')";

            using var checkSchemaCommand = new SqlCommand(checkSchemaQuery, connection);
            int columnCount = (int)checkSchemaCommand.ExecuteScalar();

            // If table doesn't have all required columns, recreate it
            if (columnCount < 7)
            {
                Console.WriteLine("Table schema mismatch detected. Recreating table...");
                
                // Drop table if it exists with wrong schema
                string dropTableQuery = "DROP TABLE QuantityMeasurements";
                using (var dropCommand = new SqlCommand(dropTableQuery, connection))
                {
                    dropCommand.ExecuteNonQuery();
                }

                // Create table with correct schema
                string createTableQuery = @"
                    CREATE TABLE QuantityMeasurements (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        FirstValue FLOAT NOT NULL,
                        FirstUnit NVARCHAR(50) NOT NULL,
                        SecondValue FLOAT NOT NULL,
                        SecondUnit NVARCHAR(50) NOT NULL,
                        Operation NVARCHAR(50) NOT NULL,
                        Result FLOAT NOT NULL,
                        MeasurementType NVARCHAR(50) NOT NULL,
                        CreatedAt DATETIME2 DEFAULT GETDATE()
                    )";

                using var command = new SqlCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                Console.WriteLine("Database table recreated with correct schema.");
            }
        }

        // Helper method to map SqlDataReader to entity
        private QuantityMeasurementEntity ReadEntity(SqlDataReader reader)
        {
            return new QuantityMeasurementEntity
            {
                Id = reader.GetInt32(0),           // Id
                FirstValue = reader.GetDouble(1),   // FirstValue
                FirstUnit = reader.GetString(2),    // FirstUnit
                SecondValue = reader.GetDouble(3),  // SecondValue
                SecondUnit = reader.GetString(4),   // SecondUnit
                Operation = reader.GetString(5),    // Operation
                Result = reader.GetDouble(6),       // Result
                MeasurementType = reader.GetString(7) // MeasurementType
            };
        }
    }
}
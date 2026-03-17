-- Create QuantityMeasurementDB database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QuantityMeasurementDB')
BEGIN
    CREATE DATABASE QuantityMeasurementDB;
END
GO

USE QuantityMeasurementDB;
GO

-- Drop table if exists to recreate with correct schema
IF OBJECT_ID('QuantityMeasurements', 'U') IS NOT NULL
    DROP TABLE QuantityMeasurements;
GO

-- Create table with correct column names matching the entity
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
);
GO

-- Verify table creation
SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'QuantityMeasurements'
ORDER BY ORDINAL_POSITION;
GO

SELECT * FROM QuantityMeasurements;
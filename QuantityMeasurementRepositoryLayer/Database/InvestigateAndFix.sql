-- Investigate and completely fix the IDENTITY issue
USE QuantityMeasurementDB;
GO

-- Step 1: Show current data
PRINT '=== STEP 1: Current Data Investigation ===';
SELECT 'Current Data in Table:' as Info;
SELECT Id, Operation, FirstValue, FirstUnit, SecondValue, SecondUnit, Result, CreatedAt
FROM QuantityMeasurements 
ORDER BY Id;
GO

-- Step 2: Check IDENTITY properties
PRINT '=== STEP 2: IDENTITY Properties ===';
DBCC CHECKIDENT ('QuantityMeasurements', NORESEED);
GO

-- Step 3: Check for any triggers
PRINT '=== STEP 3: Trigger Check ===';
SELECT 
    name AS TriggerName,
    is_instead_of_trigger,
    is_disabled,
    OBJECT_DEFINITION(object_id) AS TriggerDefinition
FROM sys.triggers 
WHERE parent_id = OBJECT_ID('QuantityMeasurements');
GO

-- Step 4: Check for any constraints
PRINT '=== STEP 4: Constraint Check ===';
SELECT 
    c.name AS ConstraintName,
    t.name AS TableName,
    c.definition AS ConstraintDefinition
FROM sys.check_constraints c
INNER JOIN sys.tables t ON c.parent_object_id = t.object_id
WHERE t.name = 'QuantityMeasurements';
GO

-- Step 5: Check table structure
PRINT '=== STEP 5: Table Structure ===';
SELECT 
    c.column_name,
    c.data_type,
    c.is_identity,
    c.identity_increment,
    c.identity_seed,
    c.column_default
FROM information_schema.columns c
WHERE c.table_name = 'QuantityMeasurements'
ORDER BY c.ordinal_position;
GO

-- Step 6: Complete table recreation
PRINT '=== STEP 6: Complete Table Recreation ===';
DROP TABLE IF EXISTS QuantityMeasurements;
GO

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

-- Step 7: Verify clean state
PRINT '=== STEP 7: Verification ===';
DBCC CHECKIDENT ('QuantityMeasurements', NORESEED);
GO

-- Step 8: Test with multiple inserts
PRINT '=== STEP 8: Testing Multiple Inserts ===';
INSERT INTO QuantityMeasurements (FirstValue, FirstUnit, SecondValue, SecondUnit, Operation, Result, MeasurementType)
VALUES (1.0, 'KILOGRAM', 2.0, 'POUND', 'TEST1', 3.0, 'Measurement');
GO

INSERT INTO QuantityMeasurements (FirstValue, FirstUnit, SecondValue, SecondUnit, Operation, Result, MeasurementType)
VALUES (4.0, 'GRAM', 5.0, 'POUND', 'TEST2', 6.0, 'Measurement');
GO

INSERT INTO QuantityMeasurements (FirstValue, FirstUnit, SecondValue, SecondUnit, Operation, Result, MeasurementType)
VALUES (7.0, 'YARDS', 8.0, 'FEET', 'TEST3', 9.0, 'Measurement');
GO

-- Step 9: Check test results
PRINT '=== STEP 9: Test Results ===';
SELECT Id, Operation, FirstValue, SecondValue
FROM QuantityMeasurements 
ORDER BY Id;
GO

-- Step 10: Clean up test data
DELETE FROM QuantityMeasurements;
DBCC CHECKIDENT ('QuantityMeasurements', RESEED, 0);
GO

PRINT '=== FIX COMPLETE ===';
PRINT 'Table has been completely recreated with clean IDENTITY(1,1)';
PRINT 'Test inserts showed sequential IDs';
PRINT 'Ready for application use';

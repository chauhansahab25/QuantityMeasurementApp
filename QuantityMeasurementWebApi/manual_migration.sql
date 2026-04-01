-- Manual migration to remove Google Auth and UserOperations table
-- Run this script only if the Users table already exists

-- Check if Users table exists, if not, skip this migration
IF OBJECT_ID('Users', 'U') IS NOT NULL
BEGIN
    -- Drop UserOperations table if exists
    IF OBJECT_ID('UserOperations', 'U') IS NOT NULL
        DROP TABLE UserOperations;

    -- Drop GoogleId index if exists
    IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_GoogleId' AND object_id = OBJECT_ID('Users'))
        DROP INDEX IX_Users_GoogleId ON Users;

    -- Drop GoogleId column if exists
    IF COL_LENGTH('Users', 'GoogleId') IS NOT NULL
        ALTER TABLE Users DROP COLUMN GoogleId;

    -- Drop ProfilePictureUrl column if exists
    IF COL_LENGTH('Users', 'ProfilePictureUrl') IS NOT NULL
        ALTER TABLE Users DROP COLUMN ProfilePictureUrl;

    -- Add PasswordHash column if not exists
    IF COL_LENGTH('Users', 'PasswordHash') IS NULL
        ALTER TABLE Users ADD PasswordHash nvarchar(100) NOT NULL DEFAULT '';

    -- Alter FirstName column if exists
    IF COL_LENGTH('Users', 'FirstName') IS NOT NULL
    BEGIN
        ALTER TABLE Users ALTER COLUMN FirstName nvarchar(50) NOT NULL;
    END

    -- Alter LastName column if exists
    IF COL_LENGTH('Users', 'LastName') IS NOT NULL
    BEGIN
        ALTER TABLE Users ALTER COLUMN LastName nvarchar(50) NOT NULL;
    END

    PRINT 'Migration completed successfully';
END
ELSE
BEGIN
    PRINT 'Users table does not exist. No migration needed. Run dotnet ef database update instead.';
END
GO

SELECT * FROM Users WHERE Email = 'priyanshuchauhan2509@gmail.com';
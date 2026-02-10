USE AIRBRANCH

IF NOT EXISTS
    (SELECT 1
     FROM sys.schemas
     WHERE name = 'air')
    BEGIN
        EXEC ('CREATE SCHEMA air');
    END;

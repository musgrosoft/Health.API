DECLARE @schemeName NVARCHAR(250);
DECLARE @name NVARCHAR(250);
DECLARE @SQL NVARCHAR(1000);

SET @schemeName = 'dbo';

SELECT @name = 
(SELECT TOP 1 o.[name] 
 FROM sysobjects o
 inner join sys.views v ON o.id = v.object_id
 WHERE SCHEMA_NAME(v.schema_id) =@schemeName AND o.[type] = 'V' AND o.category = 0 AND o.[name] NOT IN
 (
    SELECT  referenced_entity_name
    FROM sys.sql_expression_dependencies AS sed
    INNER JOIN sys.objects AS o ON sed.referencing_id = o.object_id
    WHERE referenced_schema_name = @schemeName
 )
 ORDER BY [name])

 WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DROP VIEW [' + @schemeName + '].[' + RTRIM(@name) +']'
    EXEC (@SQL)
--    PRINT 'Dropped View: ' + @name
SELECT @name = 
(SELECT TOP 1 o.[name] 
 FROM sysobjects o
 inner join sys.views v ON o.id = v.object_id
 WHERE SCHEMA_NAME(v.schema_id) = @schemeName AND o.[type] = 'V' AND o.category = 0 AND o.[name] NOT IN
 (
    SELECT  referenced_entity_name
    FROM sys.sql_expression_dependencies AS sed
    INNER JOIN sys.objects AS o ON sed.referencing_id = o.object_id
    WHERE referenced_schema_name = @schemeName
 )
 ORDER BY [name])
END

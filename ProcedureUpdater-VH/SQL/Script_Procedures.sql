SELECT
	M.definition AS Script
	, O.name AS Nombre 
FROM 
	sys.sql_modules M 
	INNER JOIN sys.objects O ON M.object_id = O.object_id 
WHERE 
	type_desc = 'SQL_STORED_PROCEDURE' 
ORDER BY 
	O.name
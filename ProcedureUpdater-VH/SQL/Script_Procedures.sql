SELECT
	M.definition AS Script
	, O.name AS Nombre 
FROM 
	sys.sql_modules M 
	INNER JOIN sys.objects O ON M.object_id = O.object_id 
WHERE 
	type_desc = 'SQL_STORED_PROCEDURE' 
	AND O.name NOT IN ('sp_alterdiagram'
						, 'sp_creatediagram'
						, 'sp_dropdiagram'
						, 'sp_helpdiagramdefinition'
						, 'sp_helpdiagrams'
						, 'sp_renamediagram'
						, 'sp_upgraddiagrams')
ORDER BY 
	O.name
SELECT 
	  T.TABLE_NAME AS nombre
	  , cast(SUM(row_count) AS INT) AS registros
FROM 
	INFORMATION_SCHEMA.tables T 
	INNER JOIN sys.dm_db_partition_stats PS ON PS.object_id = OBJECT_ID(T.TABLE_NAME)
WHERE 
	T.TABLE_NAME NOT IN ('sysdiagrams')
	AND T.TABLE_TYPE = 'BASE TABLE'
	AND T.TABLE_NAME LIKE '%@Buscar%'
GROUP BY 
	T.TABLE_NAME
ORDER BY 
	T.TABLE_NAME
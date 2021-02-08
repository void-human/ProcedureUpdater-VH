IF OBJECT_ID ('dbo.@Tabla') IS NOT NULL
	DROP TABLE dbo.@Tabla
GO

CREATE TABLE dbo.@Tabla
	(
	  @Script
	)
GO
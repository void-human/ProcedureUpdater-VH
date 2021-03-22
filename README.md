# ProcedureUpdater-VH - Efectivo para actualizar en Productivo cambios realizados en Desarrollo.

1. Esta herramienta permite guardar de manera encriptada la información de conexiones a bases de datos Transact-SQL (Microsfot SQL Server).

2. Compara el codigo de los Procedimientos Almacenados entre dos conexiones T-SQL, permite la visualización de las diferencias entre los codigos y ejecución de la actualización. Ejecutar la actualización de un Procedimiento Almacenado genera un respaldo encriptado entre la versión anterior en caso de existir (puede ser un Procedimiento nuevo) y la nueva versión del codigo, tambien genera un documento en formato SQL (*.SQL) en un directorio a selección para controladores de versiones.

3. Compara la estructura de las Tablas entre dos conexiones T-SQL, permite la visualización de las diferencias entre las dos estructuras y permite la creación de un Script para realizar los cambios manualmente. 

4. Permite la visualización de cambios en procedimientos almacenados a partir de los respaldos locales.

5. Conexión por pasos; ahora permite buscar procedimientos en una primera conexión y en un proceso por pasos una segunda busqueda a otra conexión, es decir, busca primero un listado de procedimientos almacenados, notifica al usuario que los obtuvo y que ahora puede cambiar su conexión de red o VPN en caso de ser necesario y hacer la segunda busqueda a la segunda conexión y asi ya realizar la comparación entre scripts.


ADEVERTENCIA: 
1. El Script generado para los cambios sobre Tablas es a partir de un analisis rapido de la base de datos, no considera las llaves primarias y foraneas reales, este mismo las crea a partir del nombre de las propiedades de las tablas, tomando como base la palabra "id" como una llave primaria y "id" seguido de un guíon bajo ("id_%") para las llaves foraneas, tomando como tablas de referencia el sufijo del guion bajo, por ejemplo, "id_mtr_material" considera que es un id foraneo de la tabla "mtr_material", todos los "id" del tipo "int" los considera como IDENTITY(1,1).
2. El metódo de encriptado de toda la información generada no mantiene a salvo su información, se utiliza solo para evitar la modificación de los documento de manera manual.

WORKING: 
1. Se está desarrollando una generación de scripts de cambios en Tablas mas proxima a la real.

TO WORK: 
1. Se planea implementar un parametrizador de nomenclaturas para identificar tablas tipo catalogo, para generar tambien comparaciones de información entre bases de datos.
2. Se planea implementar un controlador de versiones automatico del software.

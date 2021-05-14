# ProcedureUpdater-VH - Efectivo para actualizar en Productivo cambios realizados en Desarrollo.

1. Esta herramienta permite guardar de manera encriptada la información de conexiones a bases de datos Transact-SQL (Microsfot SQL Server).

2. Compara el codigo de los Procedimientos Almacenados entre dos conexiones T-SQL, permite la visualización de las diferencias entre los codigos y ejecución de la actualización. Ejecutar la actualización de un Procedimiento Almacenado genera un respaldo encriptado entre la versión anterior en caso de existir (puede ser un Procedimiento nuevo) y la nueva versión del codigo, tambien genera un documento en formato SQL (*.SQL) en un directorio a selección para controladores de versiones.

3. Compara la estructura de las Tablas entre dos conexiones T-SQL, permite la visualización de las diferencias entre las dos estructuras y permite la creación de un Script para realizar los cambios manualmente. 

4. Permite la visualización de cambios en procedimientos almacenados a partir de los respaldos locales.

5. Conexión por pasos; ahora permite buscar procedimientos en una primera conexión y en un proceso por pasos una segunda busqueda a otra conexión, es decir, busca primero un listado de procedimientos almacenados, notifica al usuario que los obtuvo y que ahora puede cambiar su conexión de red o VPN en caso de ser necesario y hacer la segunda busqueda a la segunda conexión y asi ya realizar la comparación entre scripts.

6. Permite la guardar una relación de Conexión a Configuración de tablas tipo catalaogo, con el fin de comparar los registros entre las tablas configuradas.

7. Configuración global y local; Permite guardar los documentos de configuración, backups y scripts en un directorio a selección dentro de la red o del sistema local, permitiendo así compartir configuraciones y respaldo con otros usuarios de la red. 

ADEVERTENCIA: 
1. El metódo de encriptado de toda la información generada no mantiene a salvo su información, se utiliza solo para evitar la modificación de los documento de manera manual.

WORKING: 
1. Restablecer respaldos, esta opción permitira al usuario restablecer un procedimiento almacenado que se encuentre en la sección de respaldos. 

TO WORK: 
1. Se planea implementar un controlador de versiones automatico del software.
2. Actualizar automaticamente registros con referencias ya establecidas en el script generado tras comparar registros de tablas tipo catalogo.


using System;
using System.Collections.Generic;
using System.Text;

namespace ProcedureUpdater_VH.SQL
{
    public abstract class SQL
    {

        public static string getProcedures()
        {
            string sScript = "";

            sScript = "	SELECT M.definition as Script, O.name as Nombre FROM sys.sql_modules M INNER JOIN sys.objects O ON M.object_id = O.object_id WHERE type_desc = 'SQL_STORED_PROCEDURE'";

            return sScript;
        }
    }
}

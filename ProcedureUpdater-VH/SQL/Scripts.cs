
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

            sScript = "SELECT Specific_name, Routine_definition FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE'";

            return sScript;
        }
    }
}

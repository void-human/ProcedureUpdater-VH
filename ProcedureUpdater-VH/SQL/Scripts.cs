
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace ProcedureUpdater_VH.SQL
{
    public class Scripts
    {

        public static string getProcedures(string sBuscar)
        {
            string sScript = "";

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_Procedures.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();
            sScript = sScript.Replace("@Buscar",String.Format("'{0}'", sBuscar));
            return sScript;
        }

        public static string getTables()
        {
            string sScript = "";

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_Tablas.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();

            return sScript;
        }

        public static string getCreateTables(string sTable)
        {
            string sScript = "";

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_CreateTable.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();

            sScript = sScript.Replace("@Tabla",sTable);

            return sScript;
        }

        public static string getTablesRowsCount(string sBusqueda)
        {
            string sScript = "";

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_Registros.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();

            sScript = sScript.Replace("@Buscar", sBusqueda);

            return sScript;
        }

        public static string getTablasInformacion(string sTabla)
        {
            string sScript = "";

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_Tabla_Informacion.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();

            sScript = sScript.Replace("@Tabla", sTabla);

            return sScript;
        }
    }
}

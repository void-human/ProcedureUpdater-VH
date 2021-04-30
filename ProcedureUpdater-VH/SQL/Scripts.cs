
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

            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_Procedures.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();
            sScript = sScript.Replace("@Buscar",String.Format("'{0}'", sBuscar));
            return sScript;
        }

        public static string getTables()
        {
            string sScript = "";

            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_Tablas.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();

            return sScript;
        }

        public static string getCreateTables(string sTable)
        {
            string sScript = "";

            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_CreateTable.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();

            sScript = sScript.Replace("@Tabla",sTable);

            return sScript;
        }
    }
}

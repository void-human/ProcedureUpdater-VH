
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

        public string getProcedures()
        {
            string sScript = "";

            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_Procedures.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();

            return sScript;
        }

        public string getTables()
        {
            string sScript = "";

            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_Tablas.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();

            return sScript;
        }

        public string getCreateTables(string sTable, string[] arrsPropiedades)
        {
            string sScript = "";

            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ProcedureUpdater_VH.SQL.Script_CreateTable.sql");
            StreamReader reader = new StreamReader(stream);
            sScript = reader.ReadToEnd();

            sScript = sScript.Replace("@Tabla",sTable);

            string sPropiedades = "";
            for (int i = 0; i < arrsPropiedades.Length; i++)
            {
                string sLinea = arrsPropiedades[i];

                if (i == 0)
                {
                    sPropiedades += "     " + sLinea + "\n\r";
                }
                else
                {
                    sPropiedades += "   , " + sLinea + "\n\r";
                }
            
            }

            sScript = sScript.Replace("@Script", sPropiedades);

            return sScript;
        }
    }
}

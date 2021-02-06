using ProcedureUpdater_VH.Metodos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProcedureUpdater_VH.SQL
{
    public class Ejecutor
    {
        private SqlConnection sqlcConexion;
        private SqlCommand cmdProcedimiento;
        private SqlDataReader DR;
        public List<Procedure> lstProcedimiento = new List<Procedure>();

        private SqlDataReader Ejecutar(Conexion conexion, string sScript)
        {

            string sConexion = string.Format(@"Data Source={0};Initial Catalog={1};User ID={2};Password={3}", conexion.IP, conexion.BDD, conexion.Usuario, conexion.Contrasena);
            sqlcConexion = new SqlConnection(sConexion);
            sqlcConexion.Open();

            cmdProcedimiento = new SqlCommand(sScript, sqlcConexion);
            DR = cmdProcedimiento.ExecuteReader();

            return DR;
        }

        private void Cerrar()
        {
            DR.Close();
            cmdProcedimiento.Dispose();
            sqlcConexion.Close();
        }

        public void ObtenerProcedimientos(Conexion ConexionV1, Conexion ConexionV2)
        {
            try
            {
                Ejecutar(ConexionV1, SQL.getProcedures());
                Comparar();
                Cerrar();

                Ejecutar(ConexionV2, SQL.getProcedures());
                Comparar();
                Cerrar();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Comparar()
        {
            try
            {
                if (DR.HasRows)
                {
                    while (DR.Read())
                    {
                        string sNombre = (string)DR["Nombre"];
                        string sScript = (string)DR["Script"];
                        sScript = sScript.Replace("[", "").Replace("]", "");
                        int nIndice = lstProcedimiento.FindIndex(x => x.Nombre.Equals(sNombre));
                        if (nIndice == -1)
                        {
                            lstProcedimiento.Add(new Procedure()
                            {
                                Modificar = false,
                                Nombre = sNombre,
                                DefinicionV1 = sScript
                            });
                        }
                        else
                        {
                            lstProcedimiento[nIndice].DefinicionV2 = sScript;
                            if (lstProcedimiento[nIndice].DefinicionV1.Equals(sScript))
                            {
                                lstProcedimiento.RemoveAt(nIndice);
                            }
                            else
                            {
                                lstProcedimiento[nIndice].Modificar = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

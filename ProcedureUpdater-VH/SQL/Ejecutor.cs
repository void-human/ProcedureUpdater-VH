using ProcedureUpdater_VH.Metodos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ProcedureUpdater_VH.SQL
{
    public class Ejecutor
    {
        private SqlConnection sqlcConexion;
        private SqlCommand cmdProcedimiento;
        private SqlDataReader DR;

        public SqlDataReader Ejecutar(Conexion conexion, string sScript)
        {
            
            string sConexion = string.Format(@"Data Source={0};Initial Catalog={1};User ID={2};Password={3}", conexion.IP, conexion.BDD, conexion.Usuario, conexion.Contrasena);
            sqlcConexion = new SqlConnection(sConexion);
            sqlcConexion.Open();

            cmdProcedimiento = new SqlCommand(sScript, sqlcConexion);
            DR = cmdProcedimiento.ExecuteReader();

            return DR;
        }

        public void Cerrar()
        {
            DR.Close();
            cmdProcedimiento.Dispose();
            sqlcConexion.Close();
        }

    }
}

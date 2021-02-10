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
        public List<Procedure> lstProcedimiento = null;
        public List<VersionesTabla> lstVersionesTablas = null;

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
                lstProcedimiento = new List<Procedure>();

                Ejecutar(ConexionV1, new Scripts().getProcedures());
                CompararProcedimientos();
                Cerrar();

                Ejecutar(ConexionV2, new Scripts().getProcedures());
                CompararProcedimientos();
                Cerrar();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CompararProcedimientos()
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

        public bool ActualizarConexion(Conexion conexion, string Script, string ScriptV2 = "")
        {
            try
            {
                Ejecutar(conexion, Script);
                CompararProcedimientos();
                Cerrar();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public void GuardarTablas(bool bPrimerConexion)
        {
            try
            {
                if (DR.HasRows)
                {
                    while (DR.Read())
                    {
                        string Tabla = (string)DR["Tabla"];
                        string Columna = (string)DR["Columna"];
                        string Nulo = (string)DR["Nulo"];
                        string Tipo = (string)DR["Tipo"];
                        int Longitud = (int)DR["Longitud"];
                        string Completo = (string)DR["ColumnaDefinicion"];

                        Columna objColumna = new Columna()
                        {
                            Nombre = Columna,
                            Nulo = Nulo,
                            Tipo = Tipo,
                            Longitud = Longitud,
                            Completo = Completo
                        };

                        int nIndice = lstVersionesTablas.FindIndex(x => x.TablaV1 != null && x.TablaV1.Nombre.Equals(Tabla));
                        if (nIndice == -1 && bPrimerConexion)
                        {
                            lstVersionesTablas.Add(new VersionesTabla()
                            {
                                TablaV1 = new Tabla()
                                {
                                    Nombre = Tabla,
                                    lstColumnas = new List<Columna>()
                                    {
                                         objColumna
                                    }
                                },

                                TablaV2 = new Tabla()
                                {
                                    Nombre = Tabla,
                                    lstColumnas = new List<Columna>() 
                                    { 
                                        
                                    }
                                },
                                bModificar = false
                            });
                        }
                        else if (nIndice != -1 && bPrimerConexion)
                        {

                            lstVersionesTablas[nIndice].TablaV1.lstColumnas.Add(objColumna);
                        }
                        else if (nIndice != -1 && !bPrimerConexion)
                        {
                            lstVersionesTablas[nIndice].TablaV2.lstColumnas.Add(objColumna);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void ObtenerTablas(Conexion ConexionV1, Conexion ConexionV2)
        {
            try
            {
                lstVersionesTablas = new List<VersionesTabla>();

                Ejecutar(ConexionV1, new Scripts().getTables());
                GuardarTablas(true);
                Cerrar();

                Ejecutar(ConexionV2, new Scripts().getTables());
                GuardarTablas(false);
                Cerrar();

                CompararTablas();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CompararTablas()
        {
            for (int i = 0; i < lstVersionesTablas.Count; i++)
            {
                if (lstVersionesTablas[i].TablaV1.lstColumnas.Count != lstVersionesTablas[i].TablaV2.lstColumnas.Count)
                {
                    lstVersionesTablas[i].bModificar = true;
                }
                else
                {
                    bool bModificar = false;
                    for(int x = 0; x < lstVersionesTablas[i].TablaV1.lstColumnas.Count; x++)
                    {
                        Columna columnaV1 = lstVersionesTablas[i].TablaV1.lstColumnas[x];
                        Columna columnaV2 = lstVersionesTablas[i].TablaV2.lstColumnas[x];
                        if (!columnaV1.Nombre.Equals(columnaV2.Nombre) || !columnaV1.Nulo.Equals(columnaV2.Nulo) || !columnaV1.Tipo.Equals(columnaV2.Tipo) || columnaV1.Longitud != columnaV2.Longitud)
                        {
                            bModificar = true;
                            break;
                        }
                    }
                    lstVersionesTablas[i].bModificar = bModificar;
                }
            }

            //Solo extraeremos las tablas con cambios
            lstVersionesTablas = lstVersionesTablas.Where(x => x.bModificar).ToList();
        }

    }
}

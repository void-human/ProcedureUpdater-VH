using ProcedureUpdater_VH.Metodos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcedureUpdater_VH.SQL
{
    public class Ejecutor
    {
        private SqlConnection sqlcConexion;
        private SqlCommand cmdProcedimiento;
        public SqlDataReader DR;
        public List<Procedure> lstProcedimiento = null;
        public List<VersionesTabla> lstVersionesTablas = null;
        public TablaCatalogo tablaCatalogo = null;

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

        #region Procedimientos

        public void ObtenerProcedimientos(Conexion ConexionV1, Conexion ConexionV2, string sBuscar)
        {
            try
            {
                lstProcedimiento = new List<Procedure>();

                Ejecutar(ConexionV1, Scripts.getProcedures(sBuscar));
                CompararProcedimientos();
                Cerrar();

                Ejecutar(ConexionV2, Scripts.getProcedures(sBuscar));
                CompararProcedimientos();
                Cerrar();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ObtenerProcedimientosPasos(Conexion ConexionV1, Conexion ConexionV2, bool bPrimerPaso, string sBuscar)
        {
            try
            {
                lstProcedimiento = new List<Procedure>();

                if (bPrimerPaso)
                {
                    Ejecutar(ConexionV1, Scripts.getProcedures(sBuscar));
                    CompararProcedimientos();
                    Cerrar();
                }
                else
                {
                    Ejecutar(ConexionV2, Scripts.getProcedures(sBuscar));
                    CompararProcedimientos();
                    Cerrar();
                }
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

        public bool ActualizarConexionProcedimientos(Conexion conexion, string Script, string ScriptV2 = "")
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

        #endregion

        #region Tablas

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
                                    },
                                    sScripts = ""
                                },

                                TablaV2 = new Tabla()
                                {
                                    Nombre = Tabla,
                                    lstColumnas = new List<Columna>() 
                                    { 
                                        
                                    },
                                    sScripts = ""
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

        public string ObtenerCodigoTabla(Conexion conexion, string sTabla)
        {
            string sScripts = "";

            try 
            {
                Ejecutar(conexion, Scripts.getCreateTables(sTabla));
                if (DR != null && DR.HasRows && DR.Read())
                {
                    sScripts = (string)DR["Tabla"];
                }
            } 
            catch (Exception ex) 
            {
                throw ex;
            }

            return sScripts;
        }

        public void ObtenerTablas(Conexion ConexionV1, Conexion ConexionV2)
        {
            try
            {
                lstVersionesTablas = new List<VersionesTabla>();

                Ejecutar(ConexionV1, Scripts.getTables());
                GuardarTablas(true);
                Cerrar();

                Ejecutar(ConexionV2, Scripts.getTables());
                GuardarTablas(false);
                Cerrar();

                CompararTablas();

                for (int version = 0; version < lstVersionesTablas.Count; version++)
                {
                    if (!lstVersionesTablas[version].TablaV1.Nombre.Equals(""))
                    {
                        lstVersionesTablas[version].TablaV1.sScripts = ObtenerCodigoTabla(ConexionV1, lstVersionesTablas[version].TablaV1.Nombre);
                    }

                    if (!lstVersionesTablas[version].TablaV2.Nombre.Equals(""))
                    {
                        lstVersionesTablas[version].TablaV2.sScripts = ObtenerCodigoTabla(ConexionV2, lstVersionesTablas[version].TablaV2.Nombre);
                    }
                    
                }
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
        #endregion

        #region Catalogos

        public TablaCatalogo ObtenerTablasCatalogos(Conexion ConexionV1, Conexion ConexionV2, string sBuscar)
        {
            try
            {
                tablaCatalogo = null;
                Ejecutar(ConexionV1, Scripts.getTablesRowsCount(sBuscar));
                CompararConfiguracion(ConexionV1, true);
                Cerrar();

                Ejecutar(ConexionV2, Scripts.getTablesRowsCount(sBuscar));
                CompararConfiguracion(ConexionV2, true);
                Cerrar();

                return tablaCatalogo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TablaCatalogo ObtenerTablasCatalogos(Conexion ConexionV1, string sBuscar)
        {
            try
            {
                tablaCatalogo = null;
                Ejecutar(ConexionV1, Scripts.getTablesRowsCount(sBuscar));
                CompararConfiguracion(ConexionV1, false);
                Cerrar();

                return tablaCatalogo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CompararConfiguracion(Conexion conexion, bool bLimpiar)
        {
            try
            {
                if (tablaCatalogo == null)
                {
                    tablaCatalogo = new TablaCatalogo();
                    tablaCatalogo.conexion = conexion;
                    tablaCatalogo.dtCreacion = DateTime.Now;
                    tablaCatalogo.lstCatalogos = new List<Catalogo>();

                    if (DR != null && DR.HasRows)
                    {
                        while (DR.Read())
                        {
                            tablaCatalogo.lstCatalogos.Add(new Catalogo()
                            {
                                catalogo = false,
                                nombre = (string)DR["nombre"],
                                registrosv1 = (int)DR["registros"],
                                registrosv2 = 0
                            });
                        }
                    }

                    TablaCatalogo tablaCatalogoConfiguracion = Conversor.AbrirTablaCatalogoXML(conexion);
                    if (tablaCatalogoConfiguracion != null && tablaCatalogoConfiguracion.lstCatalogos != null)
                    {
                        foreach (Catalogo catalogo in tablaCatalogoConfiguracion.lstCatalogos.Where(x => x.catalogo))
                        {
                            int nIndice = tablaCatalogo.lstCatalogos.FindIndex(x => x.nombre.Equals(catalogo.nombre));
                            if (nIndice != -1)
                            {
                                tablaCatalogo.lstCatalogos[nIndice].catalogo = true;
                            }
                        }

                        if (bLimpiar)
                        {
                            tablaCatalogo.lstCatalogos.RemoveAll(x => !x.catalogo);
                        }  
                    }
                }
                else
                {
                    if (DR != null && DR.HasRows)
                    {
                        while (DR.Read())
                        {
                            string sNombre = (string)DR["nombre"];
                            int nRegistros = (int)DR["registros"];

                            int nIndice = tablaCatalogo.lstCatalogos.FindIndex(x => x.nombre.Equals(sNombre));
                            if (nIndice != -1)
                            {
                                tablaCatalogo.lstCatalogos[nIndice].registrosv2 = nRegistros;
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

        public SqlDataReader ObtenerRegistros(Conexion conexion, string sTabla)
        {
            try
            {
                return Ejecutar(conexion, Scripts.getTablasInformacion(sTabla));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ActualizarCatalogos(Conexion conexion, string Script)
        {
            try
            {
                Ejecutar(conexion, Script);
                Cerrar();
                return true;
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}

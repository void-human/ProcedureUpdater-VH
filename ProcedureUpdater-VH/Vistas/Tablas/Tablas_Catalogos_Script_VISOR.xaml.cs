using ProcedureUpdater_VH.Metodos;
using ProcedureUpdater_VH.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProcedureUpdater_VH.Vistas.Tablas
{
    /// <summary>
    /// Lógica de interacción para Tablas_Catalogos_Script_VISOR.xaml
    /// </summary>
    public partial class Tablas_Catalogos_Script_VISOR : Window
    {
        private Conexion ConexionV2;
        public bool bGuardo = false;

        public Tablas_Catalogos_Script_VISOR(string sTabla, List<dynamic> lstdyv1, List<dynamic> lstdyv2, Conexion ConexionV2)
        {
            InitializeComponent();
            this.ConexionV2 = ConexionV2;

            GenerarScriptEliminar(sTabla, lstdyv2);
            GenerarScriptInsertar(sTabla, lstdyv1);

            this.Title += " " + sTabla;
        }

        public void GenerarScriptEliminar(string sTabla, List<dynamic> lstdyv2) 
        {
            string sScript = "";
            foreach (dynamic estructura in lstdyv2.Where(x => ((bool)((IDictionary<String, Object>)x)["Eliminar"])))
            {
                string sScriptRow = String.Format("DELETE FROM {0} WHERE @columna = @value;", sTabla);

                foreach (string sKey in ((IDictionary<String, Object>)estructura).Keys)
                {
                    if (!sKey.Equals("Eliminar"))
                    {                        
                        string sValor = (((IDictionary<String, Object>)estructura)[sKey]).ToString();

                        sScriptRow = sScriptRow.Replace("@columna", sKey);
                        sScriptRow = sScriptRow.Replace("@value", sValor);

                        break;
                    }
                }

                sScript += sScriptRow + "; \n";
            }

            txt_Scripts.Text += sScript + "\n\n";

        }

        public void GenerarScriptInsertar(string sTabla, List<dynamic> lstdyv1)
        {

            string sScript = String.Format("SET IDENTITY_INSERT {0} ON;\n", sTabla);
            foreach (dynamic estructura in lstdyv1.Where(x => ((bool)((IDictionary<String, Object>)x)["Insertar"])))
            {
                bool bPrimerParametro = true;
                string sScriptRow = String.Format("INSERT INTO {0} (@Columnas) SELECT ", sTabla);
                string sColumnas = "";
                foreach (string sKey in ((IDictionary<String, Object>)estructura).Keys)
                {
                    if (!sKey.Equals("Insertar"))
                    {
                        if (bPrimerParametro)
                        {
                            bPrimerParametro = !bPrimerParametro;
                        }
                        else
                        {
                            sScriptRow += ", ";
                            sColumnas += ", ";
                        }

                        if (sKey.Equals("lup_date"))
                        {
                            Console.WriteLine("Es Fecha");
                        }

                        object objValor = (((IDictionary<String, Object>)estructura)[sKey]);
                        switch (objValor.GetType().Name)
                        {
                            case "String":
                                if (((string)objValor).Equals("NULL"))
                                {
                                    sScriptRow += "NULL";
                                }
                                else
                                {
                                    sScriptRow += "'" + (string)objValor + "'";
                                }
                                
                                break;
                            case "Boolean":
                                if ((bool) objValor)
                                {
                                    sScriptRow += "1";
                                }
                                else
                                {
                                    sScriptRow += "0";
                                }
                                
                                break;
                            case "Int32":
                                sScriptRow += ((int)objValor).ToString();
                                break;
                            case "DateTime":
                                sScriptRow += ((DateTime)objValor).ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            default:
                                sScriptRow += (((IDictionary<String, Object>)estructura)[sKey]).ToString();
                                break;
                        }

                        sColumnas += sKey;
                    }
                }

                sScript += sScriptRow.Replace("@Columnas", sColumnas) + ";\n";
            }
            sScript += String.Format("SET IDENTITY_INSERT {0} OFF;\n", sTabla);
            txt_Scripts.Text += sScript;
        }

        public void EjecutarScript()
        {
            try
            {
                Ejecutor ejecutor = new Ejecutor();
                string sScript = txt_Scripts.Text;
                if (ejecutor.ActualizarCatalogos(ConexionV2, sScript))
                {
                    Msg.Success("El Script se ejecuto y actualizo correctamente el catalogo.");
                }
            }
            catch(Exception ex)
            {
                Msg.Error(ex);
            }
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btn_Ejecutar_Click(object sender, RoutedEventArgs e)
        {
            EjecutarScript();
        }
    }
}

using ProcedureUpdater_VH.Metodos;
using ProcedureUpdater_VH.SQL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcedureUpdater_VH.Vistas.Tablas
{
    /// <summary>
    /// Lógica de interacción para Tablas_Catalogos_VISOR.xaml
    /// </summary>
    public partial class Tablas_Catalogos_VISOR : Page
    {
        private Conexion ConexionV1;
        private Conexion ConexionV2;
        private string sTabla;
        private Ejecutor ejecutor = new Ejecutor();
        private List<string> lstsDatosV1 = new List<string>();
        private List<string> lstsDatosV2 = new List<string>();

        public Tablas_Catalogos_VISOR(Conexion conexionv1, Conexion conexionv2, string sTabla)
        {
            InitializeComponent();

            this.ConexionV1 = conexionv1;
            this.ConexionV2 = conexionv2;
            this.sTabla = sTabla;
            this.lbl_Titulo.Content += " " + sTabla;
            CargarTablas();
        }

        private void AbrirScript()
        {
            try
            {
                Tablas_Catalogos_Script_VISOR visor = new Tablas_Catalogos_Script_VISOR(sTabla, lstsDatosV1);
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        private void CargarTablas()
        {
            try
            {

                dg_registrosv1.ItemsSource = null;
                dg_registrosv2.ItemsSource = null;

                dg_registrosv1.Columns.Clear();
                dg_registrosv2.Columns.Clear();

                dg_registrosv1.Items.Clear();
                dg_registrosv2.Items.Clear();

                dg_registrosv1.Items.Refresh();
                dg_registrosv2.Items.Refresh();

                SqlDataReader drv1 = ejecutor.ObtenerRegistros(ConexionV1, sTabla);
                SqlDataReader drv2 = ejecutor.ObtenerRegistros(ConexionV2, sTabla);

                lstsDatosV1 = new List<string>();
                lstsDatosV2 = new List<string>();

                List<dynamic> lstdynDatosv1 = new List<dynamic>(); //Temporal, almacenara toda nuestra información, y va a la par en indices con lstarrDatosV1
                List<dynamic> lstdynDatosv2 = new List<dynamic>(); //Temporal, almacenara toda nuestra información, y va a la par en indices con lstarrDatosV2

                int nRowv1 = 0; // Para verificar primer registro y crear las columnas
                while (drv1.Read()) 
                {
                    //Estructura dinamica
                    dynamic dyDatos = new System.Dynamic.ExpandoObject();

                    if (nRowv1 == 0)
                    {
                        for (int i = 0; i < drv1.FieldCount; i++) // Recorremos columnas
                        {
                            //Agregamos columnas al primer DataGrid
                            DataGridTextColumn txColumna = new DataGridTextColumn();
                            txColumna.Header = drv1.GetName(i);
                            txColumna.Binding = new Binding(drv1.GetName(i)); // Generamos el Binding por el nombre en el indice de la columna del SqlDataReader
                            dg_registrosv1.Columns.Add(txColumna);
                        }
                    }

                    List<string> lstRow = new List<string>();
                    for (int i = 0; i < drv1.FieldCount; i++)
                    {
                        string sValor = drv1[i].ToString();

                        Type t = drv1[i].GetType();
                        if (t.Name.Equals("Boolean"))
                        {
                            if (sValor.Equals("True"))
                            {
                                sValor = "1";
                            }
                            else
                            {
                                sValor = "0";
                            }
                        }
                        else if (sValor == null)
                        {
                            sValor = "";
                        }

                        lstRow.Add(sValor);

                        //Convertimos temporalmente a la estructura dinamica en un diccionario para asignarle una Key y un Valor, la Key funciona asignación de nombre de propiedad.
                        ((IDictionary<String, Object>)dyDatos).Add(drv1.GetName(i), sValor);
                    }

                    lstdynDatosv1.Add(dyDatos);
                    lstsDatosV1.Add(string.Join(",", lstRow));

                    nRowv1++;
                }

                int nRowv2 = 0; // Para verificar primer registro y crear las columnas
                while (drv2.Read())
                {
                    //Estructura dinamica
                    dynamic dyDatos = new System.Dynamic.ExpandoObject();

                    if (nRowv2 == 0)
                    {
                        for (int i = 0; i < drv2.FieldCount; i++) // Recorremos columnas
                        {
                            //Agregamos columnas al primer DataGrid
                            DataGridTextColumn txColumna = new DataGridTextColumn();
                            txColumna.Header = drv2.GetName(i);
                            txColumna.Binding = new Binding(drv2.GetName(i)); // Generamos el Binding por el nombre en el indice de la columna del SqlDataReader
                            dg_registrosv2.Columns.Add(txColumna);
                        }
                    }

                    List<string> lstRow = new List<string>();
                    for (int i = 0; i < drv2.FieldCount; i++)
                    {
                        string sValor = drv2[i].ToString();

                        Type t = drv2[i].GetType();
                        if (t.Name.Equals("Boolean"))
                        {
                            if (sValor.Equals("True"))
                            {
                                sValor = "1";
                            }
                            else
                            {
                                sValor = "0";
                            }
                        }
                        else if (sValor == null)
                        {
                            sValor = "";
                        }

                        lstRow.Add(sValor);

                        //Convertimos temporalmente a la estructura dinamica en un diccionario para asignarle una Key y un Valor, la Key funciona asignación de nombre de propiedad.
                        ((IDictionary<String, Object>)dyDatos).Add(drv2.GetName(i), sValor);
                    }

                    lstdynDatosv2.Add(dyDatos);
                    lstsDatosV2.Add(string.Join(",", lstRow));

                    nRowv2++;
                }


                for (int i = 0; i < lstsDatosV1.Count; i++)
                {
                    DataGridRow dgRow = new DataGridRow();
                    if (!lstsDatosV2.Exists(x => x.Equals(lstsDatosV1[i])))
                    {
                        dgRow.Background = Msg.getColor(Colores.VerdeClaro);
                    }
                    dgRow.Item = lstdynDatosv1[i];
                    dg_registrosv1.Items.Add(dgRow);
                }

                for (int i = 0; i < lstsDatosV2.Count; i++)
                {
                    DataGridRow dgRow = new DataGridRow();
                    if (!lstsDatosV1.Exists(x => x.Equals(lstsDatosV2[i])))
                    {
                        dgRow.Background = Msg.getColor(Colores.RojoClaro);
                    }
                    dgRow.Item = lstdynDatosv2[i];
                    dg_registrosv2.Items.Add(dgRow);
                }

                dg_registrosv1.Items.Refresh();
                dg_registrosv2.Items.Refresh();

            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }

        private void btn_Recargar_Click(object sender, RoutedEventArgs e)
        {
            CargarTablas();
        }

        private void btn_Generar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}

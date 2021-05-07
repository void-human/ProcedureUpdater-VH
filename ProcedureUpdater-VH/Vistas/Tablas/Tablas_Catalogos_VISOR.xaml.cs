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
        private List<string> lstsDatosV1 = null;
        private List<string> lstsDatosV2 = null;
        List<dynamic> lstdynDatosv1 = null; 
        List<dynamic> lstdynDatosv2 = null; 

        public Tablas_Catalogos_VISOR(Conexion ConexionV1, Conexion ConexionV2, string sTabla)
        {
            InitializeComponent();

            this.ConexionV1 = ConexionV1;
            this.ConexionV2 = ConexionV2;
            this.sTabla = sTabla;
            this.lbl_Titulo.Content += " " + sTabla;
            CargarTablas();
        }

        private void AbrirScript()
        {
            try
            {
                Tablas_Catalogos_Script_VISOR visor = new Tablas_Catalogos_Script_VISOR(sTabla, lstdynDatosv1, lstdynDatosv2, ConexionV2);
                visor.ShowDialog();
                if (visor.bGuardo)
                {
                    CargarTablas();
                }
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

                lstdynDatosv1 = new List<dynamic>(); //Temporal, almacenara toda nuestra información, y va a la par en indices con lstarrDatosV1
                lstdynDatosv2 = new List<dynamic>(); //Temporal, almacenara toda nuestra información, y va a la par en indices con lstarrDatosV2

                int nRowv1 = 0; // Para verificar primer registro y crear las columnas
                while (drv1.Read()) 
                {
                    //Estructura dinamica
                    dynamic dyDatosv1 = new System.Dynamic.ExpandoObject();

                    if (nRowv1 == 0)
                    {
                        for (int i = 0; i < drv1.FieldCount; i++) // Recorremos columnas
                        {
                            if (i == 0)
                            {
                                DataGridCheckBoxColumn chxColumn = new DataGridCheckBoxColumn();
                                chxColumn.Header = "Insertar";
                                chxColumn.Binding = new Binding("Insertar");
                                dg_registrosv1.Columns.Add(chxColumn);
                            }

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
                        if (i == 0)
                        {
                            bool Insertar = false;
                            ((IDictionary<String, Object>)dyDatosv1).Add("Insertar", Insertar);
                        }

                        string sValor = drv1[i].ToString();
                        DateTime dtValor;
                        bool bValor;

                        Type t = drv1[i].GetType();
                        if (t.Name.Equals("Boolean"))
                        {
                            bValor = (bool)drv1[drv1.GetName(i)];
                            ((IDictionary<String, Object>)dyDatosv1).Add(drv1.GetName(i), bValor);
                            
                            sValor = "0";
                            if (bValor)
                            {
                                sValor = "1";
                            }
                        }
                        else if (t.Name.Equals("DateTime"))
                        {
                            dtValor = (DateTime)drv1[drv1.GetName(i)];
                            if (dtValor.Hour.ToString().Equals("0") && dtValor.Hour.ToString().Equals("0") && dtValor.Hour.ToString().Equals("0"))
                            {
                                sValor = dtValor.Date.ToString("yyyy-MM-dd");
                                ((IDictionary<String, Object>)dyDatosv1).Add(drv1.GetName(i), sValor);
                            }
                            else
                            {
                                ((IDictionary<String, Object>)dyDatosv1).Add(drv1.GetName(i), dtValor);
                            }
                        }
                        else if (t.Name.Equals("DBNull"))
                        {
                            sValor = "NULL";
                            ((IDictionary<String, Object>)dyDatosv1).Add(drv1.GetName(i), sValor);
                        }
                        else
                        {
                            ((IDictionary<String, Object>)dyDatosv1).Add(drv1.GetName(i), sValor);
                        }

                        lstRow.Add(sValor);

                        //Convertimos temporalmente a la estructura dinamica en un diccionario para asignarle una Key y un Valor, la Key funciona asignación de nombre de propiedad.
                        
                    }

                    lstdynDatosv1.Add(dyDatosv1);
                    lstsDatosV1.Add(string.Join(",", lstRow));

                    nRowv1++;
                }

                int nRowv2 = 0; // Para verificar primer registro y crear las columnas
                while (drv2.Read())
                {
                    //Estructura dinamica
                    dynamic dyDatosv2 = new System.Dynamic.ExpandoObject();

                    if (nRowv2 == 0)
                    {
                        for (int i = 0; i < drv2.FieldCount; i++) // Recorremos columnas
                        {
                            if (i == 0)
                            {
                                DataGridCheckBoxColumn chxColumn = new DataGridCheckBoxColumn();
                                chxColumn.Header = "Eliminar";
                                chxColumn.Binding = new Binding("Eliminar");
                                dg_registrosv2.Columns.Add(chxColumn);
                            }

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
                        if (i == 0)
                        {
                            bool Eliminar = false;
                            ((IDictionary<String, Object>)dyDatosv2).Add("Eliminar", Eliminar);
                        }

                        string sValor = drv2[i].ToString();
                        bool bValor;
                        DateTime dtValor;
                        Type t = drv2[i].GetType();
                        if (t.Name.Equals("Boolean"))
                        {
                            bValor = (bool)drv2[drv2.GetName(i)];
                            ((IDictionary<String, Object>)dyDatosv2).Add(drv2.GetName(i), bValor);

                            sValor = "0";
                            if (bValor)
                            {
                                sValor = "1";
                            }
                        }
                        else if (t.Name.Equals("DateTime"))
                        {
                            dtValor = (DateTime)drv2[drv2.GetName(i)];
                            if (dtValor.Hour.ToString().Equals("0") && dtValor.Hour.ToString().Equals("0") && dtValor.Hour.ToString().Equals("0"))
                            {
                                sValor = dtValor.Date.ToString("yyyy-MM-dd");
                                ((IDictionary<String, Object>)dyDatosv2).Add(drv2.GetName(i), sValor);
                            }
                            else
                            {
                                ((IDictionary<String, Object>)dyDatosv2).Add(drv2.GetName(i), dtValor);
                            }
                        }
                        else if (t.Name.Equals("DBNull"))
                        {
                            sValor = "NULL";
                            ((IDictionary<String, Object>)dyDatosv2).Add(drv1.GetName(i), sValor);
                        }
                        else
                        {
                            ((IDictionary<String, Object>)dyDatosv2).Add(drv2.GetName(i), sValor);
                        }

                        lstRow.Add(sValor);

                    }

                    lstdynDatosv2.Add(dyDatosv2);
                    lstsDatosV2.Add(string.Join(",", lstRow));

                    nRowv2++;
                }


                for (int i = 0; i < lstsDatosV1.Count; i++)
                {
                    DataGridRow dgRow = new DataGridRow();
                    if (!lstsDatosV2.Exists(x => x.Equals(lstsDatosV1[i])))
                    {
                        ((IDictionary<String, Object>)lstdynDatosv1[i])["Insertar"] = true;
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
                        ((IDictionary<String, Object>)lstdynDatosv2[i])["Eliminar"] = true;
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
            AbrirScript();
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        //private void dg_registrosv1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        DataGridRow dgrRegistro = (DataGridRow)dg_registrosv1.SelectedItem;
                
        //        if (dgrRegistro != null)
        //        {
        //            int nIndice = lstdynDatosv1.FindIndex(x => x == (dynamic)dgrRegistro.Item);
        //            if (nIndice != -1)
        //            {
        //                bool bInsertar = !(bool)((IDictionary<String, Object>)lstdynDatosv1[nIndice])["Insertar"];
        //                ((IDictionary<String, Object>)lstdynDatosv1[nIndice])["Insertar"] = bInsertar;

        //                dgrRegistro.Item = (dynamic)((IDictionary<String, Object>)lstdynDatosv1[nIndice]);
        //                if (bInsertar)
        //                {
        //                    dgrRegistro.Background = Msg.getColor(Colores.VerdeClaro);
        //                }
                        
        //                dg_registrosv1.Items.Refresh();
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Msg.Error(ex);
        //    }
        //}

        //private void dg_registrosv2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        DataGridRow dgrRegistro = (DataGridRow)dg_registrosv2.SelectedItem;
        //        if (dgrRegistro != null)
        //        {
        //            int nIndice = lstdynDatosv2.FindIndex(x => x == (dynamic)dgrRegistro.Item);
        //            if (nIndice != -1)
        //            {
        //                ((IDictionary<String, Object>)lstdynDatosv2[nIndice])["Eliminar"] = !(bool)((IDictionary<String, Object>)lstdynDatosv2[nIndice])["Eliminar"];
        //                dg_registrosv2.Items.Clear();
        //                dg_registrosv2.ItemsSource = lstdynDatosv2;
        //                dg_registrosv2.Items.Refresh();
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Msg.Error(ex);
        //    }
        //}
    }
}

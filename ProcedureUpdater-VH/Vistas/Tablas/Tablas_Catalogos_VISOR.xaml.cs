using ProcedureUpdater_VH.Metodos;
using ProcedureUpdater_VH.SQL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public Tablas_Catalogos_VISOR(Conexion conexionv1, Conexion conexionv2, string sTabla)
        {
            InitializeComponent();

            this.ConexionV1 = conexionv1;
            this.ConexionV2 = conexionv2;
            this.sTabla = sTabla;
            this.lbl_Titulo.Content += " " + sTabla;
            CargarTablas();
        }

        private void CargarTablas()
        {
            try
            {
                dg_registrosv1.ItemsSource = null;
                dg_registrosv1.ItemsSource = ejecutor.ObtenerRegistros(ConexionV1, sTabla);
                dg_registrosv1.Items.Refresh();

                dg_registrosv2.ItemsSource = null;
                dg_registrosv2.ItemsSource = ejecutor.ObtenerRegistros(ConexionV2, sTabla);
                dg_registrosv2.Items.Refresh();
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }

            int nIndice = 0;
            foreach (var rowv1 in dg_registrosv1.Items)
            {
                bool bEncontro = false; ;
                foreach (var rowv2 in dg_registrosv2.Items)
                {
                    if (rowv1 == rowv2) 
                    {
                        bEncontro = true;
                        break;
                    }
                }

                if (bEncontro)
                {
                    DataGridRow dgrow = (DataGridRow)dg_registrosv1.ItemContainerGenerator.ContainerFromIndex(nIndice);
                    dgrow.Background = Msg.getColor(Colores.Verde);
                }
                else
                {
                    DataGridRow dgrow = (DataGridRow)dg_registrosv1.ItemContainerGenerator.ContainerFromIndex(nIndice);
                    if (dgrow != null)
                    {
                        dgrow.Background = Msg.getColor(Colores.Naranja);
                    }
                }

                nIndice++;
            }

            dg_registrosv1.Items.Refresh();
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

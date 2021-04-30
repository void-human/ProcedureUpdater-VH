using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ProcedureUpdater_VH.Metodos;
using ProcedureUpdater_VH.SQL;

namespace ProcedureUpdater_VH.Vistas
{
    /// <summary>
    /// Lógica de interacción para Tablas_MON.xaml
    /// </summary>
    public partial class Tablas_MON : Page
    {
        private static NavigationService nsNavigator { get; } = (Application.Current.MainWindow as Principal_MAIN).frm_Principal.NavigationService;
        private List<VersionesTabla> lstVersionesTablas = null;
        private List<VersionesTabla> lstVersionesTablasBusqueda = null;
        private List<Conexion> lstConexiones = null;
        private Ejecutor ejecutor = null;
        private Conexion ConexionV1;
        private Conexion ConexionV2;


        public Tablas_MON()
        {
            InitializeComponent();
            CargarDatos();
            Configuracion();
        }

        private void Configuracion()
        {
            Configuracion configuracion = Conversor.AbrirConfiguracionXML();
            if (configuracion != null && configuracion.sKey1 != null)
            {
                cbx_ConexionV1.SelectedValue = configuracion.sKey1;
            }

            if (configuracion != null && configuracion.sKey2 != null)
            {
                cbx_ConexionV2.SelectedValue = configuracion.sKey2;
            }
        }

        private void CargarDatos()
        {
            lstConexiones = Conversor.AbrirConexionXML();

            cbx_ConexionV1.ItemsSource = lstConexiones;
            cbx_ConexionV2.ItemsSource = lstConexiones;

            cbx_ConexionV1.Items.Refresh();
            cbx_ConexionV2.Items.Refresh();

            cbx_ConexionV1.SelectedValuePath = "sKey";
            cbx_ConexionV1.DisplayMemberPath = "sConexion";

            cbx_ConexionV2.SelectedValuePath = "sKey";
            cbx_ConexionV2.DisplayMemberPath = "sConexion";

            if (lstConexiones.Count == 0)
            {
                Msg.Info("Agregue al menos dos conexiones a base de datos para continuar.");
                Editar();
            }
        }
        private void Editar()
        {
            Conexion_MON conexion = new Conexion_MON();
            nsNavigator.Navigate(conexion);
            if (conexion.bModifico)
            {
                CargarDatos();
            }
        }

        private void BuscarVersionesTablas()
        {
            ConexionV1 = (Conexion)cbx_ConexionV1.SelectedItem;
            ConexionV2 = (Conexion)cbx_ConexionV2.SelectedItem;

            if (ConexionV1 == null)
            {
                Msg.Warning("Información Incompleta. No has seleccionado una conexión a base de datos principal.");
            }
            else if (ConexionV2 == null)
            {
                Msg.Warning("Información Incompleta. No has seleccionado una conexión a base de datos secundaria.");
            }
            else
            {
                try
                {
                    ejecutor = new Ejecutor();
                    ejecutor.ObtenerTablas(ConexionV1, ConexionV2);
                    lstVersionesTablas = ejecutor.lstVersionesTablas;

                    lstVersionesTablasBusqueda = new List<VersionesTabla>();
                    foreach (VersionesTabla versionesTabla in lstVersionesTablas)
                    {
                        lstVersionesTablasBusqueda.Add(versionesTabla);
                    }

                    dg_Tablas.ItemsSource = lstVersionesTablasBusqueda;
                    dg_Tablas.Items.Refresh();

                    if (lstVersionesTablasBusqueda.Count == 0)
                    {
                        Msg.Info("No se encontrarón tablas desactualizadas entre las bases de datos seleccionadas.");
                    }
                }
                catch (Exception ex)
                {
                    Msg.Error(ex);
                }
            }
        }

        public void AbrirColumnas()
        {
            VersionesTabla version = (VersionesTabla)dg_Tablas.SelectedItem;
            Tablas_Columnas_VISOR visor = new Tablas_Columnas_VISOR(version, ConexionV2);
            this.NavigationService.Navigate(visor);
            if (visor.bModifico)
            {
                BuscarVersionesTablas();
            }
        }

        private void btn_Editar_Click(object sender, RoutedEventArgs e)
        {
            Editar();
        }

        private void btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            BuscarVersionesTablas();
        }

        private void txt_Buscar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_AbrirV1_Click(object sender, RoutedEventArgs e)
        {
            AbrirColumnas();
        }

        private void btn_volver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}

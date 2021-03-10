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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcedureUpdater_VH.Vistas
{
    /// <summary>
    /// Lógica de interacción para Procedimientos_MON.xaml
    /// </summary>
    public partial class Procedimientos_MON : Page
    {
        private static NavigationService nsNavigator { get; } = (Application.Current.MainWindow as Principal_MAIN).frm_Principal.NavigationService;
        private List<Procedure> lstProcedimiento = new List<Procedure>();
        private List<Procedure> lstProcedimientoBusqueda = new List<Procedure>();
        private Ejecutor ejecutor = new Ejecutor();
        private List<Conexion> lstConexiones = new List<Conexion>();
        private Conexion ConexionV1 = null;
        private Conexion ConexionV2 = null;
        private Configuracion configuracion = null;
        private bool bPasosPrimer = true;

        public Procedimientos_MON()
        {
            InitializeComponent();
            
            CargarDatos();
            Configuracion();
        }

        private void Configuracion()
        {
            configuracion = Conversor.AbrirConfiguracionXML();
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
            cbx_ConexionV1.DisplayMemberPath = "BDD";

            cbx_ConexionV2.SelectedValuePath = "sKey";
            cbx_ConexionV2.DisplayMemberPath = "BDD";

            if (lstConexiones.Count == 0)
            {
                Msg.Info("Agregue al menos dos conexiones a base de datos para continuar.");
                Editar();
            }
        }

        private void CargarDatosTabla()
        {
            lstProcedimientoBusqueda = new List<Procedure>();
            foreach (Procedure procedimiento in lstProcedimiento)
            {
                lstProcedimientoBusqueda.Add(procedimiento);
            }

            dg_Procedimientos.ItemsSource = lstProcedimientoBusqueda;
            dg_Procedimientos.Items.Refresh();

            if (lstProcedimientoBusqueda.Count == 0)
            {
                lbl_Resultado.Content = "";
                Msg.Info("No se encontrarón procedimientos almacenados desactualizados entre las bases de datos seleccionadas.");
            }
            else
            {
                int nModificar = lstProcedimientoBusqueda.Count(x => !x.Modificar);
                lbl_Resultado.Content = String.Format("Por Actualizar: [{0}]   Por Agregar: [{1}]", lstProcedimientoBusqueda.Count - nModificar, nModificar);
            }
        }


        private void BuscarProcedures()
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
                    ejecutor.ObtenerProcedimientos(ConexionV1, ConexionV2);
                    lstProcedimiento = ejecutor.lstProcedimiento;
                    CargarDatosTabla();
                }
                catch (Exception ex)
                {
                    Msg.Error(ex);
                }
            }
        }

        private void BuscarProceduresPasos()
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
                
                if (bPasosPrimer)
                {
                    ejecutor = new Ejecutor();
                }

                ejecutor.ObtenerProcedimientosPasos(ConexionV1, ConexionV2, bPasosPrimer);

                if (!bPasosPrimer)
                {
                    lstProcedimiento = ejecutor.lstProcedimiento;
                    CargarDatosTabla();
                }
                else
                {
                    Msg.Success("Se concreto la obtención de datos de la primera conexión, modifica tu acceso a la segunda conexión y vuelve a buscar para completa el proceso de comparación.");
                }

                bPasosPrimer = !bPasosPrimer;
                cbx_ConexionV1.IsEnabled = bPasosPrimer;
                cbx_ConexionV2.IsEnabled = bPasosPrimer;
            }
        }

        private void Abrir()
        {
            Procedure procedure = (Procedure)dg_Procedimientos.SelectedItem;
            Procedimientos_Script_VISOR visor = new Procedimientos_Script_VISOR(procedure.Nombre, procedure.DefinicionV1, procedure.DefinicionV2, ConexionV2);
            this.NavigationService.Navigate(visor);
            if (visor.bActualizo)
            {
                int nIndice = lstProcedimiento.FindIndex(x => x.Nombre.Equals(procedure.Nombre));
                if (nIndice > -1)
                {
                    lstProcedimiento.RemoveAt(nIndice);
                    dg_Procedimientos.ItemsSource = lstProcedimiento;
                    dg_Procedimientos.Items.Refresh();

                    Buscar();
                }
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

        private void Buscar()
        {
            string sBuscar = txt_Buscar.Text;

            if (sBuscar.Length >= 3)
            {
                lstProcedimientoBusqueda = new List<Procedure>();
                foreach (Procedure procedimiento in lstProcedimiento.Where(x => x.Nombre.Contains(sBuscar)))
                {
                    lstProcedimientoBusqueda.Add(procedimiento);
                }
            }
            else if (sBuscar.Length == 0)
            {
                lstProcedimientoBusqueda = new List<Procedure>();
                foreach (Procedure procedimiento in lstProcedimiento)
                {
                    lstProcedimientoBusqueda.Add(procedimiento);
                }
            }

            dg_Procedimientos.ItemsSource = lstProcedimientoBusqueda;
            dg_Procedimientos.Items.Refresh();

            if (lstProcedimientoBusqueda.Count == 0)
            {
                lbl_Resultado.Content = "";
            }
            else
            {
                int nModificar = lstProcedimientoBusqueda.Count(x => !x.Modificar);
                lbl_Resultado.Content = String.Format("Por Actualizar: [{0}]   Por Agregar: [{1}]", lstProcedimientoBusqueda.Count - nModificar, nModificar);
            }
        }

        private void btn_AbrirV1_Click(object sender, RoutedEventArgs e)
        {
            Abrir();
        }

        private void btn_Editar_Click(object sender, RoutedEventArgs e)
        {
            Editar();
        }

        private void btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (configuracion.UsarPasos)
            {
                BuscarProceduresPasos();
            }
            else
            {
                BuscarProcedures();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Buscar();
        }

        private void btn_volver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}

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

namespace ProcedureUpdater_VH.Vistas
{
    /// <summary>
    /// Lógica de interacción para Procedimientos_MON.xaml
    /// </summary>
    public partial class Procedimientos_MON : Window
    {
        private List<Procedure> lstProcedimiento = new List<Procedure>();
        private List<Procedure> lstProcedimientoBusqueda = new List<Procedure>();
        private Ejecutor ejecutor = new Ejecutor();
        private List<Conexion> lstConexiones = new List<Conexion>();
        private Conexion ConexionV1 = null;
        private Conexion ConexionV2 = null;

        public Procedimientos_MON()
        {
            InitializeComponent();
            
            CargarDatos();
        }

        private void CargarDatos()
        {
            lstConexiones = Conversor.OpenXML();

            cbx_ConexionV1.ItemsSource = lstConexiones;
            cbx_ConexionV2.ItemsSource = lstConexiones;

            cbx_ConexionV1.Items.Refresh();
            cbx_ConexionV2.Items.Refresh();

            cbx_ConexionV1.SelectedValuePath = "BDD";
            cbx_ConexionV1.DisplayMemberPath = "BDD";

            cbx_ConexionV2.SelectedValuePath = "BDD";
            cbx_ConexionV2.DisplayMemberPath = "BDD";

            if (lstConexiones.Count == 0)
            {
                Msg.Info("Agregue al menos dos conexiones a base de datos para continuar.");
                Editar();
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
                catch (Exception ex)
                {
                    Msg.Error(ex);
                }
            }
        }

        private void Abrir()
        {
            Procedure procedure = (Procedure)dg_Procedimientos.SelectedItem;
            Script_VISOR visor = new Script_VISOR(procedure.Nombre, procedure.DefinicionV1, procedure.DefinicionV2, ConexionV2);
            visor.ShowDialog();
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
            conexion.ShowDialog();
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
            BuscarProcedures();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Buscar();
        }
    }
}

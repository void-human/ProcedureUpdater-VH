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
        }

        private void BuscarProcedures()
        {
            ConexionV1 = (Conexion)cbx_ConexionV1.SelectedItem;
            ConexionV2 = (Conexion)cbx_ConexionV2.SelectedItem;

            ejecutor.ObtenerProcedimientos(ConexionV1, ConexionV2);
            lstProcedimiento = ejecutor.lstProcedimiento;
            dg_Procedimientos.ItemsSource = lstProcedimiento.OrderBy(x => x.Nombre).ToList();
            dg_Procedimientos.Items.Refresh();
        }

        private void Abrir()
        {
            Procedure procedure = (Procedure)dg_Procedimientos.SelectedItem;
            Script_VISOR visor = new Script_VISOR(procedure.Nombre, procedure.DefinicionV1, procedure.DefinicionV2);
            visor.ShowDialog();
        }

        private void Editar()
        {
            Conexion_FORM conexion = new Conexion_FORM();
            conexion.ShowDialog();
            if (conexion.bGuardar)
            {
                CargarDatos();
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

    }
}

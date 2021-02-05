using ProcedureUpdater_VH.Metodos;
using ProcedureUpdater_VH.SQL;
using System;
using System.Collections.Generic;
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
        List<Procedure> lstProcedimiento = new List<Procedure>();
        Ejecutor ejecutor = new Ejecutor();

        public Procedimientos_MON()
        {
            InitializeComponent();
            
            //CargarDatos();
        }

        public void CargarDatos()
        {
            ejecutor.ObtenerProcedimientos(null,null);
            lstProcedimiento = ejecutor.lstProcedimiento;
            dg_Procedimientos.ItemsSource = lstProcedimiento;

        }

        public void AbrirV1()
        {
            Procedure procedure = (Procedure)dg_Procedimientos.SelectedItem;
            Script_VISOR visor = new Script_VISOR(procedure.DefinicionV1);
            visor.Show();
        }

        public void AbrirV2()
        {
            Procedure procedure = (Procedure)dg_Procedimientos.SelectedItem;
            Script_VISOR visor = new Script_VISOR(procedure.DefinicionV2);
            visor.Show();
        }



        private void btn_AbrirV1_Click(object sender, RoutedEventArgs e)
        {
            AbrirV1();
        }

        private void btn_AbrirV2_Click(object sender, RoutedEventArgs e)
        {
            AbrirV2();
        }
    }
}

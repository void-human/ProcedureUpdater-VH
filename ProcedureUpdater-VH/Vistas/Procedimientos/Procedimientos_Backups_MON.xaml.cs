using ProcedureUpdater_VH.Metodos;
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
    /// Lógica de interacción para Procedimientos_Backups_MON.xaml
    /// </summary>
    public partial class Procedimientos_Backups_MON : Page
    {
        List<RespaldoVersion> lstVersiones = null;

        public Procedimientos_Backups_MON()
        {
            InitializeComponent();
            txt_FechaFiltro.SelectedDate = DateTime.Now;
        }

        public void CargarDatos()
        {
            DateTime dtFechaFiltro = (DateTime)txt_FechaFiltro.SelectedDate;
            lstVersiones = Conversor.AbrirBackupScriptXML(dtFechaFiltro);
            dg_Historial.ItemsSource = lstVersiones;
            dg_Historial.Items.Refresh();
        }

        public void Ver()
        {
            RespaldoVersion version = (RespaldoVersion)dg_Historial.SelectedItem;
            Procedimientos_Script_VISOR visor = new Procedimientos_Script_VISOR(version.Nombre, version.ScriptV1, version.ScriptV2);
            this.NavigationService.Navigate(visor);
        }

        private void btn_Ver_Click(object sender, RoutedEventArgs e)
        {
            Ver();
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void txt_FechaaFiltro_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarDatos();
        }
    }
}

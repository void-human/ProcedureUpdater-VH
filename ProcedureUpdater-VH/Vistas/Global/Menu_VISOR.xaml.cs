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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcedureUpdater_VH.Vistas.Global
{
    /// <summary>
    /// Lógica de interacción para Menu_VISOR.xaml
    /// </summary>
    public partial class Menu_VISOR : Page
    {
        public Menu_VISOR()
        {
            InitializeComponent();
        }
        private void btn_procedimientos_Click(object sender, RoutedEventArgs e)
        {
            Procedimientos_MON mon = new Procedimientos_MON();
            this.NavigationService.Navigate(mon);
        }

        private void btn_tablas_Click(object sender, RoutedEventArgs e)
        {
            Tablas_MON mon = new Tablas_MON();
            this.NavigationService.Navigate(mon);
        }

        private void btn_respaldos_Click(object sender, RoutedEventArgs e)
        {
            Procedimientos_Backups_MON mon = new Procedimientos_Backups_MON();
            this.NavigationService.Navigate(mon);
        }

        private void btn_conexiones_Click(object sender, RoutedEventArgs e)
        {
            Conexion_MON mon = new Conexion_MON();
            this.NavigationService.Navigate(mon);
        }

        private void btn_configuracion_Click(object sender, RoutedEventArgs e)
        {
            Configuracion_FORM conf = new Configuracion_FORM();
            this.NavigationService.Navigate(conf);
        }
    }
}

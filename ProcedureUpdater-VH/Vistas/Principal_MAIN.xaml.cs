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
    /// Lógica de interacción para Principal_MAIN.xaml
    /// </summary>
    public partial class Principal_MAIN : Window
    {
        public Principal_MAIN()
        {
            InitializeComponent();
        }

        private void btn_procedimientos_Click(object sender, RoutedEventArgs e)
        {
            Procedimientos_MON mon = new Procedimientos_MON();
            mon.ShowDialog();
        }

        private void btn_tablas_Click(object sender, RoutedEventArgs e)
        {
            Tablas_MON mon = new Tablas_MON();
            mon.ShowDialog();
        }

        private void btn_respaldos_Click(object sender, RoutedEventArgs e)
        {
            Procedimientos_Backups_MON mon = new Procedimientos_Backups_MON();
            mon.ShowDialog();
        }

        private void btn_conexiones_Click(object sender, RoutedEventArgs e)
        {
            Conexion_MON mon = new Conexion_MON();
            mon.ShowDialog();
        }
    }
}

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
    /// Lógica de interacción para Tablas_Script_VISOR.xaml
    /// </summary>
    public partial class Tablas_Columnas_Script_VISOR : Window
    {
        public Tablas_Columnas_Script_VISOR(string sScript)
        {
            InitializeComponent();
            this.txt_Scripts.Text = sScript;
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

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
    /// Lógica de interacción para Script_VISOR.xaml
    /// </summary>
    public partial class Script_VISOR : Window
    {
        public Script_VISOR(string sScript)
        {
            InitializeComponent();
            txt_script.Text = sScript;
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

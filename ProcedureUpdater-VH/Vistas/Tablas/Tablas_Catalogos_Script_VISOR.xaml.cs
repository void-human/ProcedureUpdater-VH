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

namespace ProcedureUpdater_VH.Vistas.Tablas
{
    /// <summary>
    /// Lógica de interacción para Tablas_Catalogos_Script_VISOR.xaml
    /// </summary>
    public partial class Tablas_Catalogos_Script_VISOR : Window
    {
        public Tablas_Catalogos_Script_VISOR(string sTabla, List<string> lstDatos)
        {
            InitializeComponent();
            GenerarScript();

            this.Title += " " + sTabla;
        }
        
        public void GenerarScript()
        {
            string sScript = "";




            txt_Scripts.Text = sScript;
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

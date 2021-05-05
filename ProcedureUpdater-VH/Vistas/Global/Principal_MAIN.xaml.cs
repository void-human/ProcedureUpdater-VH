using ProcedureUpdater_VH.Metodos;
using ProcedureUpdater_VH.Vistas.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
            MostrarMenu();
            Version();
        }

        public void MostrarMenu()
        {
            Menu_VISOR menu = new Menu_VISOR();
            frm_Principal.Navigate(menu);
        }

        private void Version()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Title += " v" + fileVersion.FileVersion;
        }

       
    }
}

﻿using ProcedureUpdater_VH.Metodos;
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
    public partial class Procedimientos_Backups_MON : Window
    {
        List<RespaldoVersion> lstVersiones = null;

        public Procedimientos_Backups_MON()
        {
            InitializeComponent();
            CargarDatos();
        }

        public void CargarDatos()
        {
            lstVersiones = Conversor.OpenBackupScriptXML();
            dg_Historial.ItemsSource = lstVersiones;
            dg_Historial.Items.Refresh();
        }

        public void Ver()
        {
            RespaldoVersion version = (RespaldoVersion)dg_Historial.SelectedItem;
            Procedimientos_Script_VISOR visor = new Procedimientos_Script_VISOR(version.Nombre, version.ScriptV1, version.ScriptV2);
            visor.ShowDialog();
        }

        private void btn_Ver_Click(object sender, RoutedEventArgs e)
        {
            Ver();
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

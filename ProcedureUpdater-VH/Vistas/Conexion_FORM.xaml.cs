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
    /// Lógica de interacción para Conexion_FORM.xaml
    /// </summary>
    public partial class Conexion_FORM : Window
    {
        public Conexion_FORM()
        {
            InitializeComponent();

            Conversor.OpenXML();
        }


        public void Guardar()
        {
            Conexion conexion = new Conexion();
            conexion.BDD = txt_BDD.Text;
            conexion.Contrasena = txt_Contrasena.Text;
            conexion.IP = txt_IP.Text;
            conexion.Usuario = txt_Usuario.Text;

            Conversor.GuardarConexion(conexion);
        }

        private void btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            Guardar();
        }
    }
}

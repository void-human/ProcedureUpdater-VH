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

        public bool bGuardar = false;

        private Conexion conexion = null;

        public Conexion_FORM(Conexion conexion = null)
        {
            InitializeComponent();

            if (conexion != null)
            {
                this.conexion = conexion;
                txt_BDD.Text = conexion.BDD;
                txt_Contrasena.Text = getContrasena(conexion.Contrasena);
                txt_IP.Text = conexion.IP;
                txt_Usuario.Text = conexion.Usuario;
            }
        }

        private string getContrasena(string sContrasena, string sNuevaContrasena = "")
        {
            //Asi evitamos mostrar la contraseña actual de la base de datos.
            if (sNuevaContrasena.Length == sContrasena.Length)
            {
                return sNuevaContrasena;
            }
            else
            {
                sNuevaContrasena += "*";
                return getContrasena(sContrasena, sNuevaContrasena);
            }

        public Conexion_FORM()
        {
            InitializeComponent();

            Conversor.OpenXML();
        }

        public void Guardar()
        {
            bool bRespuesta = Msg.Confirm("¿Estas seguro de que deseas guardar esta conexión?");
            if (bRespuesta)
            {
                Conexion NuevaConexion = new Conexion();
                NuevaConexion.BDD = txt_BDD.Text;
                NuevaConexion.Contrasena = txt_Contrasena.Text;
                NuevaConexion.IP = txt_IP.Text;
                NuevaConexion.Usuario = txt_Usuario.Text;
                NuevaConexion.sKey = "";

                //¿Estamos editando?
                if (conexion != null)
                {
                    //Traemos la ruta de la conexión
                    NuevaConexion.sKey = conexion.sKey;

                    //Si se estaba editando una conexión existente y no se modifico la contraseña entonces traemos de vuelta la contraseña original.
                    if (NuevaConexion.Contrasena.Equals(getContrasena(conexion.Contrasena)))
                    {
                        NuevaConexion.Contrasena = conexion.Contrasena;
                    }
                }

                bGuardar = Conversor.GuardarConexion(NuevaConexion);
                if (bGuardar)
                {
                    Msg.Success("Correcto. La conexión se guardo/actualizo correctamente.");
                    this.Close();
                }
            }
        }

        private void btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            Guardar();
        }
    }
}

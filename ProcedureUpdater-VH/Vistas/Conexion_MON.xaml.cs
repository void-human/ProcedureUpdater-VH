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
    /// Lógica de interacción para Conexion_MON.xaml
    /// </summary>
    public partial class Conexion_MON : Window
    {
        public bool bModifico = false;
        private List<Conexion> lstConexiones = new List<Conexion>();

        public Conexion_MON()
        {
            InitializeComponent();
            CargarConexiones();
        }

        public void CargarConexiones()
        {
            lstConexiones = Conversor.OpenConexionXML();
            dg_Conexiones.ItemsSource = lstConexiones;
            dg_Conexiones.Items.Refresh();
        }

        public void Modificar()
        {
            Conexion conexion = (Conexion)dg_Conexiones.SelectedItem;
            Conexion_FORM conModificar = new Conexion_FORM(conexion);
            conModificar.ShowDialog();
            if (conModificar.bGuardar)
            {
                bModifico = true;
                CargarConexiones();
            }
        }

        public void Agregar()
        {
            Conexion_FORM conAgregar = new Conexion_FORM();
            conAgregar.ShowDialog();
            if (conAgregar.bGuardar)
            {
                bModifico = true;
                CargarConexiones();
            }
        }

        public void Eliminar()
        {
            Conexion conexion = (Conexion)dg_Conexiones.SelectedItem;
            if (conexion != null)
            {
                bool bRespuesta = Msg.Confirm("¿Estas seguro de que deseas eliminar esta conexión?");
                if (bRespuesta)
                {
                    try
                    {
                        if (Conversor.EliminarConexionXML(conexion.sKey))
                        {
                            bModifico = true;
                            Msg.Success("Correcto. La conexión se elimino correctamente.");
                            CargarConexiones();
                        }
                    }
                    catch (Exception ex)
                    {
                        Msg.Error(ex);
                    }

                }
            }
            else
            {
                Msg.Warning("Información Incompleta. No se ha seleccionado una conexión a eliminar.");
            }

        }

        private void btn_Modificar_Click(object sender, RoutedEventArgs e)
        {
            Modificar();
        }

        private void btn_Eliminar_Click(object sender, RoutedEventArgs e)
        {
            Eliminar();
        }

        private void btn_Agregar_Click(object sender, RoutedEventArgs e)
        {
            Agregar();
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

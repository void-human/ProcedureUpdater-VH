using Microsoft.Win32;
using ProcedureUpdater_VH.Metodos;
using System;
using System.IO;
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

namespace ProcedureUpdater_VH.Vistas
{
    /// <summary>
    /// Lógica de interacción para Configuracion_FORM.xaml
    /// </summary>
    public partial class Configuracion_FORM : Page
    {
        private Configuracion configuracion = null;
        private bool bCambios = false;
        private List<Conexion> lstConexiones = null;

        public Configuracion_FORM()
        {
            InitializeComponent();
            this.configuracion = Conversor.AbrirConfiguracionXML();
            txt_Directorio.Text = configuracion.Direccion;
            cbx_usar_directorio.IsChecked = configuracion.UsarDireccion;
            CargarConexiones();
        }

        public void CargarConexiones()
        {
            lstConexiones = Conversor.AbrirConexionXML();
            cbx_ConexionV1.ItemsSource = lstConexiones;
            cbx_ConexionV2.ItemsSource = lstConexiones;
            
            cbx_ConexionV1.DisplayMemberPath = "BDD";
            cbx_ConexionV1.SelectedValuePath = "sKey";
            
            cbx_ConexionV2.DisplayMemberPath = "BDD";
            cbx_ConexionV2.SelectedValuePath = "sKey";

            cbx_ConexionV1.Items.Refresh();
            cbx_ConexionV2.Items.Refresh();

            if (configuracion.sKey1 != null)
            {
                cbx_ConexionV1.SelectedValue = configuracion.sKey1;
            }
            else
            {
                configuracion.sKey1 = "";
            }

            if (configuracion.sKey2 != null)
            {
                cbx_ConexionV2.SelectedValue = configuracion.sKey2;
            }
            else
            {
                configuracion.sKey2 = "";
            }

        }

        private void BuscarDirectorio()
        {
            SaveFileDialog sfdDirectorio = new SaveFileDialog();
            sfdDirectorio.Filter = "Archivos SQL (*.sql)|*.sql";
            sfdDirectorio.FileName = "Script.sql";
            sfdDirectorio.ShowDialog();

            sfdDirectorio.FileName = sfdDirectorio.FileName.Replace("Script.sql","");
            
            if (!sfdDirectorio.FileName.Equals(""))
            {
                configuracion.Direccion = sfdDirectorio.FileName;
                txt_Directorio.Text = configuracion.Direccion;
                bCambios = true;
            }
            else
            {
                if (Msg.Confirm("No se ha seleccionado ningún directorio, ¿Deseas limpiar el directorio?"))
                {
                    configuracion.Direccion = "";
                    txt_Directorio.Text = configuracion.Direccion;
                    bCambios = true;
                }
            }
        }

        private void Guardar()
        {
            if (bCambios 
                || configuracion.UsarDireccion != (bool)cbx_usar_directorio.IsChecked 
                || (cbx_ConexionV1.SelectedValue != null && !configuracion.sKey1.Equals((string)cbx_ConexionV1.SelectedValue)) 
                || (cbx_ConexionV2.SelectedValue != null && !configuracion.sKey2.Equals((string)cbx_ConexionV2.SelectedValue)))
            {
                try
                {
                    if (Msg.Confirm("¿Deseas guardar los cambios realizados?"))
                    {
                        configuracion.sKey1 = (string) cbx_ConexionV1.SelectedValue;
                        configuracion.sKey2 = (string) cbx_ConexionV2.SelectedValue;
                        configuracion.UsarDireccion = (bool)cbx_usar_directorio.IsChecked;
                        Conversor.GuardarConfiguracion(this.configuracion);
                        Msg.Success("Correcto. Los cambios se guardarón correctamente.");
                    }
                }
                catch (Exception e)
                {
                    Msg.Error(e);
                }
            }
        }

        private void Volver() //Volver
        {
            Guardar();
            this.NavigationService.GoBack();
        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Volver();
        }

        private void btn_Seleccionar_Click(object sender, RoutedEventArgs e)
        {
            BuscarDirectorio();
        }
    }
}

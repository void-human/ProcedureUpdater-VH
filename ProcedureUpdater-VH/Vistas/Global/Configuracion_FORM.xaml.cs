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

        public Configuracion_FORM()
        {
            InitializeComponent();
            this.configuracion = Conversor.AbrirConfiguracionXML();
            txt_Directorio.Text = configuracion.Direccion;
            cbx_usar_directorio.IsChecked = configuracion.UsarDireccion;
        }

        private void BuscarDirectorio()
        {
            SaveFileDialog sfdDirectorio = new SaveFileDialog();
            sfdDirectorio.Filter = "Archivos SQL (*.sql)|*.sql";
            sfdDirectorio.FileName = "Script.sql";
            sfdDirectorio.ShowDialog();

            if (sfdDirectorio.FileName != String.Empty)
            {
                configuracion.Direccion = sfdDirectorio.FileName;
                txt_Directorio.Text = configuracion.Direccion;
            }
            else
            {
                if (Msg.Confirm("No se ha seleccionado ningún directorio, ¿Deseas limpiar el directorio?"))
                {
                    configuracion.Direccion = "";
                    txt_Directorio.Text = configuracion.Direccion;
                }
            }
        }

        private void Guardar()
        {
            try
            {
                if (Msg.Confirm("¿Estas seguro de que deseas guardar los cambios?"))
                {
                    configuracion.BDD = "";
                    configuracion.IP = "";
                    configuracion.UsarDireccion = (bool)cbx_usar_directorio.IsChecked;
                    Conversor.GuardarConfiguracion(this.configuracion);
                }
            }
            catch (Exception e)
            {
                Msg.Error(e);
            }
        }

        private void Cancelar() //Volver
        {

        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            Guardar();
        }

        private void btn_Seleccionar_Click(object sender, RoutedEventArgs e)
        {
            BuscarDirectorio();
        }
    }
}

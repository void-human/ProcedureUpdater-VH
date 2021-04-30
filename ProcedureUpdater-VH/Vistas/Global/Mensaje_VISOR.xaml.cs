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
    /// Lógica de interacción para Mensaje_VISOR.xaml
    /// </summary>
    public partial class Mensaje_VISOR : Window
    {
        public bool bRespuesta = true;

        public Mensaje_VISOR(string sMensaje, string sTipo)
        {
            InitializeComponent();

            txt_Mensaje.Text = sMensaje;

            switch (sTipo)
            {
                case "Error":
                    stkp_Titulo.Background = Msg.getColor(Colores.Rojo);
                    this.BorderBrush = Msg.getColor(Colores.Rojo);
                    lbl_Titulo.Content = "Error";
                    btn_Cancelar.Visibility = Visibility.Collapsed;
                    btn_Aceptar.Background = Msg.getColor(Colores.Negro);
                    break;
                case "Warning":
                    stkp_Titulo.Background = Msg.getColor(Colores.Naranja);
                    this.BorderBrush = Msg.getColor(Colores.Naranja);
                    lbl_Titulo.Content = "Advertencia";
                    btn_Cancelar.Visibility = Visibility.Collapsed;
                    btn_Aceptar.Background = Msg.getColor(Colores.Negro);
                    break;
                case "Info":
                    stkp_Titulo.Background = Msg.getColor(Colores.Azul);
                    this.BorderBrush = Msg.getColor(Colores.Azul);
                    lbl_Titulo.Content = "Información";
                    btn_Cancelar.Visibility = Visibility.Collapsed;
                    btn_Aceptar.Background = Msg.getColor(Colores.Negro);
                    break;
                case "Success":
                    stkp_Titulo.Background = Msg.getColor(Colores.Verde);
                    this.BorderBrush = Msg.getColor(Colores.Verde);
                    lbl_Titulo.Content = "Completado";
                    btn_Cancelar.Visibility = Visibility.Collapsed;
                    break;
                case "Confirm":
                    stkp_Titulo.Background = Msg.getColor(Colores.GrisOscuro);
                    this.BorderBrush = Msg.getColor(Colores.GrisOscuro);
                    lbl_Titulo.Content = "Confirmar";
                    break;
            }

            FocusManager.SetFocusedElement(this, btn_Aceptar);
        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            bRespuesta = false;
            Close();
        }

        private void btn_Aceptar_Click(object sender, RoutedEventArgs e)
        {
            bRespuesta = true;
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch
            {

            }
        }

        private void btn_Aceptar_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Escape))
            {
                bRespuesta = false;
                Close();
            }
            else if (e.Key.Equals(Key.Enter))
            {
                bRespuesta = true;
                Close();
            }
        }
    }
}

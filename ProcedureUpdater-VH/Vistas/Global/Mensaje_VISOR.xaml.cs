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
                    stkp_Titulo.Background = getColor("#782020");
                    lbl_Titulo.Content = "Error";
                    btn_Cancelar.Visibility = Visibility.Collapsed;
                    btn_Aceptar.Background = getColor("#000000");
                    break;
                case "Warning":
                    stkp_Titulo.Background = getColor("#B16806");
                    lbl_Titulo.Content = "Advertencia";
                    btn_Cancelar.Visibility = Visibility.Collapsed;
                    btn_Aceptar.Background = getColor("#000000");
                    break;
                case "Info":
                    stkp_Titulo.Background = getColor("#0F3E8D");
                    lbl_Titulo.Content = "Información";
                    btn_Cancelar.Visibility = Visibility.Collapsed;
                    btn_Aceptar.Background = getColor("#000000");
                    break;
                case "Success":
                    stkp_Titulo.Background = getColor("#137E04");
                    lbl_Titulo.Content = "Completado";
                    btn_Cancelar.Visibility = Visibility.Collapsed;
                    break;
                case "Confirm":
                    stkp_Titulo.Background = getColor("#2D302C");
                    lbl_Titulo.Content = "Confirmar";
                    break;
            }

            FocusManager.SetFocusedElement(this, btn_Aceptar);
        }

        private Brush getColor(string sColor)
        {
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString(sColor);
            return brush;
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

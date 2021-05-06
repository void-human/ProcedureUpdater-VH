using ProcedureUpdater_VH.Vistas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ProcedureUpdater_VH.Metodos
{
    public abstract class Msg
    {

        private static List<Popup> lstppToast = new List<Popup>();

        public static void Error(string sMensaje)
        {
            Mensaje_VISOR msg = new Mensaje_VISOR(sMensaje,"Error");
            msg.ShowDialog();
        }

        public static void Error(Exception ex)
        {
            Mensaje_VISOR msg = new Mensaje_VISOR(ex.Message, "Error");
            msg.ShowDialog();
            //Process.Start(AppDomain.CurrentDomain.BaseDirectory + "VersionUpdater.exe");
        }

        public static void Warning(string sMensaje)
        {
            Mensaje_VISOR msg = new Mensaje_VISOR(sMensaje, "Warning");
            msg.ShowDialog();
        }
        public static void Info(string sMensaje)
        {
            Mensaje_VISOR msg = new Mensaje_VISOR(sMensaje, "Info");
            msg.ShowDialog();
        }

        public static void Success(string sMensaje)
        {
            Mensaje_VISOR msg = new Mensaje_VISOR(sMensaje, "Success");
            msg.ShowDialog();
        }

        public static bool Confirm(string sMensaje)
        {
            Mensaje_VISOR msg = new Mensaje_VISOR(sMensaje, "Confirm");
            msg.ShowDialog();
            bool bRespuesta = msg.bRespuesta;
            return bRespuesta;
        }

        public static async Task Toast(string sMensaje, Colores color, int nSegundos = 3)
        {
            TextBlock txb_Mensaje = new TextBlock();
            txb_Mensaje.Text = sMensaje;
            txb_Mensaje.Foreground = getColor("White");
            txb_Mensaje.VerticalAlignment = VerticalAlignment.Center;
            txb_Mensaje.HorizontalAlignment = HorizontalAlignment.Center;
            txb_Mensaje.FontSize = 16;
            txb_Mensaje.FontWeight = FontWeights.Light;

            Grid gd_Contenedor = new Grid();
            gd_Contenedor.Background = getColor(color);
            gd_Contenedor.Children.Add(txb_Mensaje);

            int nIndice = lstppToast.Count;
            lstppToast.Add(new Popup());
            lstppToast[nIndice].Width = ((Principal_MAIN)((App)App.Current).MainWindow).Width;
            lstppToast[nIndice].Height = 45;
            lstppToast[nIndice].Child = gd_Contenedor;

            lstppToast[nIndice].PopupAnimation = PopupAnimation.Fade;
            lstppToast[nIndice].IsOpen = true;

            if (nIndice > 0 && lstppToast[nIndice-1].IsOpen && false) 
            {
                // Con false fuerzo a no realizar la comprobación, continuamos en desarrollo para apilar 
                // los mensajes Toast uno debajo de otro en caso de que estan abiertos.

                lstppToast[nIndice].PlacementTarget = lstppToast[nIndice-1];
                lstppToast[nIndice].Placement = PlacementMode.Center;
            }
            else
            {
                lstppToast[nIndice].PlacementTarget = ((Principal_MAIN)((App)App.Current).MainWindow).frm_Principal;
                lstppToast[nIndice].Placement = PlacementMode.Center;
            }

            await Task.Run(() => {
                Thread.Sleep((nSegundos * 1000));
            });

            lstppToast[nIndice].IsOpen = false;
        }

        public static Brush getColor(Colores color)
        {
            switch (color)
            {
                case Colores.Verde:
                    return getColor("#137E04");
                case Colores.Azul:
                    return getColor("#0F3E8D");
                case Colores.Naranja:
                    return getColor("#B16806");
                case Colores.Rojo:
                    return getColor("#782020");
                case Colores.Blanco:
                    return getColor("White");
                case Colores.Negro:
                    return getColor("Black");
                case Colores.GrisOscuro:
                    return getColor("#2D302C");
                case Colores.VerdeClaro:
                    return getColor("#DBF4D8");
                case Colores.RojoClaro:
                    return getColor("#FFC3C2");
                default:
                    return getColor("Transparent");
            }
        }

        private static Brush getColor(string sNombre)
        {
            return (Brush)new BrushConverter().ConvertFromString(sNombre);
        }
        
    }

    public enum Colores
    {
        Verde,
        VerdeClaro,
        Azul,
        Naranja,
        Rojo,
        RojoClaro,
        Blanco,
        Negro,
        GrisOscuro
    }
}

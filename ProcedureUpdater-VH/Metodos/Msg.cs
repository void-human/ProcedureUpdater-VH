using ProcedureUpdater_VH.Vistas;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcedureUpdater_VH.Metodos
{
    public abstract class Msg
    {
        public static void Error(string sMensaje)
        {
            Mensaje_VISOR msg = new Mensaje_VISOR(sMensaje,"Error");
            msg.ShowDialog();
        }

        public static void Error(Exception ex)
        {
            Mensaje_VISOR msg = new Mensaje_VISOR(ex.Message, "Error");
            msg.ShowDialog();
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

    }
}

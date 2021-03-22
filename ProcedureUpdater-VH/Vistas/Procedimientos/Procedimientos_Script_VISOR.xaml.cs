using ProcedureUpdater_VH.Metodos;
using ProcedureUpdater_VH.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Lógica de interacción para Script_VISOR.xaml
    /// </summary>
    public partial class Procedimientos_Script_VISOR : Page
    {
        private string sScriptV1;
        private string sScriptV2;
        private string sProcedure;
        private List<Procedimiento> lstScripts = new List<Procedimiento>();
        private Conexion ConexionV2;
        public bool bActualizo;
        private string sPath = "";
        private bool bUsarDireccion = false;

        public Procedimientos_Script_VISOR(string sProcedure, string sScriptV1, string sScriptV2, Conexion ConexionV2 = null)
        {
            InitializeComponent();

            bActualizo = false;


            if (sScriptV1 == null)
            {
                sScriptV1 = "";
            }

            if (sScriptV2 == null)
            {
                sScriptV2 = "";
            }
            else
            {
                sScriptV1 = sScriptV1.Replace("CREATE PROCEDURE", "ALTER PROCEDURE");
                sScriptV2 = sScriptV2.Replace("CREATE PROCEDURE", "ALTER PROCEDURE");
            }


            this.Title = "Procedimiento Almacenado: "+sProcedure;
            this.lbl_Titulo.Content += sProcedure;

            this.sScriptV1 = sScriptV1;
            this.sScriptV2 = sScriptV2;
            this.sProcedure = sProcedure;

            this.ConexionV2 = ConexionV2;

            if (ConexionV2 == null)
            {
                btn_Actualizar.Visibility = Visibility.Hidden;
            }
            Convertir();
            Configuracion();
        }

        private void Configuracion()
        {
            Configuracion configuracion = Conversor.AbrirConfiguracionXML();
            if (configuracion != null)
            {
                sPath = configuracion.Direccion;
                bUsarDireccion = configuracion.UsarDireccion;
            }
        }

        public void Convertir()
        {
            string[] arrsLineasV1 = sScriptV1.Split("\r\n");
            string[] arrsLineasV2 = sScriptV2.Split("\r\n");

            if (arrsLineasV1.Length == 1)
            {
                arrsLineasV1 = sScriptV1.Split("\n");
                arrsLineasV2 = sScriptV2.Split("\n");
            }

            int nCantidad1 = arrsLineasV1.Count();
            int nCantidad2 = arrsLineasV2.Count();
            int nCantidadMaxima = nCantidad2;
            if (nCantidad1 >= nCantidad2)
            {
                nCantidadMaxima = nCantidad1;
            }

            for (int i = 0; i < nCantidadMaxima; i++)
            {
                string sLineaV1 = "";
                string sLineaV2 = "";

                if (arrsLineasV1.Count() > i)
                {
                    sLineaV1 = arrsLineasV1[i];
                }

                if (arrsLineasV2.Count() > i)
                {
                    sLineaV2 = arrsLineasV2[i];
                }

                lstScripts.Add(new Procedimiento()
                {
                    Indice = (i + 1),
                    v1 = sLineaV1,
                    v2 = sLineaV2,
                    bNuevo = !arrsLineasV2.Contains(sLineaV1), 
                    bRemovido = !arrsLineasV1.Contains(sLineaV2)
                });
            }

            dg_Scripts.ItemsSource = lstScripts;
            dg_Scripts.Items.Refresh();
        }

        public void Actualizar(bool bValidar = false)
        {
            if (bValidar || Msg.Confirm("¿Estas seguro de que deseas actualizar este script?"))
            {
                try
                {
                    Ejecutor ejecutor = new Ejecutor();
                    bool bRespuesta = ejecutor.ActualizarConexion(ConexionV2, sScriptV1);
                    bActualizo = true;
                    if (bRespuesta)
                    {
                        RespaldoVersion version = new RespaldoVersion();
                        version.BDD = ConexionV2.BDD;
                        version.IP = ConexionV2.IP;
                        version.Nombre = sProcedure;
                        version.dtActualizacion = DateTime.Now;
                        version.ScriptV1 = sScriptV1;
                        version.ScriptV2 = sScriptV2;
                        version.sKey = "";
                        Conversor.GuardarBackupScript(version);

                        if(!bValidar)
                        Msg.Success(String.Format("Correcto. El Script se actualizo en la base de datos {0}", ConexionV2.BDD));

                        if (!bUsarDireccion)
                        {
                            bRespuesta = Msg.Confirm("¿Deseas generar/guardar un documento \"*.SQL\" con el código actualizado?");
                            if (bRespuesta)
                            {
                                Conversor.GuardarSQL(sProcedure, sScriptV1, sPath, bUsarDireccion);
                            }
                        }
                        else
                        {
                            Conversor.GuardarSQL(sProcedure, sScriptV1, sPath, bUsarDireccion);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Msg.Error(ex);
                }
            }
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btn_Actualizar_Click(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }
    }
}

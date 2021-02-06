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
    public partial class Script_VISOR : Window
    {
        private string sScriptV1;
        private string sScriptV2;
        private List<Procedimiento> lstScripts = new List<Procedimiento>();

        public Script_VISOR(string sProcedure, string sScriptV1, string sScriptV2)
        {
            InitializeComponent();

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


            this.Title = sProcedure;

            this.sScriptV1 = sScriptV1;
            this.sScriptV2 = sScriptV2;
            Convertir();

        }

        public void Convertir()
        {
            string[] arrsLineasV1 = sScriptV1.Split("\r\n");
            string[] arrsLineasV2 = sScriptV2.Split("\r\n");

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

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       public class Procedimiento
       {
            public int Indice { set; get; }
            public string v1 { set; get; }
            public string v2 { set; get; }
            public string sColor { set; get; }
            public bool bNuevo { set; get; }
            public bool bRemovido { set; get; }
       }
    }
}

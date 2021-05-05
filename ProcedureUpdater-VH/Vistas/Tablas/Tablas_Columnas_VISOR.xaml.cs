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
    /// Lógica de interacción para Columnas_VISOR.xaml
    /// </summary>
    public partial class Tablas_Columnas_VISOR : Page
    {
        private List<VersionesColumna> lstVersiones;
        private VersionesTabla vtTabla;
        private Conexion ConexionV2 = null;
        public bool bModifico = false;


        public Tablas_Columnas_VISOR(VersionesTabla vtTabla, Conexion ConexionV2)
        {
            InitializeComponent();
            this.Title = vtTabla.TablaV1.Nombre;
            this.ConexionV2 = ConexionV2;
            this.vtTabla = vtTabla;
            Comparar();
        }

        public void Comparar()
        {
            lstVersiones = new List<VersionesColumna>();
            string[] arrsColumnasV1 = vtTabla.TablaV1.sScripts.Split((char)13);
            string[] arrsColumnasV2 = vtTabla.TablaV2.sScripts.Split((char)13);

            int nMaximoV1 = arrsColumnasV1.Length;
            int nMaximoV2 = arrsColumnasV2.Length;
            int nMaximo = nMaximoV1;

            if (nMaximoV2 > nMaximo)
            {
                nMaximo = nMaximoV2;
            }

            for (int i = 0; i < nMaximo; i++)
            {
                string CompletoV1 = "";
                string CompletoV2 = "";
                bool bModificacion = false;
                bool bNuevo = false;
                bool bRemovido = false;
                bool bModificacionV2 = false;

                if (arrsColumnasV1.Length > i)
                {
                    CompletoV1 = arrsColumnasV1[i];
                    bNuevo = !arrsColumnasV2.ToList().Exists(x => x.Equals(CompletoV1) );
                    bModificacion = !arrsColumnasV2.ToList().Exists(x => x.Equals(CompletoV1) ) && !bNuevo;
                }

                if (arrsColumnasV2.Length > i)
                {
                    CompletoV2 = arrsColumnasV2[i];
                    bRemovido = !arrsColumnasV1.ToList().Exists(x => x.Equals(CompletoV2));
                }

                lstVersiones.Add(new VersionesColumna
                {
                    Indice = (i + 1),
                    CompletoV1 = CompletoV1,
                    CompletoV2 = CompletoV2,
                    bModificacion = bModificacion,
                    bRemovido = bRemovido,
                    bNuevo = bNuevo,
                    bModificacionV2 = bModificacionV2
                });
            }

            for (int i = 0; i < lstVersiones.Count; i++)
            {
                string sNombreV2 = lstVersiones[i].CompletoV2;

                if (!sNombreV2.Equals(""))
                {
                    sNombreV2 = sNombreV2.Split(" ")[0];
                    lstVersiones[i].bModificacionV2 = lstVersiones.Exists(x => !x.CompletoV1.Equals("") && x.CompletoV1.Split(" ")[0].Equals(sNombreV2) && x.bModificacion);
                }
            }

            dg_Scripts.ItemsSource = lstVersiones;
            dg_Scripts.Items.Refresh();
        }

        private void btn_Actualizar_Click(object sender, RoutedEventArgs e)
        {
            string sScriptCreate = vtTabla.TablaV1.sScripts;
            string sScriptAlter = "";
            foreach (VersionesColumna columna in lstVersiones.Where(x => x.bNuevo || x.bModificacion || x.bRemovido))
            {
                if (columna.bNuevo)
                {
                    sScriptAlter += String.Format("ALTER TABLE {0} ADD {1} \nGO\n\n", vtTabla.TablaV1.Nombre, columna.CompletoV1.Replace("  , ",""));
                }
                else if (columna.bModificacion)
                {
                    sScriptAlter += String.Format("ALTER TABLE {0} ALTER COLUMN {1} \nGO\n\n", vtTabla.TablaV1.Nombre, columna.CompletoV1.Replace("  , ", ""));
                }
                
                if (columna.bRemovido && !columna.bModificacionV2)
                {
                    sScriptAlter += String.Format("ALTER TABLE {0} DROP COLUMN {1} \nGO\n\n", vtTabla.TablaV1.Nombre, columna.CompletoV2.Split(" ")[0].Replace("  , ", ""));
                }
            }

            sScriptCreate += "\n\r\n\r\n\r" + sScriptAlter;

            Tablas_Columnas_Script_VISOR visor = new Tablas_Columnas_Script_VISOR(sScriptCreate);
            visor.ShowDialog();
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}

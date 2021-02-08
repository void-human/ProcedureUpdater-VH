using ProcedureUpdater_VH.Metodos;
using ProcedureUpdater_VH.SQL;
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
    /// Lógica de interacción para Columnas_VISOR.xaml
    /// </summary>
    public partial class Columnas_VISOR : Window
    {
        private List<Columna> lstColumnasV1 = null;
        private List<Columna> lstColumnasV2 = null;
        private List<VersionesColumna> lstColumnas = null;
        private string Tabla = "";
        private Conexion ConexionV2 = null;
        public bool bModifico = false;


        public Columnas_VISOR(string Tabla, List<Columna> lstColumnasV1, List<Columna> lstColumnasV2, Conexion ConexionV2)
        {
            InitializeComponent();
            this.Title = Tabla;
            this.lstColumnasV1 = lstColumnasV1;
            this.lstColumnasV2 = lstColumnasV2;
            this.ConexionV2 = ConexionV2;
            this.Tabla = Tabla;
            Comparar();
        }

        public void Comparar()
        {
            lstColumnas = new List<VersionesColumna>();

            int nMaximoV1 = lstColumnasV1.Count;
            int nMaximoV2 = lstColumnasV2.Count;
            int nMaximo = nMaximoV1;

            if (nMaximoV2 > nMaximo)
            {
                nMaximo = nMaximoV2;
            }

            for (int i = 0; i < nMaximo; i++)
            {
                string CompletoV1 = "";
                string CompletoV2 = "";
                bool bModificado = false;
                bool bNuevo = false;
                bool bRemovido = false;

                if (lstColumnasV1.Count > i)
                {
                    CompletoV1 = lstColumnasV1[i].Completo;
                    bNuevo = !lstColumnasV2.Exists(x => x.Nombre.Equals(lstColumnasV1[i].Nombre));
                    bModificado = !lstColumnasV2.Exists(x => x.Completo.Equals(CompletoV1)) && !bNuevo;
                }

                if (lstColumnasV2.Count > i)
                {
                    CompletoV2 = lstColumnasV2[i].Completo;
                    bRemovido = !lstColumnasV1.Exists(x => x.Completo.Equals(CompletoV2));
                }

                lstColumnas.Add(new VersionesColumna
                {
                    Indice = (i + 1),
                    CompletoV1 = CompletoV1,
                    CompletoV2 = CompletoV2,
                    bModificacion = bModificado,
                    bRemovido = bRemovido,
                    bNuevo = bNuevo
                });
            }

            dg_Scripts.ItemsSource = lstColumnas;
            dg_Scripts.Items.Refresh();
        }

        private void btn_Actualizar_Click(object sender, RoutedEventArgs e)
        {
            Scripts sql = new Scripts();
            List<string> lstPropiedades = new List<string>();
            foreach (Columna columna in lstColumnasV1)
            {
                lstPropiedades.Add(columna.Completo);
            }

            sql.getCreateTables(Tabla, lstPropiedades.ToArray());
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

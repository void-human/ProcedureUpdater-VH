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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcedureUpdater_VH.Vistas.Tablas
{
    /// <summary>
    /// Lógica de interacción para Tablas_Catalogos_Configuracion_MON.xaml
    /// </summary>
    public partial class Tablas_Catalogos_Configuracion_MON : Page
    {

        private Ejecutor ejecutor = new Ejecutor();
        private TablaCatalogo objTablaCatalogo = new TablaCatalogo();
        private List<Catalogo> lstCatologosBusqueda = new List<Catalogo>();
        private Conexion ConexionV1;

        public Tablas_Catalogos_Configuracion_MON()
        {
            InitializeComponent();

            CargarDatos();
            Configuracion();
        }

        private void Configuracion()
        {
            Configuracion configuracion = Conversor.AbrirConfiguracionXML();
            if (configuracion != null && configuracion.sKey1 != null)
            {
                cbx_ConexionV1.SelectedValue = configuracion.sKey1;
            }
        }

        private void CargarDatos()
        {
            List<Conexion> lstConexiones = Conversor.AbrirConexionXML();

            cbx_ConexionV1.ItemsSource = lstConexiones;

            cbx_ConexionV1.Items.Refresh();

            cbx_ConexionV1.SelectedValuePath = "sKey";
            cbx_ConexionV1.DisplayMemberPath = "sConexion";

            if (lstConexiones.Count == 0)
            {
                Msg.Info("Agregue al menos dos conexiones a base de datos para continuar.");
            }
        }


        private void GuardarConfiguracion()
        {
            try
            {
                if (objTablaCatalogo == null || objTablaCatalogo.lstCatalogos == null || objTablaCatalogo.lstCatalogos.Count == 0)
                {
                    Msg.Warning("Información Incompleta. No puedes guardar la configuración de catalogos de esta conexión porque no existen registros.");
                }
                else
                {
                    if (Msg.Confirm("¿Estás seguro de que deseas guardar los cambios realizados en esta configuración de catalogos a la conexión actual?"))
                    {
                        Conversor.GuardarTablaCatalogoXML(objTablaCatalogo);
                        Msg.Success(string.Format("Correcto. La configuración de catalogos para la conexión {0} se guardó correctamente.", ConexionV1.sConexion));
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        private void Buscar()
        {
            try
            {
                if (objTablaCatalogo != null && objTablaCatalogo.lstCatalogos != null)
                {
                    string sBuscar = txt_Buscar.Text;
                    lstCatologosBusqueda = new List<Catalogo>();
                    if (sBuscar.Replace(" ", "").Length == 0)
                    {
                        foreach (Catalogo catalogo in objTablaCatalogo.lstCatalogos)
                        {
                            lstCatologosBusqueda.Add(catalogo);
                        }
                    }
                    else
                    {
                        foreach (Catalogo catalogo in objTablaCatalogo.lstCatalogos.Where(x => x.nombre.Contains(sBuscar)))
                        {
                            lstCatologosBusqueda.Add(catalogo);
                        }
                    }

                    dg_Tablas.ItemsSource = lstCatologosBusqueda;
                    dg_Tablas.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        private void BuscarCatalogos()
        {
            try
            {
                ConexionV1 = (Conexion)cbx_ConexionV1.SelectedItem;
                if (ConexionV1 == null)
                {
                    Msg.Warning("Información Incompleta. No has seleccionado una conexión a base de datos principal.");
                }
                else
                {
                    string sBusqueda = txt_Buscar.Text;
                    objTablaCatalogo = ejecutor.ObtenerTablasCatalogos(ConexionV1, sBusqueda);
                    Buscar();
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        private void TipoCatalogo()
        {
            try
            {
                Catalogo catalogo = (Catalogo)dg_Tablas.SelectedItem;
                int nIndice = objTablaCatalogo.lstCatalogos.FindIndex(x => x.nombre.Equals(catalogo.nombre));
                if (nIndice != -1)
                {
                    objTablaCatalogo.lstCatalogos[nIndice].catalogo = !catalogo.catalogo;
                }
                else
                {
                    throw new Exception("Error. Ocurrió un error inesperado al buscar el indice del registro.");
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }

        private void cbx_Actualizar_Click(object sender, RoutedEventArgs e)
        {
            TipoCatalogo();
        }

        private void btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            BuscarCatalogos();
        }

        private void btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            GuardarConfiguracion();
        }

        private void btn_volver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void txt_Buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Buscar();
        }
    }
}

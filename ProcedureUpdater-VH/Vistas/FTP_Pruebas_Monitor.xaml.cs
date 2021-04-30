using ProcedureUpdater_VH.Metodos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
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
    /// Lógica de interacción para FTP_Pruebas_Monitor.xaml
    /// </summary>
    public partial class FTP_Pruebas_Monitor : Window
    {
        public FTP_Pruebas_Monitor()
        {
            InitializeComponent();
        }

        private void btn_conectar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://192.168.0.50:21/");
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential("luis.robles", "System200");
                request.KeepAlive = false;
                request.UseBinary = true;
                request.UsePassive = true;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Msg.Success(reader.ReadToEnd());

                Msg.Success(String.Format("Directory List Complete, status {0}", response.StatusDescription));

                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }

        }
    }
}

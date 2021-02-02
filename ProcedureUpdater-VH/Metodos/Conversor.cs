using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ProcedureUpdater_VH.Metodos
{
    public abstract class Conversor
    {

        public static bool GuardarConexion(Conexion objConexion)
        {
            string sXML = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);
                XmlSerializer ser = new XmlSerializer(typeof(Conexion));
                ser.Serialize(tw, objConexion);
                tw.Close();

                sXML = sb.ToString();
                string sKey = Guid.NewGuid().ToString().Substring(0, 12);
                GuardarXML(sXML, sKey);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void GuardarXML(string sXML, string sFileKey)
        {
            string[] lines = new string[] { Encriptar(sXML) };
            string sFile = sFileKey;
            
            string sPath = "";
            sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\";
            Directory.CreateDirectory(sPath);

            try
            {
                File.WriteAllLines(@sPath + sFile + ".vh", lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static List<Conexion> OpenXML()
        {
            List<Conexion> lstConexiones = new List<Conexion>();

            string sPath = "";
            sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\";
            Directory.CreateDirectory(sPath);

            string[] files = Directory.GetFiles(sPath, "*.vh");
            
            foreach (string sArchivo in files)
            {
                string sInformacion = File.ReadAllText(sArchivo);
                sInformacion = DesEncriptar(sInformacion);
                //sInformacion = sInformacion.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

                string[] sLineas = sInformacion.Split("\n");
                sInformacion = "";
                for (int i = 1; i < sLineas.Length; i++)
                {
                    sInformacion += sLineas[i];
                }

                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(sInformacion);
                writer.Flush();
                stream.Position = 0;
                XmlSerializer ser = new XmlSerializer(typeof(Conexion));
                Conexion conexion = (Conexion)ser.Deserialize(stream);

                lstConexiones.Add(conexion);
            }
            
            return lstConexiones;
        }

        public static string Encriptar(string sValor)
        {
            string result = string.Empty;
            byte[] encryted = Encoding.Unicode.GetBytes(sValor);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        public static string DesEncriptar(string sValor)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(sValor);
            result = Encoding.Unicode.GetString(decryted);
            return result;
        }

    }
}

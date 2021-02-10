using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProcedureUpdater_VH.Metodos
{
    public abstract class Conversor
    {

        #region Backups
        public static bool GuardarBackupScript(RespaldoVersion objVersion)
        {
            string sXML = "";
            try
            {
                string sKey = Guid.NewGuid().ToString().Substring(0, 12);
                if (!objVersion.sKey.Equals(""))
                {
                    sKey = objVersion.sKey;
                }

                objVersion.sKey = sKey;

                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);
                XmlSerializer ser = new XmlSerializer(typeof(RespaldoVersion));
                ser.Serialize(tw, objVersion);
                tw.Close();

                sXML = sb.ToString();

                GuardarBackupScriptXML(sXML, sKey);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void GuardarBackupScriptXML(string sXML, string sFileKey)
        {
            string[] lines = new string[] { Encriptar(sXML) };
            string sFile = sFileKey;

            string sPath = "";
            sPath = AppDomain.CurrentDomain.BaseDirectory + "backups\\";
            Directory.CreateDirectory(sPath);

            try
            {
                File.WriteAllLines(@sPath + sFile + ".bkvh", lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static List<RespaldoVersion> OpenBackupScriptXML()
        {
            List<RespaldoVersion> lstVersiones = new List<RespaldoVersion>();

            string sPath = "";
            sPath = AppDomain.CurrentDomain.BaseDirectory + "backups\\";
            Directory.CreateDirectory(sPath);

            string[] files = Directory.GetFiles(sPath, "*.bkvh");

            foreach (string sArchivo in files)
            {
                string sInformacion = File.ReadAllText(sArchivo);
                sInformacion = DesEncriptar(sInformacion);

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
                XmlSerializer ser = new XmlSerializer(typeof(RespaldoVersion));
                RespaldoVersion version = (RespaldoVersion)ser.Deserialize(stream);

                lstVersiones.Add(version);
            }

            lstVersiones = lstVersiones.OrderBy(x => x.dtActualizacion).ToList();

            return lstVersiones;
        }

        #endregion 

        #region Conexion

        public static bool GuardarConexion(Conexion objConexion)
        {
            string sXML = "";
            try
            {
                string sKey = Guid.NewGuid().ToString().Substring(0, 12);
                if (!objConexion.sKey.Equals(""))
                {
                    sKey = objConexion.sKey;
                }

                objConexion.sKey = sKey;

                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);
                XmlSerializer ser = new XmlSerializer(typeof(Conexion));
                ser.Serialize(tw, objConexion);
                tw.Close();

                sXML = sb.ToString();

                GuardarConexionXML(sXML, sKey);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool EliminarConexionXML(string sKey)
        {
            try
            {
                string sPath = "";
                sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\" + sKey + ".cxvh";
                File.Delete(sPath);
                return true;
            }
            catch
            {
                throw;
            }
        }

        private static void GuardarConexionXML(string sXML, string sFileKey)
        {
            string[] lines = new string[] { Encriptar(sXML) };
            string sFile = sFileKey;
            
            string sPath = "";
            sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\";
            Directory.CreateDirectory(sPath);

            try
            {
                File.WriteAllLines(@sPath + sFile + ".cxvh", lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static List<Conexion> OpenConexionXML()
        {
            List<Conexion> lstConexiones = new List<Conexion>();

            string sPath = "";
            sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\";
            Directory.CreateDirectory(sPath);

            string[] files = Directory.GetFiles(sPath, "*.cxvh");
            
            foreach (string sArchivo in files)
            {
                string sInformacion = File.ReadAllText(sArchivo);
                sInformacion = DesEncriptar(sInformacion);
                
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

        #endregion
        
        public static void GuardarSQL(string sProcedure, string sScript)
        {
            try
            {
                string[] lines = sScript.Split("\n\r");

                SaveFileDialog sfdGuardado = new SaveFileDialog();
                sfdGuardado.FileName = sProcedure;
                sfdGuardado.Filter = "SQL (*.sql)|*.sql";
                sfdGuardado.ShowDialog();
                string sFile = sfdGuardado.FileName;

                File.WriteAllLines(sFile + ".sql", lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string Encriptar(string sValor)
        {
            string result = string.Empty;
            byte[] encryted = Encoding.Unicode.GetBytes(sValor);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        private static string DesEncriptar(string sValor)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(sValor);
            result = Encoding.Unicode.GetString(decryted);
            return result;
        }

    }
}

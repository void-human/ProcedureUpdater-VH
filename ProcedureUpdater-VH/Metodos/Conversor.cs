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

                GenerarBackupScriptXML(sXML, sKey);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void GenerarBackupScriptXML(string sXML, string sFileKey)
        {
            string[] lines = new string[] { Encriptar(sXML) };
            string sFile = sFileKey;
            DateTime dtFecha = DateTime.Now;
            string sDate = dtFecha.Year + "\\" + dtFecha.Month + "\\" + dtFecha.Day + "\\";
            ConfiguracionLocal configuracionLocal = AbrirConfiguracionLocalXML();

            string sPath = "";
            if (configuracionLocal.Compartir)
            {
                sPath = configuracionLocal.Direccion + "backups\\" + sDate;
            }
            else
            {
                sPath = AppDomain.CurrentDomain.BaseDirectory + "backups\\" + sDate;
            }

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

        public static List<RespaldoVersion> AbrirBackupScriptXML(DateTime dtFechaFiltro)
        {
            List<RespaldoVersion> lstVersiones = new List<RespaldoVersion>();
            string sDate = dtFechaFiltro.Year + "\\" + dtFechaFiltro.Month + "\\" + dtFechaFiltro.Day + "\\";
            ConfiguracionLocal configuracionLocal = AbrirConfiguracionLocalXML();

            string sPath = "";
            if (configuracionLocal.Compartir)
            {
                sPath = configuracionLocal.Direccion + "backups\\" + sDate;
            }
            else
            {
                sPath = AppDomain.CurrentDomain.BaseDirectory + "backups\\" + sDate;
            }

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

        #region Catalogos
        public static TablaCatalogo AbrirTablaCatalogoXML(Conexion conexion)
        {
            TablaCatalogo tablaCatalogo = null;
            ConfiguracionLocal configuracionLocal = AbrirConfiguracionLocalXML();

            string sPath = "";
            if (configuracionLocal.Compartir)
            {
                sPath = configuracionLocal.Direccion + "catologos\\";
            }
            else
            {
                sPath = AppDomain.CurrentDomain.BaseDirectory + "catologos\\";
            }
            Directory.CreateDirectory(sPath);

            string[] files = Directory.GetFiles(sPath, conexion.sKey + ".ctvh");
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

                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(sInformacion);
                writer.Flush();
                stream.Position = 0;
                XmlSerializer ser = new XmlSerializer(typeof(TablaCatalogo));
                tablaCatalogo = (TablaCatalogo)ser.Deserialize(stream);
            }

            return tablaCatalogo;
        }

        private static void GenerarTablaCatalogoXML(string sXML, string sKey)
        {
            string[] lines = new string[] { Encriptar(sXML) };
            string sFile = sKey;
            ConfiguracionLocal configuracionLocal = AbrirConfiguracionLocalXML();

            string sPath = "";
            if (configuracionLocal.Compartir)
            {
                sPath = configuracionLocal.Direccion + "catologos\\";
            }
            else
            {
                sPath = AppDomain.CurrentDomain.BaseDirectory + "catologos\\";
            }
            Directory.CreateDirectory(sPath);

            try
            {
                File.WriteAllLines(@sPath + sFile + ".ctvh", lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static bool GuardarTablaCatalogoXML(TablaCatalogo tablaCatalogo)
        {
            try
            {
                string sXML = "";
                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);
                XmlSerializer ser = new XmlSerializer(typeof(TablaCatalogo));
                ser.Serialize(tw, tablaCatalogo);
                tw.Close();

                sXML = sb.ToString();

                GenerarTablaCatalogoXML(sXML, tablaCatalogo.conexion.sKey);

                return true;
            }
            catch
            {
                return false;
            }
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

                GenerarConexionXML(sXML, sKey);

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
                ConfiguracionLocal configuracionLocal = AbrirConfiguracionLocalXML();

                string sPath = "";
                if (configuracionLocal.Compartir)
                {
                    sPath = configuracionLocal.Direccion + "vh\\" + sKey + ".cxvh";
                }
                else
                {
                    sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\" + sKey + ".cxvh";
                }

                File.Delete(sPath);
                return true;
            }
            catch
            {
                throw;
            }
        }

        private static void GenerarConexionXML(string sXML, string sFileKey)
        {
            string[] lines = new string[] { Encriptar(sXML) };
            string sFile = sFileKey;
            ConfiguracionLocal configuracionLocal = AbrirConfiguracionLocalXML();
            
            string sPath = "";
            if (configuracionLocal.Compartir)
            {
                sPath = configuracionLocal.Direccion + "vh\\";
            }
            else
            {
                sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\";
            }
            
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

        public static List<Conexion> AbrirConexionXML()
        {
            List<Conexion> lstConexiones = new List<Conexion>();
            ConfiguracionLocal configuracionLocal = AbrirConfiguracionLocalXML();

            string sPath = "";
            if (configuracionLocal.Compartir)
            {
                sPath = configuracionLocal.Direccion + "vh\\";
            }
            else
            {
                sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\";
            }

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

        #region Configuracion

        public static void GuardarConfiguracion(Configuracion configuracion)
        {
            string sXML = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);
                XmlSerializer ser = new XmlSerializer(typeof(Configuracion));
                ser.Serialize(tw, configuracion);
                tw.Close();

                sXML = sb.ToString();

                GenerarConfiguracionXML(sXML);
            }
            catch
            {
                throw;
            }
        }

        private static void GenerarConfiguracionXML(string sXML)
        {
            string[] lines = new string[] { Encriptar(sXML) };
            ConfiguracionLocal configuracionLocal = AbrirConfiguracionLocalXML();

            string sPath = "";
            if (configuracionLocal.Compartir)
            {
                sPath = configuracionLocal.Direccion + "vh\\";
            }
            else
            {
                sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\";
            }

            Directory.CreateDirectory(sPath);

            try
            {
                File.WriteAllLines(@sPath + "config.cfvh", lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static Configuracion AbrirConfiguracionXML()
        {
            Configuracion configuracion = new Configuracion();
            ConfiguracionLocal configuracionLocal = AbrirConfiguracionLocalXML();

            string sPath = "";
            if (configuracionLocal.Compartir)
            {
                sPath = configuracionLocal.Direccion + "vh\\";
            }
            else
            {
                sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\";
            }

            Directory.CreateDirectory(sPath);

            string[] files = Directory.GetFiles(sPath, "*.cfvh");

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
                XmlSerializer ser = new XmlSerializer(typeof(Configuracion));
                configuracion = (Configuracion)ser.Deserialize(stream);
            }

            return configuracion;
        }

        public static void GuardarConfiguracionLocal(ConfiguracionLocal configuracion)
        {
            string sXML = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);
                XmlSerializer ser = new XmlSerializer(typeof(ConfiguracionLocal));
                ser.Serialize(tw, configuracion);
                tw.Close();

                sXML = sb.ToString();

                GenerarConfiguracionLocalXML(sXML);
            }
            catch
            {
                throw;
            }
        }

        private static void GenerarConfiguracionLocalXML(string sXML)
        {
            string[] lines = new string[] { Encriptar(sXML) };

            string sPath = "";
            sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\";
            Directory.CreateDirectory(sPath);

            try
            {
                File.WriteAllLines(@sPath + "config.cflvh", lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static ConfiguracionLocal AbrirConfiguracionLocalXML()
        {
            ConfiguracionLocal configuracion = new ConfiguracionLocal();

            string sPath = "";
            sPath = AppDomain.CurrentDomain.BaseDirectory + "vh\\";
            Directory.CreateDirectory(sPath);

            string[] files = Directory.GetFiles(sPath, "*.cflvh");

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
                XmlSerializer ser = new XmlSerializer(typeof(ConfiguracionLocal));
                configuracion = (ConfiguracionLocal)ser.Deserialize(stream);
            }

            if (configuracion == null)
            {
                configuracion.Compartir = false;
                configuracion.Direccion = AppDomain.CurrentDomain.BaseDirectory;
                GuardarConfiguracionLocal(configuracion);
                return AbrirConfiguracionLocalXML();
            }

            return configuracion;
        }
        #endregion



        public static void GuardarSQL(string sProcedure, string sScript, string sPath, bool bUsar)
        {
            try
            {
                string sFile = "";


                string[] lines = sScript.Split("\n\r");

                if (!bUsar)
                {
                    SaveFileDialog sfdGuardado = new SaveFileDialog();
                    sfdGuardado.FileName = sPath+sProcedure;
                    sfdGuardado.Filter = "SQL (*.sql)|*.sql";
                    sfdGuardado.ShowDialog();
                    sFile = sfdGuardado.FileName;
                }
                else
                {
                    sFile = sPath + sProcedure;
                }

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

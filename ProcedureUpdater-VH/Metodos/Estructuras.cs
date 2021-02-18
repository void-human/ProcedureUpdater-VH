using System;
using System.Collections.Generic;
using System.Text;

namespace ProcedureUpdater_VH.Metodos
{
    public class Estructuras
    {
    }

    public class Conexion
    {
        public string Usuario { set; get; }
        public string Contrasena { set; get; }
        public string BDD { set; get; }
        public string IP { set; get; }
        public string sKey { set; get; } //Considerar como ID
    }

    public class Procedure
    {
        public string Nombre { set; get; }
        public string DefinicionV1 { set; get; }
        public string DefinicionV2 { set; get; }
        public bool Modificar { set; get; }
    }

    public class Procedimiento
    {
        public int Indice { set; get; }
        public string v1 { set; get; }
        public string v2 { set; get; }
        public bool bNuevo { set; get; }
        public bool bRemovido { set; get; }
    }

    public class Tabla
    {
        public string Nombre { set; get; }
        public List<Columna> lstColumnas { set; get; }
    }

    public class Columna
    {
        public string Nombre { set; get; }
        public string Nulo { set; get; }
        public string Tipo { set; get; }
        public int Longitud { set; get; }
        public string Completo { set; get; }
    }

    public class VersionesColumna
    {
        public int Indice { set; get; }
        public string CompletoV1 { set; get; }
        public string CompletoV2 { set; get; }
        public bool bModificacion { set; get; }
        public bool bModificacionV2 { set; get; }
        public bool bNuevo { set; get; }
        public bool bRemovido { set; get; }

    }

    public class VersionesTabla
    {
        public bool bModificar { set; get; }
        public Tabla TablaV1 { set; get; }
        public Tabla TablaV2 { set; get; }
    }

    public class RespaldoVersion
    {
        public string sKey { set; get; }
        public string BDD { set; get; }
        public string IP { set; get; }
        public string Nombre { set; get; }
        public string ScriptV1 { set; get; }
        public string ScriptV2 { set; get; }
        public DateTime dtActualizacion { set; get; }
    }

    public class Configuracion
    {
        public string BDD { set; get;}
        public string IP { set; get; }
        public string Direccion { set; get; }
        public bool UsarDireccion { set; get; }
    
        private bool getUsarDireccion()
        {
            if(Direccion != null && !Direccion.Equals("")){
                return this.UsarDireccion;
            }
            else
            {
                return false;
            }
        }
    }

}

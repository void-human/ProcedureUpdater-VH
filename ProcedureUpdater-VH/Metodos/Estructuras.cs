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
        public string sColor { set; get; }
        public bool bNuevo { set; get; }
        public bool bRemovido { set; get; }
    }
}

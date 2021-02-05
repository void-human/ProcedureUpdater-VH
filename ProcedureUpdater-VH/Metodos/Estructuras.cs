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
    }

    public class Procedure
    {
        public string Nombre { set; get; }
        public string DefinicionV1 { set; get; }
        public string DefinicionV2 { set; get; }
        public bool Modificar { set; get; }
    }
}

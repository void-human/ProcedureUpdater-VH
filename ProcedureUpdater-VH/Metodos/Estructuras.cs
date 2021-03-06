﻿using System;
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
        public string sConexion { get { return IP + ':' + BDD; } }
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
        public string sScripts { set; get; }
    }

    public class TablaCatalogo
    {
        public List<Catalogo> lstCatalogos { set; get; }
        public Conexion conexion { set; get; }
        public DateTime dtCreacion { set; get; }
    }

    public class Catalogo
    {
        public string nombre { set; get; }
        public bool catalogo { set; get; }
        public int registrosv1 { set; get; }
        public int registrosv2 { set; get; }
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

    public class ConfiguracionLocal
    {
        public bool Compartir { set; get; }
        public string Direccion { set; get; }
    }

    public class Configuracion
    {
        public string sKey2 { set; get;}
        public string sKey1 { set; get; }
        public string Direccion { set; get; }
        public bool UsarDireccion { set; get; }
        public bool UsarPasos { set; get; }
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

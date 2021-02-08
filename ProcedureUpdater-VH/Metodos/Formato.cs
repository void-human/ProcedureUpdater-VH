using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcedureUpdater_VH.Metodos
{
    public abstract class Formato
    {
        public static string IP(string sIP)
        {
            string sError = "0.0.0.0";

            if (sIP.Count(x => x.ToString() == ".") == 3)
            {
                try
                {
                    foreach (string sOcteto in sIP.Split("."))
                    {
                        int nOcteto = Int32.Parse(sOcteto);
                    }
                }
                catch 
                {
                    return sError;
                }
            }
            else
            {
                return sError;
            }

            return sIP;
        }

    }
}

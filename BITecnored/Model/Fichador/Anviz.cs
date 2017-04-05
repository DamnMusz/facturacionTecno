using BITecnored.Model.DataContract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace BITecnored.Model.Fichador
{
    public class Anviz
    {
        public enum FICHADORES_TECNORED { H_YRIGOYEN=1, AV_SAN_JUAN }
        public Dictionary<FICHADORES_TECNORED, string> ip_fichadores;

        private static Anviz _instance = null;
        public static Anviz GetInstance()
        {
            if (_instance == null)
                _instance = new Anviz();
            return _instance;
        }

        public Anviz()
        {
            ip_fichadores = new Dictionary<FICHADORES_TECNORED, string>();
            ip_fichadores.Add(FICHADORES_TECNORED.H_YRIGOYEN, "192.168.100.4");
            ip_fichadores.Add(FICHADORES_TECNORED.AV_SAN_JUAN, "172.16.1.99");
        }

        public void Connect(FICHADORES_TECNORED id)
        {
            int ret_value = CKT_DLL.CKT_RegisterNet((int)id, ip_fichadores[id]);
            if (ret_value != 1)
                throw new AnvizConnectionException();
        }

        public void Disonnect(FICHADORES_TECNORED id)
        {
            CKT_DLL.CKT_UnregisterSnoNet((int)id);
        }
        
        public DateTime GetDeviceClock(FICHADORES_TECNORED id)
        {
            CKT_DLL.DATETIMEINFO devclock = new CKT_DLL.DATETIMEINFO();

            if (CKT_DLL.CKT_GetDeviceClock((int)id, ref devclock) == 1)
            {
                string date = devclock.Year_Renamed.ToString("D" + 4) + "-" 
                    + devclock.Month_Renamed.ToString("D" + 2) + "-" 
                    + devclock.Day_Renamed.ToString("D" + 2) + " " 
                    + devclock.Hour_Renamed.ToString("D" + 2) + ":" 
                    + devclock.Minute_Renamed.ToString("D" + 2) + ":" 
                    + devclock.Second_Renamed.ToString("D" + 2);
                DateTime res = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                return res;
            }
            else
            {
                throw new AnvizConnectionException();
            }
        }

        public List<IdValueValue> GetClocks()
        {
            List<IdValueValue> res = new List<IdValueValue>();
            Array fichadores = Enum.GetValues(typeof(FICHADORES_TECNORED));
            foreach(FICHADORES_TECNORED f in fichadores)
            {
                try
                {
                    Connect(f);
                    DateTime clock = GetDeviceClock(f);
                    Disonnect(f);
                    res.Add(new IdValueValue((int)f, f.ToString(), clock.ToString()));
                }
                catch {
                    res.Add(new IdValueValue((int)f, f.ToString(), "Error al conectar"));
                }
            }
            return res;
        }
    }
}
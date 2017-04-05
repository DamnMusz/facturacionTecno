using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;

namespace BITecnored.Model.DB
{
    public class DBI : AbstractDB
    {
        private static DBI instance = null;
        public DBI() { dns = "dbi"; }

        public static DBI GetInstance()
        {
            if (instance == null)
                instance = new DBI();
            return instance;
        }
    }
}
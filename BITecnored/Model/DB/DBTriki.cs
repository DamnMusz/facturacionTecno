using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;

namespace BITecnored.Model.DB
{
    public class DBTriki : AbstractDB
    {
        public DBTriki() { dns = "triki"; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BITecnored.Model
{
    public class SLATriki_dbManager
    {
        public List<Inspeccion> GetInspecciones(DateTime periodo)
        {
            List<Inspeccion> res = new List<Inspeccion>();
            //Query query = new Query();
            //query.AddCriterio(new SQL_Criteria(SQL_Criteria.CriteriaType.AND, new Operator("cierre_fact", Operator.Operator_Type.EQUALS, periodo.Year.ToString("D" + 4) + periodo.Month.ToString("D" + 2))));
            return res;
        }
    }
}
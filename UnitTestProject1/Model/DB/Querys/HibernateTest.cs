using BITecnored.Entities;
using BITecnored.Entities.SLA;
using BITecnored.Model.SLA;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using BITecnored.Model.Fichador;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BITecnored.Model.DB.Querys
{  
    [TestClass]
    public class HibernateTest
    {
        [TestMethod]
        public void ReadTest()
        {
            //InspeccionSLA_DatosAgenda insp = new InspeccionSLA_DatosAgenda();
            //List<InspeccionSLA_DatosAgenda> res = insp.SetPeriodoFacturacion(new DateTime(2016, 8, 1)).Read().Cast<InspeccionSLA_DatosAgenda>().ToList();

            //List<InspeccionSLA_AnexoAgenda> aux = new List<InspeccionSLA_AnexoAgenda>();

            //foreach (InspeccionSLA_DatosAgenda i in res)
            //    aux.Add(new InspeccionSLA_AnexoAgenda(i));

            //Entity.UpdateAll(aux.Cast<Entity>().ToList());

            //BaseSLAGenerator.GetInstance().GenerateNoRealizadas(new DateTime(2016,06,01));


            Anviz fichador = new Anviz();
            fichador.Connect(Anviz.FICHADORES_TECNORED.H_YRIGOYEN);
            DateTime date = fichador.GetDeviceClock(Anviz.FICHADORES_TECNORED.H_YRIGOYEN);
            fichador.Disonnect(Anviz.FICHADORES_TECNORED.H_YRIGOYEN);
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class RangoTiempos
    {
        [DataMember]
        protected int mismoDia { get; set; }
        [DataMember]
        protected int en24hs { get; set; }
        [DataMember]
        protected int en48hs { get; set; }
        [DataMember]
        protected int en72hs { get; set; }
        [DataMember]
        protected int masDe72hs { get; set; }
        [DataMember]
        private double promedio;

        protected double sumatoriaDias = 0;
        protected int cantidadValores = 0;

        public RangoTiempos(int mismodia, int en24hs, int en48hs, int en72hs, int masde72hs, double tiempoPromedio)
        {
            this.mismoDia = mismodia;
            this.en24hs = en24hs;
            this.en48hs = en48hs;
            this.en72hs = en72hs;
            this.masDe72hs = masde72hs;
            this.cantidadValores = mismoDia + en24hs + en48hs + en72hs + masDe72hs;
            this.promedio = Math.Round(tiempoPromedio,2);
        }

        public RangoTiempos()
        {
            this.mismoDia = this.en24hs = this.en48hs = this.en72hs = this.masDe72hs = 0;
        }

        public void AddTiempos(double diferenciaDias)
        {
            if (diferenciaDias < 1) { IncreaseMismoDia(); }
            else if (diferenciaDias < 2) { Increase24Hs(); }
            else if (diferenciaDias < 3) { Increase48Hs(); }
            else if (diferenciaDias < 4) { Increase72Hs(); }
            else { IncreaseMasDe72Hs(); }
            ++cantidadValores;
            sumatoriaDias += diferenciaDias;
            promedio = sumatoriaDias / cantidadValores;
        }
        
        private void IncreaseMismoDia() { ++mismoDia; }
        private void Increase24Hs() { ++en24hs; }
        private void Increase48Hs() { ++en48hs; }
        private void Increase72Hs() { ++en72hs; }
        private void IncreaseMasDe72Hs() { ++masDe72hs; }
        public void SetPromedio(double promedioDias) { this.promedio = Math.Round(promedioDias, 2); }
        public double GetPromedio() { return this.promedio; }
    }
}
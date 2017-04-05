using System.Runtime.Serialization;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class MotivoDiaPactado
    {
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public int total { get; set; }
        [DataMember]
        public int realizadas { get; set; }
        [DataMember]
        public int en_gestion { get; set; }
        [DataMember]
        public int sin_efecto { get; set; }

        public MotivoDiaPactado(string nombre, int total, int realizadas, int en_gestion, int sin_efecto)
        {
            this.nombre = nombre;
            this.total = total;
            this.realizadas = realizadas;
            this.en_gestion = en_gestion;
            this.sin_efecto = sin_efecto;
        }

        public MotivoDiaPactado(string nombre)
        {
            this.nombre = nombre;
            this.total = 0;
            this.realizadas = 0;
            this.en_gestion = 0;
            this.sin_efecto = 0;
        }

        public void setByEstado(int estado, int cantidad)
        {
            switch (estado) {
                // EN GESTION
                case 2:
                    en_gestion = cantidad;
                    break;
                // REALIZADA
                case 5:
                    realizadas = cantidad;
                    break;
                // SIN EFECTO
                case 6:
                    sin_efecto = cantidad;
                    break;
            }
            total = realizadas + en_gestion + sin_efecto;
        }   
    }
}
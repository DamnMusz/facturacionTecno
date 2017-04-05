using System.Data.Odbc;

namespace BITecnored.Model.DB
{
    public class DBAgendaPrueba : AbstractDB
    {
        private static DBAgendaPrueba instance = null;

        public DBAgendaPrueba() { dns = "agenda_prueba"; }

        public static DBAgendaPrueba GetInstance()
        {
            if (instance == null)
                instance = new DBAgendaPrueba();
            return instance;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BITecnored.Model.LongProcessState
{
    [DataContract]
    public class GeneradorSLAProcessState : AbstractProcessState
    {
        private static GeneradorSLAProcessState _instance { get; set; } = null;
        private GeneradorSLAProcessState() {
            this.state = AbstractProcessState.STOPPED;
        }
        public static GeneradorSLAProcessState GetInstance()
        {
            if (_instance == null) {
                _instance = new GeneradorSLAProcessState();
            }
            return _instance;
        }
    }
}
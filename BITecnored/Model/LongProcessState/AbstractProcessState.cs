using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BITecnored.Model
{
    [DataContract]
    public class AbstractProcessState
    {
        public static string EXISTS = "EXISTS";
        public static string STARTED = "STARTED";
        public static string RUNNING = "RUNNING";
        public static string STOPPED = "STOPPED";
        public static string ERROR = "ERROR";
        public static string INVALID = "INVALID";
        public static string ZERO = "ZERO";
        public bool running;
        [DataMember]
        public string state { get; set; }  = STOPPED;
        [DataMember]
        public string result { get; set; } = "";
        public string param;
        [DataMember]
        public int progress { get; set; } = 0;
        [DataMember]
        public string info { get; set; } = "";
        [DataMember]
        public string error_info { get; set; } = "";
    }
}
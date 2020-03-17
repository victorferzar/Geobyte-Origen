using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.DrillingModels
{
    public class PerforacionDiaria
    {
        public string HOLEID { get; set; }
        public string STATUS { get; set; }
        public  string PROJECTCODE { get; set; }
        public double? DEPTH { get; set; }
        public string PROSPECT { get; set; }
        public string FECHA_COLLAR { get; set; }
        public string HOLEID_PROP { get; set; }
        public string HOLEID_MOD { get; set; }
        public double? PROF_PROPUESTA { get; set; }
        public string SR_PROPUESTO { get; set; }
        public double? AZIMUTH_PROP { get; set; }
        public double? DIP_PROP { get; set; }
        public string Maquina_Perf { get; set; }
        public double? PROP_DDH_M { get; set; }
        public double? PROP_ODEX_M { get; set; }
        public double? PROP_RC_M { get; set; }
        public double? PROP_TLODO_M { get; set; }
        public string FIN_POZO { get; set; }
        public int? Survey { get; set; }
        public string SURVEY_GEO { get; set; }
        public string SURVEY_DRILLING { get; set; }
        public double? ENTREGADO { get; set; }
    }
}
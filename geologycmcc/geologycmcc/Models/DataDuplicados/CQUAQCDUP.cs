using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.DataDuplicados
{
    public class CQUAQCDUP
    {
        public int NREGISTRO { get; set; }
        public string HOLEID { get; set; }
        public string PROJECTCODE { get; set; }
        public string STATUS { get; set; }
        public string ID_OR { get; set; }
        public string ID_CK { get; set; }
        public int PRIORITY_OR { get; set; }
        public string ANALYSISSUITE { get; set; }
        public double? SAMPFROM { get; set; }
        public double? SAMPTO { get; set; }
        public double? ASSAYVALUE_OR { get; set; }
        public double? ASSAYVALUE_CK { get; set; }
        public double? DIFERENCIA { get; set; }
        public double? VAR_REL { get; set; }
        public double? NPORCDATOS { get; set; }
        public double? AMPD { get; set; }
        public double? PROMEDIO { get; set; }
        public double? MPD { get; set; }
        public double? NAMPDPOND { get; set; }
        public double? NAMPORD { get; set; }
        public double? NZSCORE { get; set; }
        public string SAMPLE_DRILTYPE { get; set; }
        public string CHECKSTAGE { get; set; }
        public string ASSAYNAME { get; set; }
        public string RETURNDATE { get; set; }
        public string DESPATCHNO { get; set; }
        public string LABJOBNO { get; set; }
        public string LABCODE { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.QaQcModels
{
    public class QaQcDup
    {
        public String ID_OR { get; set; }
        public String ID_CK { get; set; }
        public String HOLEID { get; set; }
        public String PROJECTCODE { get; set; }
        public Double? SAMPFROM { get; set; }
        public Double? SAMPTO { get; set; }
        public String CHECKSTAGE { get; set; }
        public String ASSAYNAME { get; set; }
        public Double? ASSAYVALUE_OR { get; set; }
        public Double? ASSAYVALUE_CK { get; set; }
        public Double? DIFERENCIA { get; set; }
        public Double? VAR_REL { get; set; }
        public Double? AMPD { get; set; }
        public Double? PROMEDIO { get; set; }
        public Double? MPD { get; set; }
        public int? PRIORITY_OR { get; set; }
        public int? PRIORITY_CK { get; set; }
        public String DESPATCHNO { get; set; }
        public String LABJOBNO_OR { get; set; }
        public String LABJOBNO_CK { get; set; }
        public String LABCODE { get; set; }
        public String ANALYSISSUITE { get; set; }
        public String SENDDATE { get; set; }
        public String RETURNDATE { get; set; }
        public String SAMPLE_DRILLTYPE { get; set; }
        public String T_MIN { get; set; }
     

    }
}
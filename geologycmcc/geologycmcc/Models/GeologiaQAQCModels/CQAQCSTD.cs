using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.GeologiaQAQCModels
{
    public class CQAQCSTD
    {
        public int INDICE { get; set; }
        public string CHECKID { get; set; }
        public string STANDARDID { get; set; }
        public string HOLEID { get; set; }
        public string ASSAYNAME { get; set; }
        public Nullable<double> ASSAYVALUE { get; set; }
        public int ASSAY_PRIORITY { get; set; }
        public string DESPATCHNO { get; set; }
        public string LABJOBNO { get; set; }
        public string LABCODE { get; set; }
        public string ANALYSISSUITE { get; set; }
        public string RETURNDATE { get; set; }
        public string SENDDATE { get; set; }
        public double? NORMALIZACION { get; set; }
        public double? MIN { get; set; }
        public double? MAX { get; set; }
        public double? DEV { get; set; }
        public double? STANDARDVALUE { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.DataDuplicados
{
    public class CDetalleDUP
    {

        public string N { get; set; }
        public string HOLEID { get; set; }
        public string ID_OR { get; set; }
        public string ID_CK { get; set; }
        public string CHECKSTAGE { get; set; }
        public double? SAMPFROM { get; set; }
        public double? SAMPTO { get; set; }
        public string NAME { get; set; }
        public double? ASSAYVALUE_OR { get; set; }
        public double? ASSAYVALUE_CK { get; set; }
        public double? DIFERENCIA { get; set; }
        public double? VAL_REL { get; set; }
        public double? NPORCDATOS { get; set; }
        public double? AMPD { get; set; }
        public double? PROMEDIO { get; set; }
        public double? MPD { get; set; }
        public double? AMPD_POND { get; set; }
        public double? AMPD_ORD { get; set; }
        public double? ZCORE { get; set; }

        public CDetalleDUP(string n, string holeid, string id_or, string id_ck, string checkstage, double? sampfrom, double? sampto, string name, double? assayvalue_or, double? assayvalue_ck, double? diferencia, double? datos, double? val_rel, double? ampd, double? promedio, double? mpd, double? ampd_pond, double? ampd_ord, double? zcore)
        {
            N = n;
            HOLEID = holeid;
            ID_OR = id_or;
            ID_CK = id_ck;
            CHECKSTAGE = checkstage;
            SAMPFROM = sampfrom;
            SAMPTO = sampto;
            NAME = name;
            ASSAYVALUE_OR = assayvalue_or;
            ASSAYVALUE_CK = assayvalue_ck;
            DIFERENCIA = diferencia;
            VAL_REL = val_rel;
            AMPD = ampd;
            PROMEDIO = promedio;
            MPD = mpd;
            AMPD_POND = ampd_pond;
            AMPD_ORD = ampd_ord;
            ZCORE = zcore;
            NPORCDATOS = datos;



        }

    }
}
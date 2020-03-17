using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.DocumentosModels
{
    public class DocumentsDrillHole
    {
        public String PROJECTCODE { get; set; }
        public String HOLEID { get; set; }
        public Double? FISCAL_YEAR { get; set; }
        public String STATUS { get; set; }
        public String DH_DRILLTYPE_LIST { get; set; }
    }
}
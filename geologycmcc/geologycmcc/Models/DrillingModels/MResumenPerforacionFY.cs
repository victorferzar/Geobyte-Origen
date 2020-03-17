using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.DrillingModels
{
    public class MResumenPerforacionFY
    {
        public string PROJECTCODE { get; set; }
        public double DEPTH { get; set; }
        public double PROF_PROPUESTA { get; set; }
    }
    public class MRFechaMetros
    {
        public DateTime FECHA { get; set; }
        public double TOTAL { get; set; }
        
    }
}
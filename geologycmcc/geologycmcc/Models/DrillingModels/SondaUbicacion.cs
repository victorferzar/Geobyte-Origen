using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.DrillingModels
{
    public class SondaUbicacion
    {
        public String HOLEID { get; set; }
        public  String FECHA { get; set; }
        public  String SONDA { get; set; }
        public  double DEPTH { get; set; }
        public  double PROF_PROPUESTA {get; set;}
        public  double NORTE_GIS { get; set; }
        public double ESTE_GIS { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.GeometalurgiaModels
{
    public class ResumenPilas
    {
        public String HOLEID { get; set; }
        public String PLANTA { get; set; }
        public DateTime INICIOCARGA { get; set; }
        public DateTime TERMINOCARGA { get; set; }
        public Double TMS { get; set; }
        public Double CuT_PCT { get; set; }
        public Double CuS_PCT { get; set; }
        public Double CUFTTON { get; set; }
        public Double CUFSTON { get; set; }
        public Double RS { get; set; }

        

    }
}
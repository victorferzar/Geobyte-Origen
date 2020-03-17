using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.GeometalurgiaModels
{
    public class RecuperacionPondTonelaje
    {
        public DateTime FECHACARGA {get;set;}
        public Double CuT_RIPIOS { get; set; }
        public Double CuT_CORTADOR { get; set; }
        public Double CuS_CORTADOR { get; set; }
        public Double TONELAJEDIARIO { get; set; }
        public Double Rec_CuT_Fecha_Carga { get; set; }

    }
}
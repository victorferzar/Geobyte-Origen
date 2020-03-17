using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.GeometalurgiaModels
{
    public class MineralRecuperacion
    {
        public DateTime FECHAMOVIMIENTO { get; set; }
        public Double TONDIA { get; set; }
        public double? OXIDO { get; set; }
        public double? SULFURO { get; set; }
        public double? MSH { get; set; }
        public double? MSHB { get; set; }
        public double? MSHM { get; set; }
        public DateTime FECHACARGA { get; set; }
        public Double CuT_RIPIOS { get; set; }
        public Double CuT_CORTADOR { get; set; }
        public Double CuS_CORTADOR { get; set; }
        public Double TONELAJEDIARIO { get; set; }
        public Double Rec_CuT_Fecha_Carga { get; set; }
    }
}
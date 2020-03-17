using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.HidrogeologiaModels
{
    public class ResumenHidrogeologia
    {
        public String HOLEID { get; set; }
        public String SAMPLEID { get; set; }
        public Double? SAMPFROM { get; set; }
        public Double? Hidro_Bi_mgl { get; set; }
        public int? Hidro_Bi_mgl_Umbral { get; set; }
        public Double? Hidro_Ca_mgl { get; set; }
        public int? Hidro_Ca_mgl_Umbral { get; set; }
        public Double? Hidro_Cl_mgl { get; set; }
        public int? Hidro_Cl_mgl_Umbral { get; set; }
        public Double? Hidro_Conductivity { get; set; }
        public int? Hidro_Conductivity_Umbral { get; set; }
        public Double? Hidro_Cu_mgl { get; set; }
        public int? Hidro_Cu_mgl_Umbral { get; set; }
        public Double? Hidro_Fe_mgl { get; set; }
        public int? Hidro_Fe_mgl_Umbral { get; set; }
        public Double? Hidro_K_Uph { get; set; }
        public int? Hidro_K_Uph_Umbral { get; set; }
        public Double? Hidro_Mn_mgl { get; set; }
        public int? Hidro_Mn_mgl_Umbral { get; set; }
        public Double? Hidro_Na_Uph { get; set; }
        public int? Hidro_Na_Uph_Umbral { get; set; }
        public Double? Hidro_Ph_Uph { get; set; }
        public int? Hidro_Ph_Uph_Umbral { get; set; }
        public Double? Hidro_Sul_mgl { get; set; }
        public int? Hidro_Sul_mgl_Umbral { get; set; }
        public Double? Hidro_Temp_mgl { get; set; }
        public int? Hidro_Temp_mgl_Umbral { get; set; }
        public Double? NivelFreatico { get; set; }
        public int? NivelFreatico_Umbral { get; set; }
        public DateTime Fecha { get; set; }

    }
}
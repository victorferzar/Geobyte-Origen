//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace geologycmcc.Models.GeologiaQAQCModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class DTM_COLLAR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DTM_COLLAR()
        {
            this.DTM_QAQC_DUP = new HashSet<DTM_QAQC_DUP>();
        }
    
        public string HOLEID { get; set; }
        public string PROJECTCODE { get; set; }
        public string HOLETYPE { get; set; }
        public Nullable<double> EAST { get; set; }
        public Nullable<double> NORTH { get; set; }
        public Nullable<double> RL { get; set; }
        public Nullable<double> DEPTH { get; set; }
        public string PROSPECT { get; set; }
        public string STARTDATE { get; set; }
        public string ENDDATE { get; set; }
        public Nullable<double> ANO { get; set; }
        public string EMPRESA { get; set; }
        public string FECHA_COLLAR { get; set; }
        public string Fecha_Esquema { get; set; }
        public string Fecha_Mapeo { get; set; }
        public string FECHA_MEDICION { get; set; }
        public string FECHA_PARCIAL { get; set; }
        public string FECHA_PROP { get; set; }
        public string FECHA_RECUPERACION { get; set; }
        public string FECHA_REPLANTEO { get; set; }
        public string FECHA_SURVEY { get; set; }
        public string FECHA_VALIDACION { get; set; }
        public string FF_ID_FINAL { get; set; }
        public string FF_ID_PROPUESTO { get; set; }
        public string FF_NOMBRE_FINAL { get; set; }
        public string FF_NOMBRE_PROPUESTO { get; set; }
        public Nullable<double> FISCAL_YEAR { get; set; }
        public string Geologo_Esquema { get; set; }
        public string Geologo_Mapeo { get; set; }
        public string HOLEID_MOD { get; set; }
        public string HOLEID_PROP { get; set; }
        public string LOA16 { get; set; }
        public string LOA17 { get; set; }
        public string MEDICION { get; set; }
        public Nullable<double> MODELO { get; set; }
        public Nullable<double> MODELO_2008 { get; set; }
        public Nullable<double> MODELO_2009 { get; set; }
        public Nullable<double> MODELO_2010 { get; set; }
        public Nullable<double> MODELO_2011 { get; set; }
        public Nullable<double> MODELO_2012 { get; set; }
        public Nullable<double> MODELO_2012_HYP { get; set; }
        public Nullable<double> MODELO_2013 { get; set; }
        public Nullable<double> MODELO_2014 { get; set; }
        public Nullable<double> MODELO_2015 { get; set; }
        public Nullable<double> MODELO_2016 { get; set; }
        public Nullable<double> MODELO_2017 { get; set; }
        public Nullable<double> MODELO_2YB { get; set; }
        public Nullable<double> MODELO_QRM17 { get; set; }
        public string PERSONA_COLLAR { get; set; }
        public string PERSONA_PARCIAL { get; set; }
        public string PERSONA_RECUPERACION { get; set; }
        public string PERSONA_REPLANTEO { get; set; }
        public string PERSONA_REVER_SOND { get; set; }
        public string PERSONA_SURVEY { get; set; }
        public string PERSONA_VALIDACION { get; set; }
        public Nullable<double> PRIORIDAD_CARGA { get; set; }
        public Nullable<double> PRIORIDAD_PERFO { get; set; }
        public Nullable<double> PROF_PROPUESTA { get; set; }
        public string RECOD_BD { get; set; }
        public string RECOD_FECHA { get; set; }
        public string RECOD_GEOL_ALT { get; set; }
        public string RECOD_GEOL_TMIN { get; set; }
        public string RECOD_STATUS { get; set; }
        public string SR_PROPUESTO { get; set; }
        public string STATUS { get; set; }
        public Nullable<double> T_MSH { get; set; }
        public Nullable<double> T_OX { get; set; }
        public Nullable<double> T_PRI { get; set; }
        public Nullable<double> T_SULF { get; set; }
        public string OBS_COLLAR { get; set; }
        public string OBS_REPLANTEO { get; set; }
        public string COMENTARIOS { get; set; }
        public string RUTA_FOTO { get; set; }
        public Nullable<double> PROPUESTAS_X { get; set; }
        public Nullable<double> PROPUESTAS_Y { get; set; }
        public Nullable<double> PROPUESTAS_Z { get; set; }
        public Nullable<double> REPLANTEADAS_X { get; set; }
        public Nullable<double> REPLANTEADAS_Y { get; set; }
        public Nullable<double> REPLANTEADAS_Z { get; set; }
        public Nullable<double> AZIMUTH_inicio { get; set; }
        public Nullable<double> AZIMUTH_media { get; set; }
        public Nullable<double> AZIMUTH_PROP { get; set; }
        public Nullable<double> COTA_VULCAN_d { get; set; }
        public Nullable<double> CS_Atrapado { get; set; }
        public Nullable<double> DDH_Perf { get; set; }
        public string DH_DIAMETERTYPE_LIST { get; set; }
        public string DH_DRILLTYPE_LIST { get; set; }
        public Nullable<double> DIP_inicio { get; set; }
        public Nullable<double> DIP_media { get; set; }
        public Nullable<double> DIP_PROP { get; set; }
        public Nullable<double> Distancia_Prop { get; set; }
        public Nullable<double> ESTE_GIS { get; set; }
        public Nullable<double> ESTE_VULCAN_d { get; set; }
        public string FIN_POZO { get; set; }
        public Nullable<double> LODO_Perf { get; set; }
        public string Maquina_Perf { get; set; }
        public string MLP { get; set; }
        public Nullable<double> Muestras_mts { get; set; }
        public Nullable<double> NORTE_GIS { get; set; }
        public Nullable<double> NORTE_VULCAN_d { get; set; }
        public Nullable<double> RC_Perf { get; set; }
        public Nullable<int> Survey { get; set; }
        public string TIPO_SURVEY { get; set; }
        public Nullable<double> TSULF { get; set; }
        public Nullable<double> PROP_DDH_M { get; set; }
        public Nullable<double> PROP_ODEX_M { get; set; }
        public Nullable<double> PROP_RC_M { get; set; }
        public Nullable<double> PROP_TLODO_M { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DTM_QAQC_DUP> DTM_QAQC_DUP { get; set; }
    }
}
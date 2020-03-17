using geologycmcc.Controllers.DataBase;
using geologycmcc.Models.GeometalurgiaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Controllers.Crud
{
    public class CRUDGeometalurgia
    {

        TransaccionesGeometalurgia tr;
        public IEnumerable<RecuperacionPondTonelaje> ponderacionTonelaje(String HOLEID)
        {
            tr = new TransaccionesGeometalurgia();
            IEnumerable<RecuperacionPondTonelaje> resultado = tr.dataRecuperacionPondTonelaje("SELECT CAST(FECHACARGA AS DATE) FECHACARGA,isnull(CuT_RIPIOS,0)CuT_RIPIOS,isnull(CuT_CORTADOR,0)CuT_CORTADOR,isnull(CuS_CORTADOR,0)CuS_CORTADOR,TONELAJEDIARIO ,isnull( Round((((CuT_CORTADOR * TONELAJEDIARIO) - (CuT_RIPIOS * TONELAJEDIARIO)) / (CuT_CORTADOR * TONELAJEDIARIO)), 2),0)*100 Rec_CuT_Fecha_Carga FROM ( SELECT PVTCORTADOR.FECHACARGA,Round(AVG(CONVERT(FLOAT,PVTRIPIOS.CuT_CMCCAAS_pct)), 2)CuT_RIPIOS ,Round(AVG(CONVERT(FLOAT,PVTCORTADOR.CuT_CMCCAAS_pct)), 2)CuT_CORTADOR ,Round(AVG(CONVERT(FLOAT,PVTCORTADOR.CuS_CMCCAAS_pct)), 2)CuS_CORTADOR  , max(CONVERT(FLOAT,PVTCORTADOR.TONELAJEDIARIO)) TONELAJEDIARIO  FROM  ( SELECT S.SAMPLEID,S.HOLEID  , NAME,VALUE FROM( SELECT SAMPLEID  , NAME, CONVERT(VARCHAR,VALUE) VALUE FROM SAMPLEDETAILS UNION SELECT SAMPLEID  , NAME, CONVERT(VARCHAR,VALUE) VALUE FROM CORPSAMPLEASSAY  )B INNER JOIN SAMPLE S ON B.SAMPLEID=S.SAMPLEID WHERE NAME IN ('TONELAJEDIARIO','CuT_CMCCAAS_pct','FECHACARGA','CuS_CMCCAAS_pct') )A pivot( MAX(VALUE) FOR NAME IN ([TONELAJEDIARIO],[CuT_CMCCAAS_pct],[CuS_CMCCAAS_pct],[FECHACARGA]) ) AS PVTCORTADOR LEFT JOIN   ( SELECT LEFT(S.HOLEID,LEN(S.HOLEID)-3)HOLEID,STARTDATE FECHACARGA,S.SAMPLEID  , NAME, CONVERT(VARCHAR,VALUE) VALUE FROM CORPSAMPLEASSAY CS INNER JOIN SAMPLE S ON CS.SAMPLEID=S.SAMPLEID INNER JOIN HOLELOCATION HL ON HL.HOLEID=S.HOLEID WHERE NAME IN ('CuT_CMCCAAS_pct') AND S.SAMPLETYPE='PILA' )A pivot( MAX(VALUE) FOR NAME IN ([CuT_CMCCAAS_pct]) ) AS PVTRIPIOS ON PVTCORTADOR.HOLEID=PVTRIPIOS.HOLEID AND  CAST(PVTCORTADOR.FECHACARGA AS DATE)=CAST(PVTRIPIOS.FECHACARGA AS DATE) WHERE PVTCORTADOR.HOLEID LIKE '" + HOLEID+"' GROUP BY PVTCORTADOR.FECHACARGA  )S ORDER BY CAST(S.FECHACARGA AS DATE)");

            return resultado;
        }

        public IEnumerable<BusquedaPilas> ListaSondajes()
        {
            tr = new TransaccionesGeometalurgia();
            IEnumerable<BusquedaPilas> resultado = tr.DataBusquedaPilas(" SELECT HOLEID  FROM HOLELOCATION WHERE  PROJECTCODE ='PILA'");

            return resultado;
        }

        public IEnumerable<ResumenPilas> ResumenPilas(String HOLEID)
        {
            tr = new TransaccionesGeometalurgia();
            IEnumerable<ResumenPilas> resultado = tr.ResumenPilas("SELECT TMP.HOLEID "+
                                                                    ",(SELECT PROSPECT FROM HOLELOCATION HL WHERE HL.HOLEID=TMP.HOLEID)PLANTA " +
                                                                    ",TMP.INICIOCARGA " +
                                                                    ",TMP.TERMINOCARGA " +
                                                                    ",TMP.TMS " +
                                                                    ",ROUND((TMP.[CUFTTON]/TMP.TMS)*100,2) 'CuT_PCT' " +
                                                                    ",ROUND((TMP.[CUFSTON]/TMP.TMS)*100,2) 'CuS_PCT' " +
                                                                    ",TMP.[CUFTTON] " +
                                                                    ",TMP.[CUFSTON] " +
                                                                    ",ROUND(ROUND((TMP.[CUFSTON]/TMP.TMS)*100,2)/ROUND((TMP.[CUFTTON]/TMP.TMS)*100,2),2)  'RS' " +                                                              
                                                                    "FROM(SELECT HOLEID " +
                                                                    ",MIN(CAST(FECHACARGA AS DATE)) 'INICIOCARGA'  " +
                                                                    ",MAX(CAST(FECHACARGA AS DATE)) 'TERMINOCARGA' " +
                                                                    ",SUM(CONVERT(FLOAT,TONELAJEDIARIO)) 'TMS' " +
                                                                    ",ROUND(SUM(CONVERT(FLOAT,TONELAJEDIARIO) * CONVERT(FLOAT,CuT_CMCCAAS_pct))/100,0) 'CUFTTON'  " +
                                                                    ",ROUND(SUM(CONVERT(FLOAT,TONELAJEDIARIO) * CONVERT(FLOAT,CuS_CMCCAAS_pct))/100,0) 'CUFSTON'  " +
                                                                    "FROM    " +
                                                                    "( SELECT S.SAMPLEID,S.HOLEID  , NAME,VALUE FROM( SELECT SAMPLEID  , NAME, CONVERT(VARCHAR,VALUE) VALUE FROM SAMPLEDETAILS  " +
                                                                    "  UNION  " +
                                                                    "  SELECT SAMPLEID  , NAME, CONVERT(VARCHAR,VALUE) VALUE FROM CORPSAMPLEASSAY  )B  " +
                                                                    "  INNER JOIN SAMPLE S ON B.SAMPLEID=S.SAMPLEID WHERE NAME IN ('TONELAJEDIARIO','CuT_CMCCAAS_pct','FECHACARGA','CuS_CMCCAAS_pct') )A pivot( MAX(VALUE) FOR NAME IN ([TONELAJEDIARIO],[CuT_CMCCAAS_pct],[CuS_CMCCAAS_pct],[FECHACARGA]) )  " +
                                                                    "  AS PVTCORTADOR " +
                                                                    "  WHERE PVTCORTADOR.HOLEID='"+HOLEID+"' " +
                                                                    "  GROUP BY PVTCORTADOR.HOLEID)TMP ");

            return resultado;
        }
        public IEnumerable<MineralPila> MineralPila(String PLANTA, DateTime INICIO, DateTime FIN)
        {
            tr = new TransaccionesGeometalurgia();
            IEnumerable<MineralPila> resultado = tr.MineralPila("  select FECHAMOVIMIENTO,TONDIA,[OXIDO],[SULFURO],[MSH],[MSHB],[MSHM] from ( " +
                        "  SELECT FECHAMOVIMIENTO,T_MIN_MINA,TONMINERAL,TONDIA,TONTOTAL  " +
                        "  FROM (SELECT CAST(FECHAMOVIMIENTO  AS DATE)FECHAMOVIMIENTO,T_MIN_MINA,SUM(CONVERT(FLOAT,TONFINAL)) TONMINERAL,sum(sum(CONVERT(FLOAT,TONFINAL))) over (PARTITION BY FECHAMOVIMIENTO) TONDIA,sum(sum(CONVERT(FLOAT,TONFINAL))) over () TONTOTAL  " +
                        "  FROM    " +
                        "( SELECT S.SAMPLEID,S.HOLEID  , NAME,VALUE FROM(  " +
                        "SELECT SAMPLEID  , NAME, CONVERT(VARCHAR,VALUE) VALUE FROM SAMPLEDETAILS  " +
                        "  UNION  " +
                        "  SELECT SAMPLEID  , NAME, CONVERT(VARCHAR,VALUE) VALUE FROM CORPSAMPLEASSAY  )B  " +
                        "  INNER JOIN SAMPLE S ON B.SAMPLEID=S.SAMPLEID WHERE NAME IN ('TONFINAL','T_MIN_MINA','FECHAMOVIMIENTO','DESTINOPLANTA') )A  " +
                        "  pivot( MAX(VALUE) FOR NAME IN ([TONFINAL],[T_MIN_MINA],[FECHAMOVIMIENTO],[DESTINOPLANTA]) )  " +
                        "  AS PVTMINA " +
                        "  WHERE CASt( PVTMINA.FECHAMOVIMIENTO AS DATE) >='" + INICIO.ToString("yyyy-MM-dd") + "' AND CASt(PVTMINA.FECHAMOVIMIENTO AS DATE) <='" + FIN.ToString("yyyy-MM-dd") + "' AND DESTINOPLANTA='" + PLANTA + "' " +
                        "  GROUP BY FECHAMOVIMIENTO,T_MIN_MINA)TMP)B  pivot( MAX(TONMINERAL) FOR T_MIN_MINA IN ([OXIDO],[SULFURO],[MSH],[MSHB],[MSHM]) )  " +
                        "  AS PCTMINA " +
                        "  ORDER BY CAST(FECHAMOVIMIENTO AS DATE) " );

            return resultado;
        }
    }
}
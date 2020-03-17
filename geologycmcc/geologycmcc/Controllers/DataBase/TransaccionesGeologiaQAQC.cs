using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using geologycmcc.Models.GeologiaQAQCModels;

namespace geologycmcc.Controllers.DataBase
{
    public class TransaccionesGeologiaQAQC
    {
        private Conexion cnLocalBD;

        public IEnumerable<FiscalYear> dataListFY(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionDM_SON_String());

            IEnumerable<FiscalYear> resumen = context.ExecuteQuery<FiscalYear>(data).ToList();
            context.Dispose();
            return resumen;
        }

        public IEnumerable<Laboratorio> dataLaboratorio(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionDM_SON_String());

            IEnumerable<Laboratorio> resumen = context.ExecuteQuery<Laboratorio>(data).ToList();
            context.Dispose();
            return resumen;
        }



        public IEnumerable<DTM_QAQC_BLK_STD> dataALLBlancos(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionDM_SON_String());

            IEnumerable<DTM_QAQC_BLK_STD> resumen = context.ExecuteQuery<DTM_QAQC_BLK_STD>(data).ToList();
            context.Dispose();
            return resumen;
        }





        /*########################## DEFINITION STANDARD #################################### */
        public object GetDefinitionSTD(DM_CC_SONEntities ddb)
        {

            var myList = ((from t1 in ddb.DTM_STANDARDSDEFINITION
                           select new
                           {
                               STANDARDID = t1.STANDARDID,
                               STANDARDDATE = t1.STANDARDDATE,
                               DESCRIPTION = t1.DESCRIPTION,
                               STANDARDTYPE = t1.STANDARDTYPE
                           })).OrderBy(x => x.STANDARDID).ToList();

            return myList;

        }
        /*################################## STANDARD #################################### */

        public object GetStdFy(DM_CC_SONEntities ddb)
        {

            var myList = ((from t1 in ddb.DTM_QAQC_BLK_STD
                           from t2 in ddb.DTM_STANDARDSASSAY.Where(x => t1.STANDARDID == x.STANDARDID & t1.ASSAYVALUE == x.STANDARDVALUE)
                           select new
                           {
                               CHECKID = t1.CHECKID,
                               STANDARDID = t1.STANDARDID,
                               PRIORITY = t1.ASSAY_PRIORITY,
                               NAME = t1.ASSAYNAME,
                               ASSAYVALUE = t1.ASSAYVALUE,
                               STANDARDVALUE = t2.STANDARDVALUE,
                               STANDARDDEVIATION = t2.STANDARDDEVIATION,
                               LABJOBNO = t1.LABJOBNO,
                               RETURNDATE = t1.RETURNDATE,
                               NORMALIZACION = (t2.STANDARDVALUE - t1.ASSAYVALUE) / t2.STANDARDDEVIATION

                           })).OrderBy(x => x.RETURNDATE).ToList();

            return myList;

        }
        /*################################## BLK #################################### */
        public object GetBlkFy(DM_CC_SONEntities ddb)
        {

            var myList = ((from l in ddb.DTM_QAQC_BLK_STD
                           where l.STANDARDID.StartsWith("B")
                           select new
                           {
                               CHECKID = l.CHECKID,
                               STANDARDID = l.STANDARDID,
                               PRIORITY = l.ASSAY_PRIORITY,
                               NAME = l.ASSAYNAME,
                               ASSAYVALUE = l.ASSAYVALUE,
                               STANDARDVALUE = l.ASSAYVALUE,
                               STANDARDDEVIATION = l.ASSAYVALUE,
                               LABJOBNO = l.LABJOBNO,
                               LOADDATE = l.SENDDATE
                           })).OrderBy(x => x.LOADDATE).ToList();

            return myList;

        }

        /*################################## DUP #################################### */
        public object GetDupFy(DM_CC_SONEntities ddb)
        {

            var myList = ((from l in ddb.DTM_QAQC_DUP
                           where l.HOLEID.StartsWith("RC-18-")
                           select new
                           {
                               ID_OR = l.ID_OR,
                               ID_CK = l.ID_CK,
                               HOLEID = l.HOLEID,
                               Desde = l.SAMPFROM,
                               Hasta = l.SAMPTO,

                               T_MIN = l.T_MIN,
                               SAMPLE_DRILLTYPE = l.SAMPLE_DRILLTYPE,
                               ASSAYNAME = l.ASSAYNAME,
                               ASSAYVALUE_OR = l.ASSAYVALUE_OR,
                               ASSAYVALUE_CK = l.ASSAYVALUE_CK,

                               ASSAYPRIORITY_OR = l.PRIORITY_OR,
                               ASSAYPRIORITY_CK = l.PRIORITY_CK,
                               LABCODE_OR = l.LABJOBNO_OR,
                               LABCODE_CK = l.LABJOBNO_CK,

                               SENDDATE = l.SENDDATE,

                               ANALYSISSUITE = l.ANALYSISSUITE,
                               LABJOBNO_OR = l.LABJOBNO_OR,
                               LABJOBNO_CK = l.LABJOBNO_CK,
                               RETURNDATE = l.RETURNDATE


                           })).ToList().Take(80000);

            return myList;

        }

























    }
}
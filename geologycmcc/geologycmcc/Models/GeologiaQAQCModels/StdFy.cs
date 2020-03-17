using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.GeologiaQAQCModels
{
    public class StdFy
    {

        public object GetStdFy(DM_CC_SONEntities ddb)
        {

            try
            {
                var myList = ((from t1 in ddb.DTM_QAQC_BLK_STD
                               from t2 in ddb.DTM_STANDARDSASSAY.Where(x => t1.STANDARDID == x.STANDARDID && t1.ASSAYVALUE == x.STANDARDVALUE)
                               select new
                               {
                                   CHECKID = t1.CHECKID,
                                   STANDARDID = t1.STANDARDID,
                                   ASSAY_PRIORITY = t1.ASSAY_PRIORITY,
                                   ASSAYNAME = t1.ASSAYNAME,
                                   ASSAYVALUE = t1.ASSAYVALUE,
                                   STANDARDVALUE = t2.STANDARDVALUE,
                                   STANDARDDEVIATION = t2.STANDARDDEVIATION,
                                   NORMALIZACION = (t2.STANDARDVALUE - t1.ASSAYVALUE) / t2.STANDARDDEVIATION,
                                   LABJOBNO = t1.LABJOBNO,
                                   RETURNDATE = t1.RETURNDATE


                               })).OrderBy(x => x.RETURNDATE).ToList();

                return myList;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}
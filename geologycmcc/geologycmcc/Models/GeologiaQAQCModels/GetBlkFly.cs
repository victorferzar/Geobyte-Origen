using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.GeologiaQAQCModels
{
    public class GetBlkFly
    {
        public object GetBlkFy(DM_CC_SONEntities ddb)
        {
            try
            {
                var myList = ((from l in ddb.DTM_QAQC_BLK_STD
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
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Models.GeologiaQAQCModels;


namespace geologycmcc.Controllers.data
{
    public class TablesData
    {
        DM_CC_SONEntities ddb = new DM_CC_SONEntities();
        public object GetQAQCDup()
        {


            var myList = ((from l in ddb.DTM_QAQC_DUP
                           where l.HOLEID.StartsWith("RC-18-")
                           select new
                           {

                               l.HOLEID,
                               l.ANALYSISSUITE,
                               l.SAMPFROM,
                               l.SAMPTO,
                               l.ASSAYVALUE_OR,
                               l.ASSAYVALUE_CK,
                               l.DIFERENCIA,
                               l.VAR_REL,
                               l.AMPD,
                               l.PROMEDIO,
                               l.MPD
                           })).ToList();

            return myList;

        }
    }
}
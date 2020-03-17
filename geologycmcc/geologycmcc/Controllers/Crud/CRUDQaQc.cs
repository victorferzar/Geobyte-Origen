using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Controllers.DataBase;
using geologycmcc.Models.QaQcModels;
using System.Web.Mvc;

namespace geologycmcc.Controllers.Crud
{

    public class CRUDQaQc
    {

        TransaccionesQaQc tr;

        /*############################################### VIEW GRAFICOS #############################################*/

        //View INDEX
        public IEnumerable<QaQcDup> QaQcDup()
        {
            tr = new TransaccionesQaQc();
            IEnumerable<QaQcDup> resultado = tr.dataQaQcDup("SELECT *  FROM [DM_CC_SON].[dbo].[DTM_QAQC_DUP]");            
            return resultado;
        }




    }
}
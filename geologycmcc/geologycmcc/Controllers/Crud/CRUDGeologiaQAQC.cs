using geologycmcc.Controllers.DataBase;
using geologycmcc.Models.GeologiaQAQCModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Controllers.Crud
{
    public class CRUDGeologiaQAQC
    {
        TransaccionesGeologiaQAQC tr;
        public IEnumerable<FiscalYear> ResListFiscalYear ()
        {
            tr = new TransaccionesGeologiaQAQC();
            IEnumerable<FiscalYear> resultado = tr.dataListFY("SELECT DISTINCT(FISCAL_YEAR) FROM DTM_COLLAR");

            return resultado;
        }


        public IEnumerable<Laboratorio> ResListLaboratorio(string FISCAL_YEAR, string DESDE, string HASTA)
        {
            tr = new TransaccionesGeologiaQAQC();
           
                double fy = Double.Parse(FISCAL_YEAR);
                IEnumerable<Laboratorio> resultado = tr.dataLaboratorio("SELECT DISTINCT(LABCODE)  FROM DTM_QAQC_BLK_STD WHERE HOLEID IN (SELECT HOLEID FROM DTM_COLLAR WHERE FISCAL_YEAR =" + fy + ")");
             
            

            return resultado;
        }

       


    }
}
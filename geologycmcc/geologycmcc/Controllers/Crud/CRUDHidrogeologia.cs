using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Controllers.DataBase;
using geologycmcc.Models.HidrogeologiaModels;
using System.Web.Mvc;

namespace geologycmcc.Controllers.Crud
{

    public class CRUDHidrogeologia
    {

        TransaccionesHidrogeologia tr;

        /*############################################### VIEW GRAFICOS #############################################*/

        //View INDEX
        public IEnumerable<ResumenHidrogeologia> ResumenHigrogeologia()
        {
            tr = new TransaccionesHidrogeologia();
            IEnumerable<ResumenHidrogeologia> resultado = tr.dataResumenHidrogeologia("SELECT * FROM DTM_DATAHIDROGEOLOGICA ORDER BY HOLEID");
            
            return resultado;
        }




        public List<SelectListItem> DDLSondajes()
        {
            tr = new TransaccionesHidrogeologia();
            IEnumerable<ResumenHidrogeologia> resultado = tr.dataResumenHidrogeologia("SELECT * FROM DTM_DATAHIDROGEOLOGICA ORDER BY HOLEID");
            List<string> data = resultado.Select(x => x.HOLEID).Distinct().ToList();
            List<SelectListItem> lista = data.Select(x => new SelectListItem { Value = x, Text = x }).Distinct().ToList();

            return lista;
        }
    }
}
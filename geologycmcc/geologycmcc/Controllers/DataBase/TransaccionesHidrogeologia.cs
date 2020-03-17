using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Models.HidrogeologiaModels;
using System.Data.Linq;

namespace geologycmcc.Controllers.DataBase
{
    public class TransaccionesHidrogeologia
    {

        private Conexion cnLocalBD;

        public IEnumerable<ResumenHidrogeologia> dataResumenHidrogeologia(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionDM_SON_String());

            IEnumerable<ResumenHidrogeologia> resumen = context.ExecuteQuery<ResumenHidrogeologia>(data).ToList();
            context.Dispose();
            return resumen;
        }
    }
}
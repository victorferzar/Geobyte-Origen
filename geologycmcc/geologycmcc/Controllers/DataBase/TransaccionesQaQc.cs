using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Models.QaQcModels;
using System.Data.Linq;

namespace geologycmcc.Controllers.DataBase
{
    public class TransaccionesQaQc
    {

        private Conexion cnLocalBD;

        public IEnumerable<QaQcDup> dataQaQcDup(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionDM_SON_String());

            IEnumerable<QaQcDup> resumen = context.ExecuteQuery<QaQcDup>(data).ToList();
            context.Dispose();
            return resumen;
        }
    }
}
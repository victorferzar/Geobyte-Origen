using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Models.GeologiaModels;
using System.Data.Linq;

namespace geologycmcc.Controllers.DataBase
{
    public class TransaccionesGeologia
    {


        private Conexion cnLocalBD;

        public IEnumerable<StockControlesSOND> dataStockControlSOND(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionDM_SON_String());

            IEnumerable<StockControlesSOND> resumen = context.ExecuteQuery<StockControlesSOND>(data).ToList();
            context.Dispose();
            return resumen;
        }
    }
}
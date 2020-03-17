using geologycmcc.Models.GeotecniaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;

namespace geologycmcc.Controllers.DataBase
{
    public class TransaccionesGeotecnia
    {

        private Conexion cnLocalBD;

        public RigeotReporte dataReporteBANGEOT(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionGeotecnia());

            RigeotReporte resumen = context.ExecuteQuery<RigeotReporte>(data).First();
            context.Dispose();
            return resumen;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Controllers.DataBase;
using System.Data.Linq;
using geologycmcc.Models.DrillingModels;

namespace geologycmcc.Controllers.DataBase
{
    public class Transacciones
    {
        private Conexion cnLocalBD;

        public IEnumerable<MResumenPerforacionFY> dataResumenPerforacion(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionBDSON());
            
            IEnumerable<MResumenPerforacionFY> resumen = context.ExecuteQuery<MResumenPerforacionFY>(data).ToList();
            context.Dispose();
            return resumen;
        }

        public IEnumerable<MRFechaMetros> fechaMetros(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionString());

            IEnumerable<MRFechaMetros> resumen = context.ExecuteQuery<MRFechaMetros>(data).ToList();
            context.Dispose();
            return resumen;
        }


        public IEnumerable<PerforacionDiaria> perforacionDia(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionBDSON());

            IEnumerable<PerforacionDiaria> resumen = context.ExecuteQuery<PerforacionDiaria>(data).ToList();
            context.Dispose();
            
            return resumen;
        }
        public IEnumerable<SondaUbicacion> perforaciondiasonda(String data) {

            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionBDSON());
            IEnumerable<SondaUbicacion> resumen = context.ExecuteQuery<SondaUbicacion>(data).ToList();
            context.Dispose();

            return resumen;

        }
    }
}
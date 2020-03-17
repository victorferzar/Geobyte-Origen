using geologycmcc.Models.GeometalurgiaModels;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace geologycmcc.Controllers.DataBase
{
    public class TransaccionesGeometalurgia
    {
        private Conexion cnLocalBD;

        public IEnumerable<RecuperacionPondTonelaje> dataRecuperacionPondTonelaje(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionBDGM());

            IEnumerable<RecuperacionPondTonelaje> resumen = context.ExecuteQuery<RecuperacionPondTonelaje>(data).ToList();
            context.Dispose();
            return resumen;
        }
        public IEnumerable<BusquedaPilas> DataBusquedaPilas(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionBDGM());

            IEnumerable<BusquedaPilas> resumen = context.ExecuteQuery<BusquedaPilas>(data).ToList();
            context.Dispose();
            return resumen;
        }

        public IEnumerable<ResumenPilas> ResumenPilas(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionBDGM());

            IEnumerable<ResumenPilas> resumen = context.ExecuteQuery<ResumenPilas>(data).ToList();
            context.Dispose();
            return resumen;
        }

        public IEnumerable<MineralPila> MineralPila(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionBDGM());

            IEnumerable<MineralPila> resumen = context.ExecuteQuery<MineralPila>(data).ToList();
            context.Dispose();
            return resumen;
        }
    }
}
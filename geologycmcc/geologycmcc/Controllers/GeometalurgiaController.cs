using geologycmcc.Controllers.Crud;
using geologycmcc.Controllers.Graficos;
using geologycmcc.Models.GeometalurgiaModels;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace geologycmcc.Views
{
    public class GeometalurgiaController : Controller
    {
        

        // GET: Geometalurgia
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AllViewGeomet()
        {
            GraficosDrilling gd = new GraficosDrilling();
            ViewBag.GResumenPerforaciónFY = gd.GFResumenPerforacionFY();
            ViewBag.GBacklog = gd.GFBackLog();
            return View();
        }

        public ActionResult RecuperacionPila()
        {
            //Index de GraficosRecuperacionPonderadoTonelaje
            return View();
        }
        public ActionResult GraficosRecuperacionPonderadoTonelaje(String holeid)
        {
        
           
            CRUDGeometalurgia cd = new CRUDGeometalurgia();
            //Detalle Carga Diaria Cut Cortador Cut Ripios
            IEnumerable<RecuperacionPondTonelaje> data = cd.ponderacionTonelaje(holeid.ToUpper());
            var lista = from p in data select p;
            //resumen Pila
            IEnumerable<ResumenPilas> dataResumen = cd.ResumenPilas(holeid.ToUpper());  
            var listaResumen = from p in dataResumen select p;
            //Detalle Mineral
            IEnumerable<MineralPila> dataMineral = cd.MineralPila(listaResumen.Select(x => x.PLANTA).Max(), listaResumen.Select(x => x.INICIOCARGA).Max(), listaResumen.Select(x => x.TERMINOCARGA).Max());
            var listaMineral = from p in dataMineral select p;
                        
            //ViewBag.Tmin = listaMineral.GroupBy(x => x.T_MIN_MINA).Select(group => group.First()).ToList();
            ViewBag.Tmin = listaMineral.GroupBy(d =>1).Select(g => new MineralPila
            {
            TONDIA = g.Sum(s => s.TONDIA),
            SULFURO = g.Sum(s => s.SULFURO),
            OXIDO = g.Sum(s => s.OXIDO),
            });

            var dataJoin = lista.Join(listaMineral,
             post => post.FECHACARGA,
             meta => meta.FECHAMOVIMIENTO, (post, meta) => new { post, meta }).Select(z => new MineralRecuperacion
             {
                 CuT_CORTADOR = z.post.CuT_CORTADOR,
                 CuS_CORTADOR = z.post.CuS_CORTADOR,
                 CuT_RIPIOS = z.post.CuT_RIPIOS,
                 FECHACARGA = z.post.FECHACARGA,
                 Rec_CuT_Fecha_Carga = z.post.Rec_CuT_Fecha_Carga,
                 TONELAJEDIARIO = z.post.TONELAJEDIARIO,
                 FECHAMOVIMIENTO = z.meta.FECHAMOVIMIENTO,
                 MSH = z.meta.MSH,
                 MSHB = z.meta.MSHB,
                 MSHM = z.meta.MSHM,
                 OXIDO = z.meta.OXIDO,
                 SULFURO = z.meta.SULFURO,
                 TONDIA = z.meta.TONDIA

             }).ToList();
            try
            {

               var control = dataJoin;


                ViewBag.detalleResumen = listaResumen.ToList();

                var application = new Application();
                var worksheetFunction = application.WorksheetFunction;

                double sumaTotalTonelaje = lista.Select(x => x.TONELAJEDIARIO).Sum();
                ViewBag.sumaTotalTonelaje = sumaTotalTonelaje;



                ViewBag.CuTFinoIN = worksheetFunction.SumProduct(lista.Where(x => x.CuT_RIPIOS != 0).Select(x => x.CuT_CORTADOR).ToArray(), lista.Where(x => x.CuT_RIPIOS != 0).Select(x => x.TONELAJEDIARIO).ToArray());
                ViewBag.CuTFinoOUT = worksheetFunction.SumProduct(lista.Where(x => x.CuT_RIPIOS != 0).Select(x => x.CuT_RIPIOS).ToArray(), lista.Where(x => x.CuT_RIPIOS != 0).Select(x => x.TONELAJEDIARIO).ToArray());

                ViewBag.CutRecuperado_PILA = (((double)((int)(((ViewBag.CuTFinoIN - ViewBag.CuTFinoOUT) / ViewBag.CuTFinoIN * 100) * 1000.0))) / 1000.0);

                GraficosGeomet gm = new GraficosGeomet();

                // ViewBag.Graf1RecTotPila = gm.RecTotalCutPila(holeid, lista.Select(x => x.FECHACARGA.ToString().Substring(0,10)).ToArray(), lista.Select(x => x.Rec_CuT_Fecha_Carga.ToString()).Cast<object>().ToArray());
                ViewBag.Graf1RecTotPila = gm.RecTotalCutPila(holeid, dataJoin.Select(x => x.FECHACARGA.ToString().Substring(0, 10)).ToArray(), dataJoin);

                //           ViewBag.Graf2RecTotPila= gm.RecTotalTmin(holeid, dataJoin.Select(x => x.FECHACARGA.ToString().Substring(0, 10)).ToArray(), dataJoin);
                ViewBag.HOleid = holeid.ToUpper();
                ViewBag.detalle = control;

            }catch (Exception ex){
               //Excepcion sin control
            }


            return View();
        }
        public ActionResult ObtenerPilas(string term)
        {
            CRUDGeometalurgia cd = new CRUDGeometalurgia();
            IEnumerable<BusquedaPilas> data = cd.ListaSondajes();
            var lista = from p in data select p;
            return Json(lista.Where(c => c.HOLEID.Contains(term.ToUpper())).Select(a => new { label = a.HOLEID }), JsonRequestBehavior.AllowGet);
        }


        public ActionResult testc() {

            return View();
        }

    }
}
using System;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;

using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System.Drawing;
using geologycmcc.Controllers.Graficos;
using geologycmcc.Controllers.Crud;
using geologycmcc.Controllers.data;
using System.Collections.Generic;
using geologycmcc.Models.DrillingModels;
//test git
namespace geologycmcc.Controllers
{
    [Authorize(Roles = @"Americas\chachl9
,Americas\verarc9
,Americas\sancjc
,Americas\bustce
,AMERICAS\estacd,
AMERICAS\mereg,
AMERICAS\valljb,
AMERICAS\escofa,
AMERICAS\quinva,
AMERICAS\alvapa2,
AMERICAS\bricpc,
AMERICAS\arcema,
AMERICAS\silvd,
AMERICAS\hidadf,
AMERICAS\sepuy,
AMERICAS\poolra,
AMERICAS\cespjl,
AMERICAS\escaj,
AMERICAS\becec,
AMERICAS\aedol,
AMERICAS\ferrra,
AMERICAS\becec,
AMERICAS\matih,
AMERICAS\silvfj9,
AMERICAS\guzmja,
AMERICAS\BUSTCE,
AMERICAS\yanen9,
AUSTEM_BEAST1\Austen")]
    public class DrillingController : Controller
    {
        // GET: Drilling
        public ActionResult Index()
        {

            GraficosDrilling gd= new GraficosDrilling();


            ViewBag.GResumenPerforaciónFY = gd.GFResumenPerforacionFY();
            ViewBag.GBacklog = gd.GFBackLog();


            return View();
        }


        public ActionResult PerforacionDiaria()
        {
            CRUDDrilling trans = new CRUDDrilling();

            ViewBag.Perforacionxdia = trans.PerforacionDiaria();

            return View();
        }

        public ActionResult PlanoPerforacion()
        {

            ConversorCoordenadas cc = new ConversorCoordenadas();
   

            CRUDDrilling trans = new CRUDDrilling();

            IEnumerable<SondaUbicacion> maquinas = trans.SondaPerforacion().ToList();

            String[,] datamaquinas= new String[maquinas.Count(), 6];
            int nmaquina = 0;

            foreach (SondaUbicacion x in maquinas) {
                // Create a point
                String punto = "var point" +(++nmaquina)+" = new Point("+ cc.conversor(x.ESTE_GIS, x.NORTE_GIS) +");";
                String puntoMaquina = "var pointGraphicMaquina" + nmaquina + " = new Graphic({geometry: point"+nmaquina+", symbol: Symbol });";
                String textoPrincipalFormato = "var textSymbol"+nmaquina + " = new TextSymbol(";
                String textoPrincipal = "var pointGraphic"+nmaquina+" = new Graphic({geometry: point"+nmaquina+", symbol: textSymbol"+nmaquina+"});";
                String agregar = "view.graphics.add(pointGraphicMaquina" + nmaquina + ");view.graphics.add(pointGraphic" + nmaquina + ");";

                datamaquinas[nmaquina - 1, 0] = punto;
                datamaquinas[nmaquina - 1, 1] = puntoMaquina;
                datamaquinas[nmaquina - 1, 2] = textoPrincipalFormato;
       
                datamaquinas[nmaquina - 1, 3] = textoPrincipal;
                datamaquinas[nmaquina - 1 ,4] = agregar;
        
                datamaquinas[nmaquina - 1, 5] = x.SONDA + "  " + x.HOLEID ;

            }
            ViewBag.fil = nmaquina;
            ViewBag.Maquinas = datamaquinas;
         

            return View();
        }




    }
}
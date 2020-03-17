using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using geologycmcc.Controllers.Graficos;
using geologycmcc.Controllers.Crud;
using geologycmcc.Models.HidrogeologiaModels;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace geologycmcc.Controllers
{
    [Authorize(Roles = @"Americas\chachl9,Americas\sancjc,Americas\verarc9,Americas\bustce,Americas\corrjb,Americas\salabf9,Americas\alvama4,Americas\cifupg,AMERICAS\estacd,
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
AMERICAS\MORCBA,
AMERICAS\becec,
AMERICAS\matih,
AMERICAS\BUSTCE,
AMERICAS\yanen9")]
    public class HidrogeologiaController : Controller
    {
        // GET: Hidrogeologia
        public ActionResult Index()
        {
            {

                CRUDHidrogeologia crudhg = new CRUDHidrogeologia();

                ViewBag.ListaSondajes = crudhg.DDLSondajes();
                ViewBag.HOLEIDHIDRO = "";


                return View();
            }
        }

        public ActionResult GraficosHidrogeologia(String HOLEID) {
            ViewBag.Sondaje = HOLEID;

            //############################### Consulta a BD ###############################################
            CRUDHidrogeologia cd = new CRUDHidrogeologia();
            IEnumerable<ResumenHidrogeologia> data = cd.ResumenHigrogeologia();

            //############################### CONDUCTIVIDAD ELECTRICA ###############################################
            var listaConductivity = from p in data
                                    orderby p.Fecha
                                    where p.HOLEID.Equals(HOLEID)
                                    select new
                                    {

                                        p.Hidro_Conductivity,
                                        p.Hidro_Conductivity_Umbral,
                                        p.Fecha
                                    };
            object[] arrCond = new object[listaConductivity.Count()];
            object[] arrCondU = new object[listaConductivity.Count()];
            String[] arrCondF = new String[listaConductivity.Count()];

            for (int i = 0; i < listaConductivity.Count(); i++)
            {
                arrCond[i] = listaConductivity.ElementAt(i).Hidro_Conductivity;
                arrCondU[i] = String.IsNullOrEmpty(listaConductivity.ElementAt(i).Hidro_Conductivity_Umbral.ToString()) && listaConductivity.ElementAt(i).Hidro_Conductivity >= 0 && i >0 ? arrCondU[i - 1] : listaConductivity.ElementAt(i).Hidro_Conductivity_Umbral;
                arrCondF[i] = listaConductivity.ElementAt(i).Fecha.ToString(System.Globalization.CultureInfo.GetCultureInfo("es-CL")).Replace("0:00:00", "");
            }


            GraficosHidrogeologia gh = new GraficosHidrogeologia();

            ViewBag.GraficoConductivity = gh.graficoHidro("Conductivity", "Conductividad Eléctrica", "Fecha", arrCondF, "uS/cm", arrCond, arrCondU, "Hidro_Conductivity", "Hidro_Conductivity_Umbral", HOLEID);

            //############################### CONCENTRACION CU ###############################################
            var listaConcentracioncu = from p in data
                                       orderby p.Fecha
                                       where p.HOLEID.Equals(HOLEID)
                                       select new
                                       {

                                           p.Hidro_Cu_mgl,
                                           p.Hidro_Cu_mgl_Umbral,
                                           p.Fecha
                                       };

            object[] arrConCu = new object[listaConcentracioncu.Count()];
            object[] arrConCuU = new object[listaConcentracioncu.Count()];
            String[] arrConCuF = new String[listaConcentracioncu.Count()];
            for (int i = 0; i < listaConcentracioncu.Count(); i++)
            {
                arrConCu[i] = listaConcentracioncu.ElementAt(i).Hidro_Cu_mgl;
                arrConCuU[i] = String.IsNullOrEmpty(listaConcentracioncu.ElementAt(i).Hidro_Cu_mgl_Umbral.ToString()) && listaConcentracioncu.ElementAt(i).Hidro_Cu_mgl >= 0 && i > 0 ? arrConCuU[i - 1] : listaConcentracioncu.ElementAt(i).Hidro_Cu_mgl_Umbral;
                arrConCuF[i] = listaConcentracioncu.ElementAt(i).Fecha.ToString(System.Globalization.CultureInfo.GetCultureInfo("es-CL")).Replace("0:00:00", "");
            }

            gh = new GraficosHidrogeologia();

            ViewBag.GraficoConcentracionCu = gh.graficoHidro("ConcentracionCu", "Concentración Cu", "Fecha", arrConCuF, "mg/L", arrConCu, arrConCuU, "Hidro_Cu_mgl", "Hidro_Cu_mgl_Umbral", HOLEID);



            //############################### PH ###############################################

            var listaSulfato = from p in data
                               orderby p.Fecha
                               where p.HOLEID.Equals(HOLEID)
                               select new
                               {
                                   p.Hidro_Ph_Uph,
                                   p.Hidro_Ph_Uph_Umbral,
                                   p.Fecha
                               };


            object[] arrSulf = new object[listaSulfato.Count()];
            object[] arrSulfU = new object[listaSulfato.Count()];
            String[] arrsulfF = new String[listaSulfato.Count()];
            for (int i = 0; i < listaSulfato.Count(); i++)
            {
                arrSulf[i] = listaSulfato.ElementAt(i).Hidro_Ph_Uph;
                arrSulfU[i] = String.IsNullOrEmpty(listaSulfato.ElementAt(i).Hidro_Ph_Uph_Umbral.ToString()) && listaSulfato.ElementAt(i).Hidro_Ph_Uph >= 0 && i > 0 ? arrSulfU[i - 1] : listaSulfato.ElementAt(i).Hidro_Ph_Uph_Umbral;
                arrsulfF[i] = listaSulfato.ElementAt(i).Fecha.ToString(System.Globalization.CultureInfo.GetCultureInfo("es-CL")).Replace("0:00:00", "");

            }
            gh = new GraficosHidrogeologia();

            ViewBag.GraficoSulfato = gh.graficoHidro("Sulfato", "PH", "Fecha", arrConCuF, "Ph", arrSulf, arrSulfU, "Hidro_Ph_Uph", "Hidro_Ph_Uph_Umbral", HOLEID);



            //############################### CONCENTRACION SULFATO ###############################################

            var listaConsSulfato = from p in data
                                   orderby p.Fecha
                                   where p.HOLEID.Equals(HOLEID)
                                   select new
                                   {
                                       p.Hidro_Sul_mgl,
                                       p.Hidro_Sul_mgl_Umbral,
                                       p.Fecha
                                   };


            object[] arrConsSulf = new object[listaConsSulfato.Count()];
            object[] arrConsSulfU = new object[listaConsSulfato.Count()];
            String[] arrConsSulfF = new String[listaConsSulfato.Count()];

            for (int i = 0; i < listaConsSulfato.Count(); i++)
            {
                arrConsSulf[i] = listaConsSulfato.ElementAt(i).Hidro_Sul_mgl;
                arrConsSulfU[i] = String.IsNullOrEmpty(listaConsSulfato.ElementAt(i).Hidro_Sul_mgl_Umbral.ToString()) && listaConsSulfato.ElementAt(i).Hidro_Sul_mgl >= 0 && i > 0 ? arrConsSulfU[i - 1] : listaConsSulfato.ElementAt(i).Hidro_Sul_mgl_Umbral;
                arrConsSulfF[i] = listaConsSulfato.ElementAt(i).Fecha.ToString(System.Globalization.CultureInfo.GetCultureInfo("es-CL")).Replace("0:00:00", "");
            }


            gh = new GraficosHidrogeologia();
            ViewBag.GraficoConsSulfato = gh.graficoHidro("ConsSulfato", "Concentración Sulfato", "Fecha", arrConsSulfF, "mg/L", arrConsSulf, arrConsSulfU, "Hidro_Sul_mgl", "Hidro_Sul_mgl_Umbral", HOLEID);

            //############################### NIVEL FREATICO ###############################################


            var listaFreatico = from p in data
                                orderby p.Fecha
                                where p.HOLEID.Equals(HOLEID)
                                select new
                                {
                                    p.NivelFreatico,
                                    p.NivelFreatico_Umbral,
                                    p.Fecha
                                };

            object[] arrFrea = new object[listaFreatico.Count()];
            object[] arrFreaU = new object[listaFreatico.Count()];
            String[] arrFreaF = new String[listaFreatico.Count()];
            for (int i = 0; i < listaFreatico.Count(); i++)
            {
                arrFrea[i] = listaFreatico.ElementAt(i).NivelFreatico;
                arrFreaU[i] = String.IsNullOrEmpty(listaFreatico.ElementAt(i).NivelFreatico_Umbral.ToString()) && listaFreatico.ElementAt(i).NivelFreatico >= 0 && i > 0 ? arrFreaU[i - 1] : listaFreatico.ElementAt(i).NivelFreatico_Umbral;
                arrFreaF[i] = listaFreatico.ElementAt(i).Fecha.ToString(System.Globalization.CultureInfo.GetCultureInfo("es-CL")).Replace("0:00:00", "");

            }

            gh = new GraficosHidrogeologia();

            ViewBag.GraficoFreatico = gh.graficoHidro("Freatico", "Nivel Freatico", "Fecha", arrFreaF, "mbnst", arrFrea, arrFreaU, "NivelFreatico", "NivelFreatico_Umbral", HOLEID);


            return View();
        }


        [HttpGet]
        public ActionResult ExportToExcel(string holeid, string tipo)
        {
            CRUDHidrogeologia cd = new CRUDHidrogeologia();
            IEnumerable<ResumenHidrogeologia> data = cd.ResumenHigrogeologia();

            var listahidro = data.Where(x => x.HOLEID.Equals(holeid));

            var listaTipo = from p in listahidro
                            orderby p.Fecha
                            select new
                            {
                                p.HOLEID,
                                p.SAMPFROM,
                                p.Hidro_Conductivity,
                                p.Hidro_Conductivity_Umbral,
                                p.Hidro_Cu_mgl,
                                p.Hidro_Cu_mgl_Umbral,
                                p.Hidro_Ph_Uph,
                                p.Hidro_Ph_Uph_Umbral,
                                p.Hidro_Sul_mgl,
                                p.Hidro_Sul_mgl_Umbral,
                                p.NivelFreatico,
                                p.NivelFreatico_Umbral,
                                p.Fecha
                            };


            var products = new System.Data.DataTable("teste");
            products.Columns.Add("HOLEID", typeof(string));
            products.Columns.Add("SAMPFROM", typeof(string));
            products.Columns.Add("Hidro_" + tipo, typeof(string));
            products.Columns.Add("Hidro_" + tipo + "_Umbral", typeof(string));
            products.Columns.Add("FECHA", typeof(string));

            foreach (var x in listaTipo)
            {

                if (tipo == "Conductividad")
                {
                    products.Rows.Add(x.HOLEID, x.SAMPFROM, x.Hidro_Conductivity, x.Hidro_Conductivity_Umbral, x.Fecha);
                }
                else if (tipo == "Cu_mgl")
                {
                    products.Rows.Add(x.HOLEID, x.SAMPFROM, x.Hidro_Cu_mgl, x.Hidro_Cu_mgl_Umbral, x.Fecha);
                }
                else if (tipo == "Ph_Uph")
                {
                    products.Rows.Add(x.HOLEID, x.SAMPFROM, x.Hidro_Ph_Uph, x.Hidro_Ph_Uph_Umbral, x.Fecha);
                }
                else if (tipo == "Sul_mgl")
                {
                    products.Rows.Add(x.HOLEID, x.SAMPFROM, x.Hidro_Sul_mgl, x.Hidro_Sul_mgl_Umbral, x.Fecha);
                }
                else if (tipo == "NivelFreatico")
                {
                    products.Rows.Add(x.HOLEID, x.SAMPFROM, x.NivelFreatico, x.NivelFreatico_Umbral, x.Fecha);
                }
            }


            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + holeid + "_" + tipo + ".xls");
            Response.ContentType = "application/vnd.ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View("Export");
        }

        [HttpGet]
        public ActionResult ExportToExcelALL()
        {
            CRUDHidrogeologia cd = new CRUDHidrogeologia();
            IEnumerable<ResumenHidrogeologia> data = cd.ResumenHigrogeologia();

            //Data a exportar

            var listaTipo = from p in data
                            orderby p.Fecha
                            select new
                            {
                                p.HOLEID,
                                p.SAMPFROM,
                                p.Hidro_Bi_mgl,
                                p.Hidro_Bi_mgl_Umbral,
                                p.Hidro_Ca_mgl,
                                p.Hidro_Ca_mgl_Umbral,
                                p.Hidro_Cl_mgl,
                                p.Hidro_Cl_mgl_Umbral,
                                p.Hidro_Conductivity,
                                p.Hidro_Conductivity_Umbral,
                                p.Hidro_Cu_mgl,
                                p.Hidro_Cu_mgl_Umbral,
                                p.Hidro_Fe_mgl,
                                p.Hidro_Fe_mgl_Umbral,
                                p.Hidro_K_Uph,
                                p.Hidro_K_Uph_Umbral,
                                p.Hidro_Mn_mgl,
                                p.Hidro_Mn_mgl_Umbral,
                                p.Hidro_Na_Uph,
                                p.Hidro_Na_Uph_Umbral,
                                p.Hidro_Ph_Uph,
                                p.Hidro_Ph_Uph_Umbral,
                                p.Hidro_Sul_mgl,
                                p.Hidro_Sul_mgl_Umbral,
                                p.Hidro_Temp_mgl,
                                p.Hidro_Temp_mgl_Umbral,
                                p.NivelFreatico,
                                p.NivelFreatico_Umbral,
                                p.Fecha
                            };

            // Se crean los nombres de columnas

            var products = new System.Data.DataTable("teste");
            products.Columns.Add("HOLEID", typeof(string));
            products.Columns.Add("SAMPFROM", typeof(string));
            products.Columns.Add("Hidro_Bi_mgl", typeof(string));
            products.Columns.Add("Hidro_Bi_mgl_Umbral", typeof(string));
            products.Columns.Add("Hidro_Ca_mgl", typeof(string));
            products.Columns.Add("Hidro_Ca_mgl_Umbral", typeof(string));
            products.Columns.Add("Hidro_Cl_mg", typeof(string));
            products.Columns.Add("Hidro_Cl_mgl_Umbral", typeof(string));
            products.Columns.Add("Hidro_Conductivity", typeof(string));
            products.Columns.Add("Hidro_Conductivity_Umbral", typeof(string));
            products.Columns.Add("Hidro_Cu_mgl", typeof(string));
            products.Columns.Add("Hidro_Cu_mgl_Umbral", typeof(string));
            products.Columns.Add("Hidro_Fe_mgl", typeof(string));
            products.Columns.Add("Hidro_Fe_mgl_Umbral", typeof(string));
            products.Columns.Add("Hidro_K_Uph", typeof(string));
            products.Columns.Add("Hidro_K_Uph_Umbral", typeof(string));
            products.Columns.Add("Hidro_Mn_mgl", typeof(string));
            products.Columns.Add("Hidro_Mn_mgl_Umbral", typeof(string));
            products.Columns.Add("Hidro_Na_mgl", typeof(string));
            products.Columns.Add("Hidro_Na_mgl_Umbral", typeof(string));
            products.Columns.Add("Hidro_Ph_Uph", typeof(string));
            products.Columns.Add("Hidro_Ph_Uph_Umbral", typeof(string));
            products.Columns.Add("Hidro_Sul_mgl", typeof(string));
            products.Columns.Add("Hidro_Sul_mgl_Umbral", typeof(string));
            products.Columns.Add("Hidro_Temp_mgl", typeof(string));
            products.Columns.Add("Hidro_Temp_mgl_Umbral", typeof(string));
            products.Columns.Add("NivelFreatico", typeof(string));
            products.Columns.Add("NivelFreatico_Umbral", typeof(string));
            products.Columns.Add("FECHA", typeof(string));

            // Se asignan los valores en el excel

            foreach (var x in listaTipo){

             
                products.Rows.Add(x.HOLEID, 
                    x.SAMPFROM,
                    x.Hidro_Bi_mgl,x.Hidro_Bi_mgl_Umbral,
                    x.Hidro_Ca_mgl, x.Hidro_Ca_mgl_Umbral,
                    x.Hidro_Cl_mgl, x.Hidro_Cl_mgl_Umbral,
                    x.Hidro_Conductivity,  x.Hidro_Conductivity_Umbral, 
                    x.Hidro_Cu_mgl,x.Hidro_Cu_mgl_Umbral,
                    x.Hidro_Fe_mgl, x.Hidro_Fe_mgl_Umbral,
                    x.Hidro_K_Uph, x.Hidro_K_Uph_Umbral,
                    x.Hidro_Mn_mgl, x.Hidro_Mn_mgl_Umbral,
                    x.Hidro_Na_Uph, x.Hidro_Na_Uph_Umbral,
                    x.Hidro_Ph_Uph,x.Hidro_Ph_Uph_Umbral,
                    x.Hidro_Sul_mgl, x.Hidro_Sul_mgl_Umbral,
                    x.Hidro_Temp_mgl, x.Hidro_Temp_mgl_Umbral,
                    x.NivelFreatico,x.NivelFreatico_Umbral,
                    x.Fecha);
              
            }

            //Archivo salida

            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ALLHidrogeologia.xls");
            Response.ContentType = "application/vnd.ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View("Export");
        }
    }
}
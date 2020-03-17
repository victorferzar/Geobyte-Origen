using geologycmcc.Controllers.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using geologycmcc.Models.GeologiaQAQCModels;
using geologycmcc.Controllers.Crud.dataDDL;
using geologycmcc.Controllers.data;
using System.Globalization;
using geologycmcc.Controllers.Graficos;
using Point = DotNet.Highcharts.Options.Point;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using geologycmcc.Models.DataDuplicados;
using System.Drawing;
using Microsoft.Office.Interop.Excel;

using geologycmcc.Helpers;

namespace geologycmcc.Controllers
{
    [Authorize(Roles = @"Americas\chachl9,Americas\sancjc,Americas\verarc9,Americas\bustce,Americas\escofa,AMERICAS\estacd,
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
AMERICAS\moracb,
AMERICAS\BUSTCE,
AMERICAS\yanen9,
AUSTEM_BEAST1\Austen")]
    public class GeologiaQAQCController : Controller
    {

        DDLData d = new DDLData();
        TablesData t = new TablesData();
        IEnumerable<CQUAQCDUP> data = null;
        DataDuplicados dt = new DataDuplicados();

        private CRUDGeologiaQAQC crud;        
        StdFy stdFy = new StdFy();
        DataDuplicados dataStd = new DataDuplicados();  
       
        GraficosClass Graf = new GraficosClass();

        //    DM_CC_SONEntities ddb;



        // GET: Graphics
   



        static String FechaInicioQAQC = "01/07/2019";
        static String FechaFinQAQC = DateTime.Now.ToString("dd/MM/yyyy");
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Blancos(string[] StandardidLista = null, string PrioridadLista = null, string[] ElementosLista = null, string FechaDesde = null, string FechaHasta = null, string[] SuiteLista = null, string[] Lab = null)
        {
            ViewBag.StandardidLista = d.DDLListSTD("B");
            ViewBag.SuiteLista = d.DDLDespatchSuites("std");
            ViewBag.PrioridadLista = d.DDLListStatus();
            ViewBag.TipoStd = "Blancos";

            if (string.IsNullOrEmpty(FechaDesde))
            {
                return View("~/Views/GeologiaQAQC/FormFiltro.cshtml");
            }
            else
            {
                var datosTodos = dataStd.dataSTD();
                var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


                datosFiltrados = dataStd.IsNullOrEmpty(StandardidLista) ? datosFiltrados : datosFiltrados.Where(x => StandardidLista.Contains(x.STANDARDID)).Select(x => x);
                if (PrioridadLista == "1" || PrioridadLista == "2")
                {
                    datosFiltrados = datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
                }
                else if (PrioridadLista == "3")
                {
                    datosFiltrados = datosFiltrados.Where(x => x.ASSAY_PRIORITY >= Int32.Parse(PrioridadLista)).Select(x => x);
                }
                datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
                datosFiltrados = dataStd.IsNullOrEmpty(ElementosLista) ? datosFiltrados : datosFiltrados.Where(x => ElementosLista.Contains(x.ASSAYNAME)).Select(x => x);
                datosFiltrados = string.IsNullOrEmpty(FechaHasta) ? datosFiltrados : datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaDesde, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaHasta, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
                datosFiltrados = dataStd.IsNullOrEmpty(SuiteLista) ? datosFiltrados : datosFiltrados.Where(x => SuiteLista.Contains(x.ANALYSISSUITE)).Select(x => x);
                datosFiltrados = dataStd.IsNullOrEmpty(Lab) ? datosFiltrados : datosFiltrados.Where(x => Lab.Contains(x.LABCODE)).Select(x => x);
                var datosCount = datosFiltrados.Count();
                if (datosCount < 1) { return View(); }
                double WarnCount = datosFiltrados.Where(x => Math.Abs(Convert.ToDouble(x.ASSAYVALUE)) > 0.006).Count();
                double ErrorCount = datosFiltrados.Where(x => Math.Abs(Convert.ToDouble(x.ASSAYVALUE)) > 0.01).Count();
                double DevEst = dataStd.CalculateStdDev(datosFiltrados.Select(x => Convert.ToDouble(x.ASSAYVALUE)));
                double? Mean = datosFiltrados.Select(x => x.ASSAYVALUE).Average();
                double Median = dataStd.Median(datosFiltrados.Select(x => Convert.ToDouble(x.ASSAYVALUE)));
                double ErrorDev = DevEst / Math.Sqrt(datosCount);
                //Datos Tabla Detalle
                ViewBag.tablaDetalle = datosFiltrados;
                //Datos Tabla Resumen
                ViewBag.Total = datosCount;
                ViewBag.TotalWarn = WarnCount;
                ViewBag.TotalError = ErrorCount;
                ViewBag.PctError = Math.Round((ErrorCount * 100) / datosCount, 3);
                ViewBag.Mean = Math.Round(Convert.ToDouble(Mean), 3);
                ViewBag.Median = Math.Round(Median, 3);
                ViewBag.Min = datosFiltrados.Select(x => x.ASSAYVALUE).Min();
                ViewBag.Max = datosFiltrados.Select(x => x.ASSAYVALUE).Max();
                ViewBag.PctDevStd = Math.Round(Convert.ToDouble((DevEst / Mean) * 100), 3);
                ViewBag.DevStd = Math.Round(DevEst, 3);
                ViewBag.ErrorDev = Math.Round(ErrorDev, 3);
                ViewBag.PctErrorDevStd = Math.Round(Convert.ToDouble((ErrorDev / Mean) * 100), 3);
                ViewBag.Bias = Math.Round(Convert.ToDouble((Mean / datosFiltrados.First().STANDARDVALUE) - 1), 3);
                ViewBag.BiasMean = Math.Round(Convert.ToDouble((Mean / datosFiltrados.First().STANDARDVALUE) - 1), 3) * 100;
                return View(Graf.ChartIRM(datosFiltrados, "BLANK", FechaDesde, FechaHasta, "Grafico1"));

                
            }
        }

        public ActionResult AllView()
        {

            // Gráficos Standar Normalizado
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Contains("ST")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Contains("CuT_CMCCAAS_pct")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact("01/07/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            //datosFiltrados = dataStd.IsNullOrEmpty(SuiteLista) ? datosFiltrados : datosFiltrados.Where(x => SuiteLista.Contains(x.ANALYSISSUITE)).Select(x => x);
            var datosCount = datosFiltrados.Count();

            // Gráficos Blanco Normalizado
            var datosTodos1 = dataStd.dataSTD();
            var datosFiltrados1 = datosTodos1.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);
            datosFiltrados1 = datosFiltrados1.Where(x => x.STANDARDID.Contains("B")).Select(x => x);
            datosFiltrados1 = datosFiltrados1.Where(x => x.ASSAYNAME.Contains("CuT_CMCCAAS_pct")).Select(x => x);
            datosFiltrados1 = datosFiltrados1.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact("01/07/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            //datosFiltrados = dataStd.IsNullOrEmpty(SuiteLista) ? datosFiltrados : datosFiltrados.Where(x => SuiteLista.Contains(x.ANALYSISSUITE)).Select(x => x);
            var datosCount1 = datosFiltrados1.Count();

            IEnumerable<CQUAQCDUP> lista;

            //Carga los datos en el filtro

            lista = dt.dataDuplicados().Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact("01/07/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            var ProjectList = new string[] { "IN-FILL", "GEOMET", "CP" };
            lista = lista.Where(x => ProjectList.Contains(x.PROJECTCODE));
            lista = lista.Where(x => x.ASSAYNAME.Contains("CuT_CMCCAAS_pct"));
            lista = lista.Where(x => (x.ASSAYVALUE_CK + x.ASSAYVALUE_OR) / 2 > 0.01);
            var listaDupP = lista.Where(x => x.CHECKSTAGE.Equals("P")).ToList();
            var listaDupS = lista.Where(x => x.CHECKSTAGE.Equals("S")).ToList();
            var listaDupS2 = lista.Where(x => x.CHECKSTAGE.Equals("S2")).ToList();
            var listaDupC = lista.Where(x => x.CHECKSTAGE.Equals("C")).ToList();

            //Cantidad de elementos a mostrar / Cada muestra tendra 8 posiciones

            //Grafico de Puntos
            object[] arrDupP;
            object[] arrDupS;
            object[] arrDupS2;
            object[] arrDupC;

            //Frecuencia acumulada Ordenada
            object[] PlistaAMPDord;
            object[] SlistaAMPDord;
            object[] S2listaAMPDord;
            object[] ClistaAMPDord;
            //Frecuencia acumulada Ponderada


            IEnumerable<CQUAQCDUP> glistaP = null;
            IEnumerable<CQUAQCDUP> glistaS = null;
            IEnumerable<CQUAQCDUP> glistaS2 = null;
            IEnumerable<CQUAQCDUP> glistaC = null;


            Highcharts chart2;
            int numerographics = 0;
            int numerotablesR = 0;
            int numerotablesD = 0;

            glistaP = listaDupP;
            glistaS = listaDupS;
            glistaS2 = listaDupS2;
            glistaC = listaDupC;

            //Puntos Disperción
            arrDupP = new object[glistaP.Count()];
            arrDupS = new object[glistaS.Count()];
            arrDupS2 = new object[glistaS2.Count()];
            arrDupC = new object[glistaC.Count()];

            //Grafico F. Acumulada AMPDORD
            PlistaAMPDord = new object[glistaP.Count()];
            SlistaAMPDord = new object[glistaS.Count()];
            S2listaAMPDord = new object[glistaS2.Count()];
            ClistaAMPDord = new object[glistaC.Count()];




            glistaP = dt.dataDuplicados(glistaP);
            glistaS = dt.dataDuplicados(glistaS);
            glistaS2 = dt.dataDuplicados(glistaS2);
            glistaC = dt.dataDuplicados(glistaC);

            int n = 0;


            n = 0;
            var LabelFlag = 0;
            foreach (var i in glistaP)
            {
                if (i.NPORCDATOS >= 90)
                { LabelFlag++; }
                PlistaAMPDord[n] = new DotNet.Highcharts.Options.Point
                {

                    Y = Math.Round(Convert.ToDouble(i.NAMPORD), 2),
                    X = i.NPORCDATOS,
                    DataLabels = new PlotOptionsLineDataLabels
                    {
                        Enabled = i.NPORCDATOS >= 90 && LabelFlag == 1 ? true : false
                        ,
                        Crop = false,
                        Overflow = "None"

                    },
                    Marker = new PlotOptionsSeriesMarker
                    {
                        FillColor = i.NAMPORD >= 30 || i.NPORCDATOS >= 90 ? Color.Black : Color.Orange,
                        Radius = 2,
                        LineColor = Color.Orange,

                    },

                    Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                     "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                     "<b>Fecha Retorno</b> : " + i.RETURNDATE + "<br/>" +
                     "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                     "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                     "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                     "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                     "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                     "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                     "<b>AMPD ORD </b> : " + i.NAMPORD + "<br/>" +
                     "<b>Dato % </b> : " + i.NPORCDATOS + "<br/>",


                };

                n++;
            }

            n = 0;
            LabelFlag = 0;
            foreach (var i in glistaS)
            {
                if (i.NPORCDATOS >= 90)
                { LabelFlag++; }

                SlistaAMPDord[n] = new DotNet.Highcharts.Options.Point
                {

                    Y = Math.Round(Convert.ToDouble(i.NAMPORD), 2),
                    X = i.NPORCDATOS,
                    DataLabels = new PlotOptionsLineDataLabels
                    {
                        Enabled = i.NPORCDATOS >= 90 && LabelFlag == 1 ? true : false,
                        Crop = false,
                        Overflow = "None"
                    },

                    Marker = new PlotOptionsSeriesMarker
                    {
                        FillColor = i.NAMPORD >= 30 || i.NPORCDATOS >= 90 ? Color.Black : Color.Green,
                        Radius = 2
                    },

                    Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                     "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                     "<b>Fecha Retorno</b> : " + i.RETURNDATE + "<br/>" +
                     "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                     "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                     "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                     "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                     "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                     "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                     "<b>AMPD ORD </b> : " + i.NAMPORD + "<br/>" +
                     "<b>Dato % </b> : " + i.NPORCDATOS + "<br/>",

                };

                n++;
            }


            n = 0;
            LabelFlag = 0;
            foreach (var i in glistaS2)
            {
                if (i.NPORCDATOS >= 90)
                { LabelFlag++; }
                S2listaAMPDord[n] = new DotNet.Highcharts.Options.Point
                {

                    Y = Math.Round(Convert.ToDouble(i.NAMPORD), 2),
                    X = i.NPORCDATOS,
                    DataLabels = new PlotOptionsLineDataLabels
                    {
                        Enabled = i.NPORCDATOS >= 90 && LabelFlag == 1 ? true : false,
                        Crop = false,
                        Overflow = "None"
                    },

                    Marker = new PlotOptionsSeriesMarker
                    {
                        FillColor = i.NAMPORD >= 30 || i.NPORCDATOS >= 90 ? Color.Black : Color.Brown,
                        Radius = 2
                    },

                    Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                     "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                     "<b>Fecha Retorno</b> : " + i.RETURNDATE + "<br/>" +
                     "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                     "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                     "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                     "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                     "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                     "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                     "<b>AMPD ORD </b> : " + i.NAMPORD + "<br/>" +
                     "<b>Dato % </b> : " + i.NPORCDATOS + "<br/>",

                };

                n++;
            }



            n = 0;
            LabelFlag = 0;
            foreach (var i in glistaC)
            {
                if (i.NPORCDATOS >= 90)
                { LabelFlag++; }
                ClistaAMPDord[n] = new DotNet.Highcharts.Options.Point
                {

                    Y = Math.Round(Convert.ToDouble(i.NAMPORD), 2),
                    X = i.NPORCDATOS,
                    DataLabels = new PlotOptionsLineDataLabels
                    {
                        Enabled = i.NPORCDATOS >= 90 && LabelFlag == 1 ? true : false,
                        Crop = false,
                        Overflow = "None"
                    },
                    Marker = new PlotOptionsSeriesMarker
                    {
                        FillColor = i.NAMPORD >= 30 || i.NPORCDATOS >= 90 ? Color.Black : Color.Red,
                        Radius = 2
                    },

                    Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                     "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                     "<b>Fecha Retorno</b> : " + i.RETURNDATE + "<br/>" +
                     "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                     "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                     "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                     "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                     "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                     "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                     "<b>AMPD ORD </b> : " + i.NAMPORD + "<br/>" +
                     "<b>Dato % </b> : " + i.NPORCDATOS + "<br/>",

                };

                n++;
            }

            chart2 = new Highcharts("charts" + numerographics);


            chart2.InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Spline })
                .SetTitle(new Title { Text = "Gráfico frecuencia acumulada AMPD ORDENADA " })
                    .SetSubtitle(new Subtitle { Text = @"Duplicados Desde: 01/07/2019 Hasta: " + DateTime.Now.ToString("dd /MM/yyyy") + " CheckStage: " + string.Join(",", lista.Select(x => x.CHECKSTAGE).Distinct().ToList()) + " Elementos: " + string.Join(",", string.Join(",", lista.Select(x => x.ASSAYNAME).Distinct().ToList())) })
                    .SetXAxis(new XAxis
                    {
                        Title = new XAxisTitle { Text = "% Datos" },
                        Min = 0,
                        Max = 100,
                        TickInterval = 10,
                    })
                    .SetYAxis(new YAxis
                    {
                        Title = new YAxisTitle { Text = "AMPD (%)" },
                        Min = 0,
                        Max = 100,
                        TickInterval = 10,
                    })

                    .SetPlotOptions(new PlotOptions
                    {
                        Line = new PlotOptionsLine
                        {

                            DataLabels = new PlotOptionsLineDataLabels
                            {
                                Enabled = false,

                            },
                            EnableMouseTracking = true
                        }

                    })
                        .SetSeries(new[]
                        {
                            new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(PlistaAMPDord),
                                Name = "P ("+PlistaAMPDord.Length+")",
                                Color= Color.Orange,
                                 PlotOptionsLine = new PlotOptionsLine
                                {
                                    Color= Color.Orange,
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },

                                }

                            },
                            new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(SlistaAMPDord),
                                Name = "S ("+SlistaAMPDord.Length+")",
                                 PlotOptionsLine = new PlotOptionsLine
                                {
                                    Color= Color.Green,
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },
                                }
                            },
                               new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(S2listaAMPDord),
                                Name="S2 ("+S2listaAMPDord.Length+")",
                                 PlotOptionsLine = new PlotOptionsLine
                                {
                                    Color= Color.Brown,
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },

                                }
                            },
                            new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(ClistaAMPDord),
                                Name = "C ("+ClistaAMPDord.Length+")",
                                PlotOptionsLine = new PlotOptionsLine
                                {
                                  Color= Color.Red,
                                  TurboThreshold =100000,
                                  Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },

                                }
                            },
                            new DotNet.Highcharts.Options.Series
                            {
                                Type = ChartTypes.Line,
                                Name = "Linea de Regresión",
                                Data = new Data(new object[,] { { 0, 30 }, { 100, 30 } }),

                                PlotOptionsLine = new PlotOptionsLine
                                {
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker { Enabled = false },
                                    States = new PlotOptionsLineStates { Hover = new PlotOptionsLineStatesHover { LineWidth = 0 }  },
                                    EnableMouseTracking = false

                                },
                            },
                              new DotNet.Highcharts.Options.Series
                            {
                                Type = ChartTypes.Line,
                                Name = "Linea de Regresión",
                                Data = new Data(new object[,] { { 90, 0 }, { 90,100} }),
                                PlotOptionsLine = new PlotOptionsLine
                                {
                                    Marker = new PlotOptionsLineMarker { Enabled = false },
                                    States = new PlotOptionsLineStates { Hover = new PlotOptionsLineStatesHover { LineWidth = 0 } },
                                    EnableMouseTracking = false
                                },
                            },
                        });









            ViewBag.Grafico1 = Graf.ChartIRM(datosFiltrados, "NORM", "01/07/2019", DateTime.Now.ToString("dd/MM/yyyy"), "Grafico1");
            ViewBag.Grafico2 = Graf.ChartIRM(datosFiltrados1, "BLANK", "01/07/2019", DateTime.Now.ToString("dd/MM/yyyy"), "Grafico2");
            ViewBag.Grafico3 = chart2;


            // Slider contaminación
            ViewBag.BLKBG4CUT_CMCC = SlideBLKBG4CUT_CMCC();
            ViewBag.BLKBG4CUT_PCT = SlideBLKBG4CUT_PCT();
            ViewBag.BLKBG4CUS_CMCC = SlideBLKBG4CUS_CMCC();
            ViewBag.BLKBG4CUS_PCT = SlideBLKBG4CUS_PCT();

            ViewBag.BLKBF42CUT_CMCC = SlideBLKBF42CUT_CMCC();
            ViewBag.BLKBF42CUS_CMCC = SlideBLKBF42CUS_CMCC();
            ViewBag.BLKBF42CUT_PCT = SlideBLKBF42CUT_PCT();
            ViewBag.BLKBF42CUS_PCT = SlideBLKBBF42CUS_PCT();

            ViewBag.ST43CUT_CMCC = SlideST43CUT_CMCC();
            ViewBag.ST43CUS_CMCC = SlideST43CUS_CMCC();
            ViewBag.ST43CUT_PTXT = SlideST43CUT_PTXT();
            ViewBag.ST43CUS_PTXT = SlideST43CUS_PTXT();

            ViewBag.ST45CUT_CMCC = SlideST45CUT_CMCC();
            ViewBag.ST45CUS_CMCC = SlideST45CUS_CMCC();
            ViewBag.ST45CUT_PTXT = SlideST45CUT_PTXT();
            ViewBag.ST45CUS_PTXT = SlideST45CUS_PTXT();

            ViewBag.ST46CUT_CMCC = SlideST46CUT_CMCC();
            ViewBag.ST46CUS_CMCC = SlideST46CUS_CMCC();
            ViewBag.ST46CUT_PTXT = SlideST46CUT_PTXT();
            ViewBag.ST46CUS_PTXT = SlideST46CUS_PTXT();

            return View();

        }

        public DotNet.Highcharts.Highcharts SlideBLKBG4CUT_CMCC()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("BG4")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuT_CMCCAAS_pct")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "BLANK", FechaInicioQAQC, FechaFinQAQC, "GraficoBG1CutCmcc");
        }



        public DotNet.Highcharts.Highcharts SlideBLKBG4CUS_CMCC()
        {




            var datosFiltrados2 = dataStd.dataSTD().OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);

            datosFiltrados2 = datosFiltrados2.Where(x => x.STANDARDID.Equals("BG4")).Select(x => x);
            datosFiltrados2 = datosFiltrados2.Where(x => x.ASSAYNAME.Equals("CuS_CMCCAAS_pct")).Select(x => x);

            datosFiltrados2 = datosFiltrados2.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);

            //datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);

            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados2, "BLANK", FechaInicioQAQC, FechaFinQAQC, "GraficoBG1CusCmcc");
        }

        public DotNet.Highcharts.Highcharts SlideBLKBG4CUT_PCT()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("BG4")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuT_pct_2A15tAA")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "BLANK", FechaInicioQAQC, FechaFinQAQC, "GraficoBG1CutPct");
        }


        public DotNet.Highcharts.Highcharts SlideBLKBG4CUS_PCT()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("BG4")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuS_pct_SCA2pAA")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "BLANK", FechaInicioQAQC, FechaFinQAQC, "GraficoBG1CusPct");
        }


        public DotNet.Highcharts.Highcharts SlideBLKBF42CUT_CMCC()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("BF42")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuT_CMCCAAS_pct")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "BLANK", FechaInicioQAQC, FechaFinQAQC, "GraficoBF41CutCmcc");
        }

        public DotNet.Highcharts.Highcharts SlideBLKBF42CUS_CMCC()
        {




            var datosFiltrados2 = dataStd.dataSTD().OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);

            datosFiltrados2 = datosFiltrados2.Where(x => x.STANDARDID.Equals("BF42")).Select(x => x);
            datosFiltrados2 = datosFiltrados2.Where(x => x.ASSAYNAME.Equals("CuS_CMCCAAS_pct")).Select(x => x);
            //datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);

            datosFiltrados2 = datosFiltrados2.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);

            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);



            return Graf.ChartIRM(datosFiltrados2, "BLANK", FechaInicioQAQC, FechaFinQAQC, "GraficoBF41CusCmcc");
        }



        public DotNet.Highcharts.Highcharts SlideBLKBF42CUT_PCT()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("BF42")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuT_pct_2A15tAA")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "BLANK", FechaInicioQAQC, FechaFinQAQC, "GraficoBF41CutPct");
        }

        public DotNet.Highcharts.Highcharts SlideBLKBBF42CUS_PCT()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("BF42")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuS_pct_SCA2pAA")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "BLANK", FechaInicioQAQC, FechaFinQAQC, "GraficoBF41CusPct");
        }
        public DotNet.Highcharts.Highcharts SlideST43CUT_CMCC()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST43")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuT_CMCCAAS_pct")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST43CutCmcc");
        }
        public DotNet.Highcharts.Highcharts SlideST43CUS_CMCC()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST43")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuS_CMCCAAS_pct")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST43CuSCmcc");
        }
        public DotNet.Highcharts.Highcharts SlideST43CUT_PTXT()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST43")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuT_pct_2A15tAA")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST43CuTPTXT");
        }
        public DotNet.Highcharts.Highcharts SlideST43CUS_PTXT()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST43")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuS_pct_SCA2pAA")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST43CuSPTXT");
        }
        public DotNet.Highcharts.Highcharts SlideST45CUT_CMCC()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST45")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuT_CMCCAAS_pct")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST45CutCmcc");
        }
        public DotNet.Highcharts.Highcharts SlideST45CUS_CMCC()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST45")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuS_CMCCAAS_pct")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST45CuSCmcc");
        }
        public DotNet.Highcharts.Highcharts SlideST45CUT_PTXT()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST45")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuT_pct_2A15tAA")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST45CuTPTXT");
        }
        public DotNet.Highcharts.Highcharts SlideST45CUS_PTXT()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST45")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuS_pct_SCA2pAA")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST45CuSPTXT");
        }
        public DotNet.Highcharts.Highcharts SlideST46CUT_CMCC()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST46")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuT_CMCCAAS_pct")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST46CutCmcc");
        }
        public DotNet.Highcharts.Highcharts SlideST46CUS_CMCC()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST46")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuS_CMCCAAS_pct")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST46CuSCmcc");
        }
        public DotNet.Highcharts.Highcharts SlideST46CUT_PTXT()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST46")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuT_pct_2A15tAA")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST46CuTPTXT");
        }
        public DotNet.Highcharts.Highcharts SlideST46CUS_PTXT()
        {
            var datosTodos = dataStd.dataSTD();
            var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


            datosFiltrados = datosFiltrados.Where(x => x.STANDARDID.Equals("ST46")).Select(x => x);
            //  datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => x.ASSAYNAME.Equals("CuS_pct_SCA2pAA")).Select(x => x);
            datosFiltrados = datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaInicioQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaFinQAQC, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
            // datosFiltrados = datosFiltrados.Where(x => x.ANALYSISSUITE.Equals("CuT_CMCCAAS_pct")).Select(x => x);


            return Graf.ChartIRM(datosFiltrados, "", FechaInicioQAQC, FechaFinQAQC, "GraficoST46CuSPTXT");
        }


        public ActionResult Standard(string[] StandardidLista = null, string PrioridadLista = null, string[] ElementosLista = null, string FechaDesde = null, string FechaHasta = null, string[] SuiteLista = null, string[] Lab = null)
        {
            ViewBag.StandardidLista = d.DDLListSTD("S");
            ViewBag.SuiteLista = d.DDLDespatchSuites("std");
            ViewBag.PrioridadLista = d.DDLListStatus();
            ViewBag.TipoStd = "";

            if (string.IsNullOrEmpty(FechaDesde))
            {
                return View("~/Views/GeologiaQAQC/FormFiltro.cshtml");
            }
            else
            {
                var datosTodos = dataStd.dataSTD();



                var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x).Where(med =>
       ElementosLista.Any(name =>
          name.Equals(med.ASSAYNAME)
       ));

                //var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x).Where(x => x.ASSAYNAME.Equals(ElementosLista));
                //datosFiltrados = dataStd.IsNullOrEmpty(ElementosLista) ? datosFiltrados : datosFiltrados.Where(x => ElementosLista.Equals(x.ASSAYNAME)).Select(x => x);


                if (PrioridadLista == "1" || PrioridadLista == "2")
                { datosFiltrados = datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x); }
                else if (PrioridadLista == "3")
                { datosFiltrados = datosFiltrados.Where(x => x.ASSAY_PRIORITY >= Int32.Parse(PrioridadLista)).Select(x => x); }
                // datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);

                datosFiltrados = string.IsNullOrEmpty(FechaHasta) ? datosFiltrados : datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaDesde, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaHasta, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
                datosFiltrados = dataStd.IsNullOrEmpty(StandardidLista) ? datosFiltrados : datosFiltrados.Where(x => StandardidLista.Contains(x.STANDARDID)).Select(x => x);
                datosFiltrados = dataStd.IsNullOrEmpty(SuiteLista) ? datosFiltrados : datosFiltrados.Where(x => SuiteLista.Contains(x.ANALYSISSUITE)).Select(x => x);
                datosFiltrados = dataStd.IsNullOrEmpty(Lab) ? datosFiltrados : datosFiltrados.Where(x => Lab.Contains(x.LABCODE)).Select(x => x);


                var datosCount = datosFiltrados.Count();
                if (datosCount < 1) { return View(); }
                double WarnCount = datosFiltrados.Where(x => Math.Abs(Convert.ToDecimal(x.NORMALIZACION)) >= 2).Count();
                double ErrorCount = datosFiltrados.Where(x => Math.Abs(Convert.ToDecimal(x.NORMALIZACION)) >= 3).Count();
                double DevEst = dataStd.CalculateStdDev(datosFiltrados.Select(x => Convert.ToDouble(x.ASSAYVALUE)));
                double? Mean = datosFiltrados.Select(x => x.ASSAYVALUE).Average();
                double Median = dataStd.Median(datosFiltrados.Select(x => Convert.ToDouble(x.ASSAYVALUE)));
                double ErrorDev = DevEst / Math.Sqrt(datosCount);
                //Datos Tabla Detalle
                ViewBag.tablaDetalle = datosFiltrados;
                //Datos Tabla Resumen
                ViewBag.Total = datosCount;
                ViewBag.TotalWarn = WarnCount;
                ViewBag.TotalError = ErrorCount;
                ViewBag.PctError = Math.Round((ErrorCount * 100) / datosCount, 3);
                ViewBag.Mean = Math.Round(Convert.ToDouble(Mean), 3);
                ViewBag.Median = Math.Round(Median, 3);
                ViewBag.Min = datosFiltrados.Select(x => x.ASSAYVALUE).Min();
                ViewBag.Max = datosFiltrados.Select(x => x.ASSAYVALUE).Max();
                ViewBag.PctDevStd = Math.Round(Convert.ToDouble((DevEst / Mean) * 100), 3);
                ViewBag.DevStd = Math.Round(DevEst, 3);
                ViewBag.ErrorDev = Math.Round(ErrorDev, 3);
                ViewBag.PctErrorDevStd = Math.Round(Convert.ToDouble((ErrorDev / Mean) * 100), 3);
                ViewBag.Bias = Math.Round(Convert.ToDouble((Mean / datosFiltrados.First().STANDARDVALUE) - 1), 3);
                ViewBag.BiasMean = Math.Round(Convert.ToDouble((Mean / datosFiltrados.First().STANDARDVALUE) - 1), 3) * 100;
                ViewBag.GraficoNormalizado = Graf.ChartIRM(datosFiltrados, "NORM", FechaDesde, FechaHasta, "Grafico2");
                return View(Graf.ChartIRM(datosFiltrados, "", FechaDesde, FechaHasta, "Grafico1"));



            }


            //return View(GetStdFyChartByClass(StandardidLista, PrioridadLista, ElementosLista, FechaDesde, FechaHasta, SuiteLista));
        }
        public ActionResult StandardNorm(string[] StandardidLista = null, string PrioridadLista = null, string[] ElementosLista = null, string FechaDesde = null, string FechaHasta = null, string[] SuiteLista = null, string[] Lab = null)
        {


            ViewBag.PrioridadLista = d.DDLListStatus();
            ViewBag.Standard = StandardidLista?.ToString() ?? "";
            ViewBag.TipoStd = "Normalizados";
            if (string.IsNullOrEmpty(FechaDesde))
            {
                return View("~/Views/GeologiaQAQC/FormFiltro.cshtml");
            }
            else
            {
                var datosTodos = dataStd.dataSTD();
                var datosFiltrados = datosTodos.OrderBy(x => DateTime.Parse(x.RETURNDATE)).Select(x => x);


                datosFiltrados = dataStd.IsNullOrEmpty(StandardidLista) ? datosFiltrados : datosFiltrados.Where(x => StandardidLista.Contains(x.STANDARDID)).Select(x => x);
                if (PrioridadLista == "1" || PrioridadLista == "2")
                { datosFiltrados = datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x); }
                else if (PrioridadLista == "3")
                { datosFiltrados = datosFiltrados.Where(x => x.ASSAY_PRIORITY >= Int32.Parse(PrioridadLista)).Select(x => x); }
                // datosFiltrados = string.IsNullOrEmpty(PrioridadLista) ? datosFiltrados : datosFiltrados.Where(x => x.ASSAY_PRIORITY == Int32.Parse(PrioridadLista)).Select(x => x);
                datosFiltrados = dataStd.IsNullOrEmpty(ElementosLista) ? datosFiltrados : datosFiltrados.Where(x => ElementosLista.Contains(x.ASSAYNAME)).Select(x => x);
                datosFiltrados = string.IsNullOrEmpty(FechaHasta) ? datosFiltrados : datosFiltrados.Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaDesde, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) < DateTime.ParseExact(FechaHasta, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);
                datosFiltrados = dataStd.IsNullOrEmpty(SuiteLista) ? datosFiltrados : datosFiltrados.Where(x => SuiteLista.Contains(x.ANALYSISSUITE)).Select(x => x);
                datosFiltrados = dataStd.IsNullOrEmpty(Lab) ? datosFiltrados : datosFiltrados.Where(x => Lab.Contains(x.LABCODE)).Select(x => x);
                var datosCount = datosFiltrados.Count();
                if (datosCount < 1) { return View(); }
                double WarnCount = datosFiltrados.Where(x => Math.Abs(Convert.ToDecimal(x.NORMALIZACION)) >= 2).Count();
                double ErrorCount = datosFiltrados.Where(x => Math.Abs(Convert.ToDecimal(x.NORMALIZACION)) >= 3).Count();
                double DevEst = dataStd.CalculateStdDev(datosFiltrados.Select(x => Convert.ToDouble(x.NORMALIZACION)));
                double? Mean = datosFiltrados.Select(x => x.NORMALIZACION).Average();
                double Median = dataStd.Median(datosFiltrados.Select(x => Convert.ToDouble(x.NORMALIZACION)));
                double ErrorDev = DevEst / Math.Sqrt(datosCount);
                //Datos Tabla Detalle
                ViewBag.tablaDetalle = datosFiltrados;
                //Datos Tabla Resumen
                ViewBag.Total = datosCount;
                ViewBag.TotalWarn = WarnCount;
                ViewBag.TotalError = ErrorCount;
                ViewBag.PctError = Math.Round((ErrorCount * 100) / datosCount, 3);
                ViewBag.Mean = Mean;
                ViewBag.Median = Median;
                ViewBag.Min = datosFiltrados.Select(x => x.NORMALIZACION).Min();
                ViewBag.Max = datosFiltrados.Select(x => x.NORMALIZACION).Max();
                ViewBag.PctDevStd = (DevEst / Mean) * 100;
                ViewBag.DevStd = DevEst;
                ViewBag.ErrorDev = ErrorDev;
                ViewBag.PctErrorDevStd = (ErrorDev / Mean) * 100;
                return View(Graf.ChartIRM(datosFiltrados, "NORM", FechaDesde, FechaHasta, "Grafico1"));
            }


            //return View(GetStdFyChartByClass(StandardidLista, PrioridadLista, ElementosLista, FechaDesde, FechaHasta, SuiteLista));
        }

        public ActionResult Duplicado(string[] ProjectList = null, string[] DrillTypeList = null, string[] SuiteListaDup = null, string[] CheckStageList = null, string[] ElementosLista = null, string FechaDesde = @"", string FechaHasta = @"", string StatusLista = @"", string[] HoleStatusList = null, float MinAssayValue = 0, Int32 BarraAceptacion = 0, string fy = @"", string[] LabDup = null)
        {

            ViewBag.mensajeError = IsNullOrEmpty(ProjectList) && IsNullOrEmpty(DrillTypeList) && IsNullOrEmpty(SuiteListaDup) ? 1 : 0;
            var label = "0";
            var application = new Microsoft.Office.Interop.Excel.Application();
            var worksheetFunction = application.WorksheetFunction;
            //datos para listbox

            IEnumerable<CQUAQCDUP> lista;
            ViewBag.StatusLista = d.DDLListStatus();
            //Carga los datos en el filtro
           /* if (!fy.Equals(""))
            {
      
            }*/
            lista = (string.IsNullOrEmpty(FechaDesde)) ? data : dt.dataDuplicados().Where(x => DateTime.Parse(x.RETURNDATE) >= DateTime.ParseExact(FechaDesde, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.Parse(x.RETURNDATE) <= DateTime.ParseExact(FechaHasta, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Select(x => x);

            lista = (dt.IsNullOrEmpty(ProjectList)) ? lista : lista.Where(x => ProjectList.Contains(x.PROJECTCODE));
            lista = (IsNullOrEmpty(DrillTypeList)) ? lista : lista.Where(x => DrillTypeList.Contains(x.SAMPLE_DRILTYPE));
            lista = (IsNullOrEmpty(SuiteListaDup)) ? lista : lista.Where(x => SuiteListaDup.Contains(x.ANALYSISSUITE));
            lista = (IsNullOrEmpty(CheckStageList)) ? lista : lista.Where(x => CheckStageList.Contains(x.CHECKSTAGE));
            lista = (IsNullOrEmpty(ElementosLista)) ? lista : lista.Where(x => ElementosLista.Contains(x.ASSAYNAME));
            lista = (IsNullOrEmpty(ElementosLista)) ? lista : lista.Where(x => LabDup.Contains(x.LABCODE));
            if (StatusLista == "1" || StatusLista == "2")
            { lista = lista.Where(x => x.PRIORITY_OR == Int32.Parse(StatusLista)); }
            else if (StatusLista == "3")
            { lista = lista.Where(x => x.PRIORITY_OR >= Int32.Parse(StatusLista)); }

            lista = (IsNullOrEmpty(HoleStatusList)) ? lista : lista.Where(x => HoleStatusList.Contains(x.STATUS));
            lista = MinAssayValue == 0 ? lista : lista.Where(x => (x.ASSAYVALUE_CK + x.ASSAYVALUE_OR) / 2 > MinAssayValue);








            //Graficos 
            lista = dt.dataDuplicados(lista);
            if (lista != null)
            {
                decimal avgAssay = Math.Round((decimal)(lista.Select(x => x.ASSAYVALUE_OR).Max() + 0.5), 2);
                //foreach (var dup in lista)
                //{
                //    dataDupValues.Add(new ChartPointDup(dup.HOLEID, dup.DESPATCHNO, dup.ASSAYNAME, dup.CHECKSTAGE, dup.ID_OR, dup.ID_CK, dup.RETURNDATE, dup.SAMPFROM, dup.SAMPTO, dup.ASSAYVALUE_OR, dup.ASSAYVALUE_CK, dup.DIFERENCIA, dup.VAR_REL, dup.AMPD, dup.PROMEDIO, dup.MPD, dup.SAMPLE_DRILTYPE));
                //    listAmpd.Add(new ChartPointDup(dup.NAMPORD, dup.NPORCDATOS, dup.NAMPDPOND));

                //}

                ViewBag.tablaResumen = dt.dataDuplicadosTabla(lista);


                var totalRegistros = lista.Count();

                var listaDupP = lista.Where(x => x.CHECKSTAGE.Equals("P")).ToList();
                var listaDupS = lista.Where(x => x.CHECKSTAGE.Equals("S")).ToList();
                var listaDupS2 = lista.Where(x => x.CHECKSTAGE.Equals("S2")).ToList();
                var listaDupC = lista.Where(x => x.CHECKSTAGE.Equals("C")).ToList();





                //Cantidad de elementos a mostrar / Cada muestra tendra 8 posiciones
                var elementosCheckstage = (from t in lista
                                           group t by new { t.ASSAYNAME } into grp
                                           select new
                                           {
                                               grp.Key.ASSAYNAME
                                           }).ToList();
                //Grafico de Puntos
                object[] arrDupP;
                object[] arrDupS;
                object[] arrDupS2;
                object[] arrDupC;

                //Frecuencia acumulada Ordenada
                object[] PlistaAMPDord;
                object[] SlistaAMPDord;
                object[] S2listaAMPDord;
                object[] ClistaAMPDord;
                //Frecuencia acumulada Ponderada
                object[] PlistaAMPDPond;
                object[] SlistaAMPDPond;
                object[] S2listaAMPDPond;
                object[] ClistaAMPDPond;

                IEnumerable<CQUAQCDUP> glistaP = null;
                IEnumerable<CQUAQCDUP> glistaS = null;
                IEnumerable<CQUAQCDUP> glistaS2 = null;
                IEnumerable<CQUAQCDUP> glistaC = null;

                Highcharts[] arregloData = new Highcharts[elementosCheckstage.Count() * 3];
                List<CDetalleDUP>[] arregloTablasD = new List<CDetalleDUP>[elementosCheckstage.Count() * 4];
                List<CResumenDUP>[] arregloTablasR = new List<CResumenDUP>[elementosCheckstage.Count() * 4];
                Highcharts chart2;
                int numerographics = 0;
                int numerotablesR = 0;
                int numerotablesD = 0;
                foreach (var item in elementosCheckstage)
                {


                    glistaP = listaDupP.Where(x => x.ASSAYNAME.Equals(item.ASSAYNAME)).ToList();
                    glistaS = listaDupS.Where(x => x.ASSAYNAME.Equals(item.ASSAYNAME)).ToList();
                    glistaS2 = listaDupS2.Where(x => x.ASSAYNAME.Equals(item.ASSAYNAME)).ToList();
                    glistaC = listaDupC.Where(x => x.ASSAYNAME.Equals(item.ASSAYNAME)).ToList();

                    //Puntos Disperción
                    arrDupP = new object[glistaP.Count()];
                    arrDupS = new object[glistaS.Count()];
                    arrDupS2 = new object[glistaS2.Count()];
                    arrDupC = new object[glistaC.Count()];

                    //Grafico F. Acumulada AMPDORD
                    PlistaAMPDord = new object[glistaP.Count()];
                    SlistaAMPDord = new object[glistaS.Count()];
                    S2listaAMPDord = new object[glistaS2.Count()];
                    ClistaAMPDord = new object[glistaC.Count()];




                    glistaP = dt.dataDuplicados(glistaP);
                    glistaS = dt.dataDuplicados(glistaS);
                    glistaS2 = dt.dataDuplicados(glistaS2);
                    glistaC = dt.dataDuplicados(glistaC);

                    int n = 0;
                    //Puntos Disperción P
                    foreach (var punto in glistaP)
                    {

                        arrDupP[n] = new DotNet.Highcharts.Options.Point
                        {
                            Y = punto.ASSAYVALUE_CK,
                            X = punto.ASSAYVALUE_OR,
                            Marker = new PlotOptionsSeriesMarker
                            {
                                Radius = 4,
                                Enabled = true,
                                FillColor = Color.Orange,
                                Symbol = "circle"
                            },
                            Name = "<b><center>" + punto.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + punto.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + punto.RETURNDATE + "<br/>" +
                             "<b>Analito</b> : " + punto.ASSAYNAME + "<br/>" +
                             "<b>ID OR</b> : " + punto.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + punto.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + punto.CHECKSTAGE + "<br/>" +
                               "<b>Status: </b>" + (punto.PRIORITY_OR == 1 ? "Aprobado" : (punto.PRIORITY_OR == 2 ? "Pendiente" : ("Rechazado (" + punto.PRIORITY_OR + ")"))) + "<br>" +
                             "<b>Valor OR</b> : " + punto.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + punto.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + punto.SAMPLE_DRILTYPE + "<br/>",
                        };

                        n++;
                    }

                    n = 0;
                    //Puntos Disperción S
                    foreach (var punto in glistaS)
                    {
                        arrDupS[n] = new DotNet.Highcharts.Options.Point
                        {
                            Y = punto.ASSAYVALUE_CK,
                            X = punto.ASSAYVALUE_OR,
                            Marker = new PlotOptionsSeriesMarker
                            {
                                Radius = 4,
                                Enabled = true,
                                FillColor = Color.Green,
                                Symbol = "circle"
                            },
                            Name = "<b><center>" + punto.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + punto.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + punto.RETURNDATE + "<br/>" +
                             "<b>Analito</b> : " + punto.ASSAYNAME + "<br/>" +
                             "<b>ID OR</b> : " + punto.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + punto.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + punto.CHECKSTAGE + "<br/>" +
                             "<b>Status: </b>" + (punto.PRIORITY_OR == 1 ? "Aprobado" : (punto.PRIORITY_OR == 2 ? "Pendiente" : ("Rechazado (" + punto.PRIORITY_OR + ")"))) + "<br>" +
                             "<b>Valor OR</b> : " + punto.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + punto.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + punto.SAMPLE_DRILTYPE + "<br/>",
                        };

                        n++;
                    }

                    n = 0;
                    //Puntos Disperción S2
                    foreach (var punto in glistaS2)
                    {
                        arrDupS2[n] = new DotNet.Highcharts.Options.Point
                        {
                            Y = punto.ASSAYVALUE_CK,
                            X = punto.ASSAYVALUE_OR,
                            Marker = new PlotOptionsSeriesMarker
                            {
                                Radius = 4,
                                Enabled = true,
                                FillColor = Color.Brown,
                                Symbol = "circle"
                            },
                            Name = "<b><center>" + punto.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + punto.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + punto.RETURNDATE + "<br/>" +
                             "<b>Analito</b> : " + punto.ASSAYNAME + "<br/>" +
                             "<b>ID OR</b> : " + punto.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + punto.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + punto.CHECKSTAGE + "<br/>" +
                             "<b>Status: </b>" + (punto.PRIORITY_OR == 1 ? "Aprobado" : (punto.PRIORITY_OR == 2 ? "Pendiente" : ("Rechazado (" + punto.PRIORITY_OR + ")"))) + "<br>" +
                             "<b>Valor OR</b> : " + punto.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + punto.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + punto.SAMPLE_DRILTYPE + "<br/>",
                        };

                        n++;
                    }


                    n = 0;
                    //Puntos Disperción C
                    foreach (var punto in glistaC)
                    {
                        arrDupC[n] = new DotNet.Highcharts.Options.Point
                        {
                            Y = punto.ASSAYVALUE_CK,
                            X = punto.ASSAYVALUE_OR,
                            Marker = new PlotOptionsSeriesMarker
                            {
                                Radius = 4,
                                Enabled = true,
                                FillColor = Color.DarkBlue,
                                Symbol = "circle"
                            },
                            Name = "<b><center>" + punto.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + punto.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + punto.RETURNDATE + "<br/>" +
                             "<b>Analito</b> : " + punto.ASSAYNAME + "<br/>" +
                             "<b>ID OR</b> : " + punto.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + punto.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + punto.CHECKSTAGE + "<br/>" +
                             "<b>Status: </b>" + (punto.PRIORITY_OR == 1 ? "Aprobado" : (punto.PRIORITY_OR == 2 ? "Pendiente" : ("Rechazado (" + punto.PRIORITY_OR + ")"))) + "<br>" +
                             "<b>Valor OR</b> : " + punto.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + punto.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + punto.SAMPLE_DRILTYPE + "<br/>",
                        };

                        n++;
                    }
                    //creacion Grafico de Dispercion
                    chart2 = new Highcharts("charts" + numerographics);
                    System.Diagnostics.Debug.WriteLine("charts" + numerographics);

                    chart2.SetXAxis(new XAxis { Min = 0, Max = (double)avgAssay, TickInterval = (double)(avgAssay / 10), Title = new XAxisTitle { Text = "Muestra OR " } });
                    chart2.SetYAxis(new YAxis { Min = 0, Max = (double)avgAssay, TickInterval = (double)(avgAssay / 10), Title = new YAxisTitle { Text = "Muestra CK" } });
                    chart2.SetTitle(new Title { Text = "Gráfico de dispersión" });
                    chart2.SetSubtitle(new Subtitle { Text = @"Duplicados Desde: " + FechaDesde + " Hasta: " + FechaHasta + " CheckStage: " + string.Join(",", CheckStageList) + " Elementos: " + string.Join(",", ElementosLista) });
                    chart2.SetSeries(new[] {
                    new DotNet.Highcharts.Options.Series
                    {
                        Type = ChartTypes.Line,
                        Name = "Linea de Regresión",
                        Data = new Data(new object[,] { { 0, 0 }, { (double)avgAssay, (double)avgAssay } }),
                        PlotOptionsLine = new PlotOptionsLine
                        {
                            Marker = new PlotOptionsLineMarker { Enabled = false },
                            States = new PlotOptionsLineStates { Hover = new PlotOptionsLineStatesHover { LineWidth = 0 } },
                            EnableMouseTracking = false,
                        },
                    },
                    new DotNet.Highcharts.Options.Series
                    {
                        Type = ChartTypes.Scatter,
                        Name = "P ("+arrDupP.Length+")",
                        Color= Color.Orange,
                        Data = new Data(arrDupP),
                        PlotOptionsScatter = new PlotOptionsScatter { Marker = new PlotOptionsScatterMarker { Radius = 4 , Enabled = true, Symbol = "circle"}, TurboThreshold =100000, }
                    },
                     new DotNet.Highcharts.Options.Series
                    {
                        Type = ChartTypes.Scatter,
                        Name = "S ("+arrDupS.Length+")",
                        Color= Color.Green,
                        Data = new Data(arrDupS),
                        PlotOptionsScatter = new PlotOptionsScatter { Marker = new PlotOptionsScatterMarker { Radius = 4 , Enabled = true, Symbol = "circle"}, TurboThreshold =100000, }
                    },
                      new DotNet.Highcharts.Options.Series
                    {
                        Type = ChartTypes.Scatter,
                        Name = "S2 ("+arrDupS2.Length+")",
                        Data = new Data(arrDupS2),
                        Color= Color.Brown,
                        PlotOptionsScatter = new PlotOptionsScatter { Marker = new PlotOptionsScatterMarker { Radius = 4 , Enabled = true, Symbol = "circle"}, TurboThreshold =100000, }
                    },
                       new DotNet.Highcharts.Options.Series
                    {
                        Type = ChartTypes.Scatter,
                        Name = "C ("+arrDupC.Length+")",
                        Data = new Data(arrDupC),
                        Color= Color.DarkBlue,
                        PlotOptionsScatter = new PlotOptionsScatter { Marker = new PlotOptionsScatterMarker { Radius = 4 , Enabled = true, Symbol = "circle"}, TurboThreshold =100000, }
                    },


           });
                    chart2.SetTooltip(new Tooltip { Formatter = "function() { return this.point.name; }" });

                    //%######################### Fin Grafico Dispersion

                    arregloData[numerographics] = chart2;

                    numerographics += 1;

                    //%######################### inicio Grafico Frecuencia acumulada

                    n = 0;
                    var LabelFlag = 0;
                    var valuesAMPDORDglistaP = glistaP.OrderBy(x => x.NAMPORD).Select(x => (double)x.NAMPORD).ToList();
                    foreach (var i in glistaP)
                    {
                        if (i.NPORCDATOS >= 90)
                        {
                            LabelFlag++;
                            label = glistaP.Count() <= 1 ? "0" : dt.conversor3Decimales(worksheetFunction.Percentile(valuesAMPDORDglistaP.ToArray(), 0.90)).ToString();
                            System.Diagnostics.Debug.WriteLine(worksheetFunction.Percentile(valuesAMPDORDglistaP.ToArray(), 0.90).ToString());
                        }
                        PlistaAMPDord[n] = new DotNet.Highcharts.Options.Point
                        {

                            Y = i.NPORCDATOS >= 90 && LabelFlag == 1 ? Convert.ToDouble(label) : Math.Round(Convert.ToDouble(i.NAMPORD), 2),
                            X = i.NPORCDATOS,
                            DataLabels = new PlotOptionsLineDataLabels
                            {

                                Enabled = i.NPORCDATOS >= 90 && LabelFlag == 1 ? true : false
                                //,Formatter ="boom"
                                ,
                                Crop = false,
                                Overflow = "None"

                            },
                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor = i.NAMPORD >= 30 || i.NPORCDATOS >= 90 ? Color.Black : Color.Orange,
                                Radius = 2,
                                LineColor = Color.Orange,

                            },

                            Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + i.RETURNDATE + "<br/>" +
                             "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                             "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                             "<b>AMPD ORD </b> : " + i.NAMPORD + "<br/>" +
                             "<b>Dato % </b> : " + i.NPORCDATOS + "<br/>",


                        };

                        n++;
                    }

                    n = 0;
                    LabelFlag = 0;
                    var valuesAMPDORDglistaS = glistaS.OrderBy(x => x.NAMPORD).Select(x => (double)x.NAMPORD).ToList();
                    foreach (var i in glistaS)
                    {
                        if (i.NPORCDATOS >= 90)
                        {
                            LabelFlag++;
                            label = glistaS.Count() <= 1 ? "0" : dt.conversor3Decimales(worksheetFunction.Percentile(valuesAMPDORDglistaS.ToArray(), 0.90)).ToString();
                        }

                        SlistaAMPDord[n] = new DotNet.Highcharts.Options.Point
                        {

                            Y = i.NPORCDATOS >= 90 && LabelFlag == 1 ? Convert.ToDouble(label) : Math.Round(Convert.ToDouble(i.NAMPORD), 2),
                            X = i.NPORCDATOS,
                            DataLabels = new PlotOptionsLineDataLabels
                            {
                                Enabled = i.NPORCDATOS >= 90 && LabelFlag == 1 ? true : false,
                                Crop = false,
                                Overflow = "None"
                            },

                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor = i.NAMPORD >= 30 || i.NPORCDATOS >= 90 ? Color.Black : Color.Green,
                                Radius = 2
                            },

                            Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + i.RETURNDATE + "<br/>" +
                             "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                             "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                             "<b>AMPD ORD </b> : " + i.NAMPORD + "<br/>" +
                             "<b>Dato % </b> : " + i.NPORCDATOS + "<br/>",

                        };

                        n++;
                    }


                    n = 0;
                    LabelFlag = 0;
                    var valuesAMPDORDglistaS2 = glistaS2.OrderBy(x => x.NAMPORD).Select(x => (double)x.NAMPORD).ToList();
                    foreach (var i in glistaS2)
                    {
                        if (i.NPORCDATOS >= 90)
                        {
                            LabelFlag++;
                            label = glistaS2.Count() <= 1 ? "0" : dt.conversor3Decimales(worksheetFunction.Percentile(valuesAMPDORDglistaS2.ToArray(), 0.90)).ToString();
                        }
                        S2listaAMPDord[n] = new DotNet.Highcharts.Options.Point
                        {

                            Y = i.NPORCDATOS >= 90 && LabelFlag == 1 ? Convert.ToDouble(label) : Math.Round(Convert.ToDouble(i.NAMPORD), 2),
                            X = i.NPORCDATOS,
                            DataLabels = new PlotOptionsLineDataLabels
                            {
                                Enabled = i.NPORCDATOS >= 90 && LabelFlag == 1 ? true : false,
                                Crop = false,
                                Overflow = "None"
                            },

                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor = i.NAMPORD >= 30 || i.NPORCDATOS >= 90 ? Color.Black : Color.Brown,
                                Radius = 2
                            },

                            Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + i.RETURNDATE + "<br/>" +
                             "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                             "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                             "<b>AMPD ORD </b> : " + i.NAMPORD + "<br/>" +
                             "<b>Dato % </b> : " + i.NPORCDATOS + "<br/>",

                        };

                        n++;
                    }



                    n = 0;
                    LabelFlag = 0;
                    var valuesAMPDORDglistaC = glistaC.OrderBy(x => x.NAMPORD).Select(x => (double)x.NAMPORD).ToList();
                    foreach (var i in glistaC)
                    {
                        if (i.NPORCDATOS >= 90)
                        {
                            LabelFlag++;
                            label = glistaC.Count() <= 1 ? "0" : dt.conversor3Decimales(worksheetFunction.Percentile(valuesAMPDORDglistaC.ToArray(), 0.90)).ToString();
                        }
                        ClistaAMPDord[n] = new DotNet.Highcharts.Options.Point
                        {

                            Y = i.NPORCDATOS >= 90 && LabelFlag == 1 ? Convert.ToDouble(label) : Math.Round(Convert.ToDouble(i.NAMPORD), 2),
                            X = i.NPORCDATOS,
                            DataLabels = new PlotOptionsLineDataLabels
                            {
                                Enabled = i.NPORCDATOS >= 90 && LabelFlag == 1 ? true : false,
                                Crop = false,
                                Overflow = "None"
                            },
                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor = i.NAMPORD >= 30 || i.NPORCDATOS >= 90 ? Color.Black : Color.Black,
                                Radius = 2
                            },

                            Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + i.RETURNDATE + "<br/>" +
                             "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                             "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                             "<b>AMPD ORD </b> : " + i.NAMPORD + "<br/>" +
                             "<b>Dato % </b> : " + i.NPORCDATOS + "<br/>",

                        };

                        n++;
                    }

                    chart2 = new Highcharts("charts" + numerographics);


                    chart2.InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Line })
                        .SetTitle(new Title { Text = "Gráfico frecuencia acumulada AMPD ORDENADA " })
                            .SetSubtitle(new Subtitle { Text = @"Duplicados Desde: " + FechaDesde + " Hasta: " + FechaHasta + " CheckStage: " + string.Join(",", CheckStageList) + " Elementos: " + string.Join(",", ElementosLista) })
                            .SetXAxis(new XAxis
                            {
                                Title = new XAxisTitle { Text = "% Datos" },
                                Min = 0,
                                Max = 100,
                                TickInterval = 10,
                            })
                            .SetYAxis(new YAxis
                            {
                                Title = new YAxisTitle { Text = "AMPD (%)" },
                                Min = 0,
                                Max = 100,
                                TickInterval = 10,
                            })

                            .SetPlotOptions(new PlotOptions
                            {
                                Line = new PlotOptionsLine
                                {

                                    DataLabels = new PlotOptionsLineDataLabels
                                    {
                                        Enabled = false,

                                    },
                                    EnableMouseTracking = true
                                }

                            })
                                .SetSeries(new[]
                                {
                            new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(PlistaAMPDord),
                                Name = "P ("+PlistaAMPDord.Length+")",
                                Color= Color.Orange,
                                 PlotOptionsLine = new PlotOptionsLine
                                {
                                    Color= Color.Orange,
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },

                                }

                            },
                            new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(SlistaAMPDord),
                                Name = "S ("+SlistaAMPDord.Length+")",
                                 PlotOptionsLine = new PlotOptionsLine
                                {
                                    Color= Color.Green,
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },
                                }
                            },
                               new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(S2listaAMPDord),
                                Name="S2 ("+S2listaAMPDord.Length+")",
                                 PlotOptionsLine = new PlotOptionsLine
                                {
                                    Color= Color.Brown,
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },

                                }
                            },
                            new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(ClistaAMPDord),
                                Name = "C ("+ClistaAMPDord.Length+")",
                                PlotOptionsLine = new PlotOptionsLine
                                {
                                  Color= Color.DarkBlue,
                                  TurboThreshold =100000,
                                  Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },

                                }
                            },
                            new DotNet.Highcharts.Options.Series
                            {
                                Type = ChartTypes.Line,
                                Name = "Linea de Regresión",
                                Data = new Data(new object[,] { { 0, BarraAceptacion }, { 100, BarraAceptacion } }),

                                PlotOptionsLine = new PlotOptionsLine
                                {
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker { Enabled = false },
                                    States = new PlotOptionsLineStates { Hover = new PlotOptionsLineStatesHover { LineWidth = 0 }  },
                                    EnableMouseTracking = false

                                },
                            },
                              new DotNet.Highcharts.Options.Series
                            {
                                Type = ChartTypes.Line,
                                Name = "Linea de Regresión",
                                Data = new Data(new object[,] { { 90, 0 }, { 90,100} }),
                                PlotOptionsLine = new PlotOptionsLine
                                {
                                    Marker = new PlotOptionsLineMarker { Enabled = false },
                                    States = new PlotOptionsLineStates { Hover = new PlotOptionsLineStatesHover { LineWidth = 0 } },
                                    EnableMouseTracking = false
                                },
                            },
                                });


                    arregloData[numerographics] = chart2;

                    numerographics += 1;
                    /**############## FIN Grafico frecuencia Acumulada */
                    PlistaAMPDPond = new object[glistaP.Count()];
                    SlistaAMPDPond = new object[glistaS.Count()];
                    S2listaAMPDPond = new object[glistaS2.Count()];
                    ClistaAMPDPond = new object[glistaC.Count()];

                    var toperror = 20;
                    var bottomerror = -20;
                    //VArreglos con lineas limites
                    glistaP = glistaP.OrderBy(x => DateTime.Parse(x.RETURNDATE));
                    glistaS = glistaS.OrderBy(x => DateTime.Parse(x.RETURNDATE));
                    glistaS2 = glistaS2.OrderBy(x => DateTime.Parse(x.RETURNDATE));
                    glistaC = glistaC.OrderBy(x => DateTime.Parse(x.RETURNDATE));
                    object[,] ArrayTopError = new object[2, 2];
                    ArrayTopError[0, 0] = lista.Select(x => DateTime.Parse(x.RETURNDATE)).Min();
                    ArrayTopError[0, 1] = toperror;
                    ArrayTopError[1, 0] = lista.Select(x => DateTime.Parse(x.RETURNDATE)).Max();
                    ArrayTopError[1, 1] = toperror;
                    object[,] ArrayMinError = new object[2, 2];
                    ArrayMinError[0, 0] = lista.Select(x => DateTime.Parse(x.RETURNDATE)).Min();
                    ArrayMinError[0, 1] = bottomerror;
                    ArrayMinError[1, 0] = lista.Select(x => DateTime.Parse(x.RETURNDATE)).Max();
                    ArrayMinError[1, 1] = bottomerror;

                    n = 0;

                    foreach (var i in glistaP)
                    {

                        PlistaAMPDPond[n] = new Graficos.Points
                        {
                            Y = i.MPD,
                            X = DateTime.Parse(i.RETURNDATE),

                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor = i.MPD >= toperror || i.MPD <= bottomerror ? Color.Red : Color.Orange,
                                Radius = 2
                            },

                            Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + DateTime.Parse(i.RETURNDATE) + "<br/>" +
                             "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                             "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                             "<b>AMPD  </b> : " + i.MPD + "<br/>",

                        };

                        n++;
                    }

                    n = 0;

                    foreach (var i in glistaS)
                    {

                        SlistaAMPDPond[n] = new Graficos.Points
                        {
                            Y = i.MPD,
                            X = DateTime.Parse(i.RETURNDATE),

                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor = i.MPD >= toperror || i.MPD <= bottomerror ? Color.Red : Color.Green,
                                Radius = 2
                            },

                            Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + DateTime.Parse(i.RETURNDATE) + "<br/>" +
                             "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                             "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                             "<b>AMPD  </b> : " + i.MPD + "<br/>",
                        };

                        n++;
                    }


                    n = 0;

                    foreach (var i in glistaS2)
                    {

                        S2listaAMPDPond[n] = new Graficos.Points
                        {
                            Y = i.MPD,
                            X = DateTime.Parse(i.RETURNDATE),

                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor = i.MPD >= toperror || i.MPD <= bottomerror ? Color.Red : Color.Brown,
                                Radius = 2
                            },

                            Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + DateTime.Parse(i.RETURNDATE) + "<br/>" +
                             "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                             "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                             "<b>AMPD  </b> : " + i.MPD + "<br/>",
                        };

                        n++;
                    }



                    n = 0;

                    foreach (var i in glistaC)
                    {

                        ClistaAMPDPond[n] = new Graficos.Points
                        {
                            Y = i.MPD,
                            X = DateTime.Parse(i.RETURNDATE),

                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor = i.MPD >= toperror || i.MPD <= bottomerror ? Color.Red : Color.Black,
                                Radius = 2
                            },

                            Name = "<b><center>" + i.HOLEID + "</center></b><br>" +
                             "<b>DESPATCHNO</b> : " + i.DESPATCHNO + "<br/>" +
                             "<b>Fecha Retorno</b> : " + DateTime.Parse(i.RETURNDATE) + "<br/>" +
                             "<b>ID OR</b> : " + i.ID_OR + "<br/>" +
                             "<b>ID CH</b> : " + i.ID_CK + "<br/>" +
                             "<b>CheckStage</b> : " + i.CHECKSTAGE + "<br/>" +
                             "<b>Valor OR</b> : " + i.ASSAYVALUE_OR + "<br/>" +
                             "<b>Valor CK</b> : " + i.ASSAYVALUE_CK + "<br/>" +
                             "<b>Tipo Perf</b> : " + i.SAMPLE_DRILTYPE + "<br/>" +
                             "<b>AMPD  </b> : " + i.MPD + "<br/>",
                        };

                        n++;
                    }

                    chart2 = new Highcharts("charts" + numerographics);


                    lista = lista.OrderBy(x => DateTime.Parse(x.RETURNDATE));
                    chart2.InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Line })
                        .SetTitle(new Title { Text = "Gráfico Secuencia de Error" })// + String.Join(",",ElementosList)+" " + String.Join(",", CheckStageList) })
                            .SetSubtitle(new Subtitle { Text = @"Duplicados Desde: " + FechaDesde + " Hasta: " + FechaHasta + " CheckStage: " + string.Join(",", CheckStageList) + " Elementos: " + string.Join(",", ElementosLista) })
                            .SetXAxis(new XAxis
                            {
                                Type = AxisTypes.Datetime,
                                TickInterval = 3600 * 1000,
                                DateTimeLabelFormats = new DateTimeLabel { Month = "%e. %b", Year = "%b" },
                                Title = new XAxisTitle { Text = @"Fechas de Retorno" },
                            })
                            .SetYAxis(new YAxis
                            {
                                Title = new YAxisTitle { Text = "AMPD (%)" },
                                Min = -50,
                                Max = 50,
                                TickInterval = 10,
                            })
                            .SetTooltip(new Tooltip
                            {
                                Enabled = true,

                            })
                            .SetPlotOptions(new PlotOptions
                            {
                                Line = new PlotOptionsLine
                                {

                                    DataLabels = new PlotOptionsLineDataLabels
                                    {
                                        Enabled = false,

                                    },
                                    EnableMouseTracking = true
                                }

                            }).InitChart(new DotNet.Highcharts.Options.Chart
                            {
                                ZoomType = ZoomTypes.Xy,
                                DefaultSeriesType = ChartTypes.Line
                            })
                                .SetSeries(new[]
                                {
                            new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(PlistaAMPDPond),
                                Name = "P ("+PlistaAMPDPond.Length+")",

                                Color= Color.Orange,
                                 PlotOptionsLine = new PlotOptionsLine
                                {
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },
                                }

                            },
                            new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(SlistaAMPDPond),
                                Name = "S ("+SlistaAMPDPond.Length+")",
                                Color= Color.Green,
                                 PlotOptionsLine = new PlotOptionsLine
                                {
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },
                                }

                            },
                               new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(S2listaAMPDPond),
                                Name = "S2 ("+S2listaAMPDPond.Length+")",
                                Color= Color.Brown,
                                 PlotOptionsLine = new PlotOptionsLine
                                {
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },
                                }
                            },
                            new DotNet.Highcharts.Options.Series
                            {
                                Data = new Data(ClistaAMPDPond),
                                Name = "C ("+ClistaAMPDPond.Length+")",
                                Color = Color.LightBlue,
                                PlotOptionsLine = new PlotOptionsLine
                                {
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },
                                }
                            },
                            new DotNet.Highcharts.Options.Series
                            {
                                Type = ChartTypes.Line,
                                Name = "Error",
                                Data = new Data(ArrayTopError),
                                Color= Color.Red,
                                PlotOptionsLine = new PlotOptionsLine
                                {
                                    TurboThreshold =100000,
                                    Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },
                                    States = new PlotOptionsLineStates { Hover = new PlotOptionsLineStatesHover { LineWidth = 0 }  },
                                    EnableMouseTracking = false
                                },
                            },
                           new DotNet.Highcharts.Options.Series
                            {
                                Type = ChartTypes.Line,
                                Name = "Error",
                                Data = new Data(ArrayMinError),
                                Color= Color.Red,
                                PlotOptionsLine = new PlotOptionsLine
                                {
                                    TurboThreshold =100000,
                                  Marker = new PlotOptionsLineMarker {Enabled = true, Symbol = "circle" },
                                    States = new PlotOptionsLineStates { Hover = new PlotOptionsLineStatesHover { LineWidth = 0 }  },
                                    EnableMouseTracking = false
                                },
                            },

                                });
                    //chart2.SetTooltip(new Tooltip { Formatter = "function() { return this.point.name; }" });

                    arregloData[numerographics] = chart2;
                    numerographics += 1;

                    //tabla resumen


                    if (listaDupP.Count() > 0)
                    {
                        arregloTablasR[numerotablesR] = dt.dataDuplicadosTabla(listaDupP);
                        numerotablesR += 1;
                    }
                    if (listaDupS.Count() > 0)
                    {
                        arregloTablasR[numerotablesR] = dt.dataDuplicadosTabla(listaDupS);
                        numerotablesR += 1;
                    }
                    if (listaDupS2.Count() > 0)
                    {
                        arregloTablasR[numerotablesR] = dt.dataDuplicadosTabla(listaDupS2);
                        numerotablesR += 1;
                    }
                    if (listaDupC.Count() > 0)
                    {
                        arregloTablasR[numerotablesR] = dt.dataDuplicadosTabla(listaDupC);
                        numerotablesR += 1;
                    }


                    //Tabla detalle
                    if (listaDupP.Count() > 0)
                    {
                        arregloTablasD[numerotablesD] = dt.dataDetalleTabla(listaDupP);
                        numerotablesD += 1;
                    }
                    if (listaDupS.Count() > 0)
                    {
                        arregloTablasD[numerotablesD] = dt.dataDetalleTabla(listaDupS);
                        numerotablesD += 1;
                    }
                    if (listaDupS2.Count() > 0)
                    {
                        arregloTablasD[numerotablesD] = dt.dataDetalleTabla(listaDupS2);
                        numerotablesD += 1;
                    }
                    if (listaDupC.Count() > 0)
                    {
                        arregloTablasD[numerotablesD] = dt.dataDetalleTabla(listaDupC);
                        numerotablesD += 1;
                    }



                }
                //System.Diagnostics.Debug.WriteLine(CheckStageList.Count());
                ViewBag.DataError = lista.Count();
                ViewBag.G2 = arregloData;
                ViewBag.ResumenG = arregloTablasR;
                ViewBag.TablaDG = arregloTablasD;
                ViewBag.cantidad = CheckStageList.Count();
                ViewBag.proyecto = ProjectList;

            }
            if (lista != null) { return View(); }

            else { return View("~/Views/GeologiaQAQC/FormFiltroDup.cshtml"); }

        }


        private bool IsNullOrEmpty(string[] projectList)
        {
            return (projectList == null || projectList.Length == 0);
        }



        //Data Script
        public JsonResult GetDataAssay(string id)
        {
            List<String> elementList = new List<string>();
            using (DM_CC_SONEntities ddb = new DM_CC_SONEntities())
            {


                foreach (var item in ddb.DTM_QAQC_DUP.Select(x => x.ASSAYNAME).Distinct().ToList())

                {
                    elementList.Add(item.ToString());
                }
            }
            return Json(elementList, JsonRequestBehavior.AllowGet);
            // System.Diagnostics.Debug.WriteLine(id);
            // return Json(new SelectList(d.DDLNameAssay(nameAssay), "Value", "Text"));
        }

       





    }
}
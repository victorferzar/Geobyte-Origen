using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using geologycmcc.Models.GeologiaQAQCModels;
using System.Drawing;
using DotNet.Highcharts.Enums;

namespace geologycmcc.Controllers.Graficos
{
    public class GraficosClass
    {
        public Highcharts ChartIRM(IEnumerable<CQAQCSTD> datosFiltrados, string TipoDat, string desde, string hasta, string nombregrafico)
        {

            var datosCount = datosFiltrados.Count();
            var datosCountRech = datosFiltrados.Where(x => x.ASSAY_PRIORITY >= 3 && x.ASSAYVALUE > 0.01).Count();
            //Pendientes
            var datosCountPend = datosFiltrados.Where(x => x.ASSAY_PRIORITY == 2).Count();

            if (datosCount == 0)
            {
                return null;
            }


            // codigo para grafico

            System.Diagnostics.Debug.WriteLine(TipoDat);
            var firstX = datosFiltrados.Select(x => DateTime.Parse(x.RETURNDATE)).Min();
            var lastX = datosFiltrados.Select(x => DateTime.Parse(x.RETURNDATE)).Max();
            var normalizacion = new object[datosCount];
            var normalizacionNoRech = new object[datosCount - datosCountRech- datosCountPend];
            //Pendientes
            var normalizacionPend = new object[datosCountPend];
            var normalizacionRech = new object[datosCountRech];
            object[,] MediaMov = new object[datosCount, datosCount];
            var Muestras = new SortedList<DateTime, double?>();
            var errorLineTop = new object[datosCount];
            var errorLineBot = new object[datosCount];
            var warningLineTop = new object[datosCount];
            var warningLineBot = new object[datosCount];
            var zeroLine = new object[datosCount];
            System.Diagnostics.Debug.WriteLine(firstX.ToString(), lastX.ToString());
            double? maxError = TipoDat == "NORM" ? 3 : TipoDat == "BLANK" ? 0.01 : datosFiltrados.First().MAX;
            double? minError = TipoDat == "NORM" ? -3 : TipoDat == "BLANK" ? 0 : datosFiltrados.First().MIN;
            double? maxWarn = TipoDat == "NORM" ? 2 : TipoDat == "BLANK" ? 0.006 : datosFiltrados.First().MAX - datosFiltrados.First().DEV;
            double? minWarn = TipoDat == "NORM" ? -2 : TipoDat == "BLANK" ? 0 : datosFiltrados.First().MIN + datosFiltrados.First().DEV;
            double? midVal = TipoDat == "NORM" ? 0 : datosFiltrados.First().STANDARDVALUE;
            double? maxChart = TipoDat == "NORM" ? 5 : TipoDat == "BLANK" ? 0.02 : datosFiltrados.First().MAX + datosFiltrados.First().DEV;
            double? minChart = TipoDat == "NORM" ? -5 : TipoDat == "BLANK" ? 0 : datosFiltrados.First().MIN - datosFiltrados.First().DEV;




            //double limite_grafico = 0;


            int tempCount = 0;
            int tempCountRech = 0;
            int tempCountPend = 0;
            foreach (var item in datosFiltrados)
            {
                double? YDot = TipoDat == "NORM" ? Convert.ToDouble(item.NORMALIZACION) : Convert.ToDouble(item.ASSAYVALUE);
                if (TipoDat == "BLANK")
                {
                    if (item.ASSAY_PRIORITY >= 3 && item.ASSAYVALUE > 0.01)
                    {
                        normalizacionRech[tempCountRech] = new Points
                        {

                            Y = YDot,
                            X = DateTime.Parse(item.RETURNDATE),

                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor =  System.Drawing.Color.Red ,
                            },
                            Name = @"<b>Checkid: </b>" + item.CHECKID + @"<br>"
                                 + @"<b>Standardid: </b>" + item.STANDARDID + @"<br>"
                                 + @"<b>Status: </b>" + (item.ASSAY_PRIORITY == 1 ? @"Aprobado" : (item.ASSAY_PRIORITY == 2 ? @"Pendiente" : (@"Rechazado (" + item.ASSAY_PRIORITY + @")"))) + @"<br>"
                                 + @"<b>Elemento: </b>" + item.ASSAYNAME + @"<br>"
                                 + @"<b>Assay Value: </b>" + item.ASSAYVALUE + @"<br>"
                                 + @"<b>Std Value: </b>" + item.STANDARDVALUE + @"<br>"
                                 + @"<b>Std Dev: </b>" + item.DEV + @"<br>"
                                 + @"<b>Batch: </b>" + item.DESPATCHNO + @"<br>"
                                 + @"<b>LabjobNo: </b>" + item.LABJOBNO + @"<br>"
                                 + @"<b>Fecha: </b>" + item.RETURNDATE + @"<br>"
                                 + @"<b>Normalizacion: </b>" + item.NORMALIZACION
                        };

               
                            tempCountRech++;
                   

                    } else if (item.ASSAY_PRIORITY == 2)
                    {
                        normalizacionPend[tempCountPend] = new Points
                        {

                            Y = YDot,
                            X = DateTime.Parse(item.RETURNDATE),

                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor = System.Drawing.Color.Yellow,
                            },
                            Name = @"<b>Checkid: </b>" + item.CHECKID + @"<br>"
                                 + @"<b>Standardid: </b>" + item.STANDARDID + @"<br>"
                                 + @"<b>Status: </b>" + (item.ASSAY_PRIORITY == 1 ? @"Aprobado" : (item.ASSAY_PRIORITY == 2 ? @"Pendiente" : (@"Rechazado (" + item.ASSAY_PRIORITY + @")"))) + @"<br>"
                                 + @"<b>Elemento: </b>" + item.ASSAYNAME + @"<br>"
                                 + @"<b>Assay Value: </b>" + item.ASSAYVALUE + @"<br>"
                                 + @"<b>Std Value: </b>" + item.STANDARDVALUE + @"<br>"
                                 + @"<b>Std Dev: </b>" + item.DEV + @"<br>"
                                 + @"<b>Batch: </b>" + item.DESPATCHNO + @"<br>"
                                 + @"<b>LabjobNo: </b>" + item.LABJOBNO + @"<br>"
                                 + @"<b>Fecha: </b>" + item.RETURNDATE + @"<br>"
                                 + @"<b>Normalizacion: </b>" + item.NORMALIZACION
                        };


                        tempCountPend++;


                    }
                    else
                    {
                        normalizacionNoRech[tempCount] = new Points
                        {

                            Y = YDot,
                            X = DateTime.Parse(item.RETURNDATE),


                            Marker = new PlotOptionsSeriesMarker
                            {
                                FillColor = item.ASSAY_PRIORITY >= 3 ? System.Drawing.Color.Red : item.ASSAY_PRIORITY == 2 ? System.Drawing.Color.Yellow : Color.Green,
                            },
                            Name = @"<b>Checkid: </b>" + item.CHECKID + @"<br>"
             + @"<b>Standardid: </b>" + item.STANDARDID + @"<br>"
             + @"<b>Status: </b>" + (item.ASSAY_PRIORITY == 1 ? @"Aprobado" : (item.ASSAY_PRIORITY == 2 ? @"Pendiente" : (@"Rechazado (" + item.ASSAY_PRIORITY + @")"))) + @"<br>"
             + @"<b>Elemento: </b>" + item.ASSAYNAME + @"<br>"
             + @"<b>Assay Value: </b>" + item.ASSAYVALUE + @"<br>"
             + @"<b>Std Value: </b>" + item.STANDARDVALUE + @"<br>"
             + @"<b>Std Dev: </b>" + item.DEV + @"<br>"
             + @"<b>Batch: </b>" + item.DESPATCHNO + @"<br>"
             + @"<b>LabjobNo: </b>" + item.LABJOBNO + @"<br>"
             + @"<b>Fecha: </b>" + item.RETURNDATE + @"<br>"
             + @"<b>Normalizacion: </b>" + item.NORMALIZACION
                        };

                        tempCount++;
                    }

                }
                else {
                    normalizacion[tempCount] = new Points
                    {

                        Y = YDot,
                        X = DateTime.Parse(item.RETURNDATE),


                        Marker = new PlotOptionsSeriesMarker
                        {
                            FillColor = item.ASSAY_PRIORITY == 2 ? System.Drawing.Color.Yellow : Color.Green,
                            // FillColor = item.ASSAY_PRIORITY >= 3 ? System.Drawing.Color.Red : item.ASSAY_PRIORITY == 2 ? System.Drawing.Color.Yellow : Color.Green,
                        },
                        Name = @"<b>Checkid: </b>" + item.CHECKID + @"<br>"
                        + @"<b>Standardid: </b>" + item.STANDARDID + @"<br>"
                        + @"<b>Status: </b>" + (item.ASSAY_PRIORITY == 1 ? @"Aprobado" : (item.ASSAY_PRIORITY == 2 ? @"Pendiente" : (@"Rechazado (" + item.ASSAY_PRIORITY + @")"))) + @"<br>"
                        + @"<b>Elemento: </b>" + item.ASSAYNAME + @"<br>"
                        + @"<b>Assay Value: </b>" + item.ASSAYVALUE + @"<br>"
                        + @"<b>Std Value: </b>" + item.STANDARDVALUE + @"<br>"
                        + @"<b>Std Dev: </b>" + item.DEV + @"<br>"
                        + @"<b>Batch: </b>" + item.DESPATCHNO + @"<br>"
                        + @"<b>LabjobNo: </b>" + item.LABJOBNO + @"<br>"
                        + @"<b>Fecha: </b>" + item.RETURNDATE + @"<br>"
                        + @"<b>Normalizacion: </b>" + item.NORMALIZACION
                    };

                    tempCount++;

                    //limite_grafico = limite_grafico < Math.Ceiling(Convert.ToDouble(norma)) ? Convert.ToDouble(norma) : limite_grafico;

                }

            }
            //var period = 20;
            //object[,] result = new object[tempCount, tempCount];
            //for (int i = 0; i < tempCount; i++)
            //{
            //    if (i >= period - 1)
            //    {
            //        double? total = 0;
            //        for (int x = i; x > (i - period); x--)
            //            total += MediaMov.;
            //        double? average = total / period;
            //        result.Add(Muestras.Keys[i], average);
            //    }

            //}
            //Lineas fijas Error  y Warning


            object[,] ArrayTopError = new object[2, 2];
            ArrayTopError[0, 0] = firstX;
            ArrayTopError[0, 1] = maxError;
            ArrayTopError[1, 0] = lastX;
            ArrayTopError[1, 1] = maxError;
            object[,] ArrayMinError = new object[2, 2];
            ArrayMinError[0, 0] = firstX;
            ArrayMinError[0, 1] = minError;
            ArrayMinError[1, 0] = lastX;
            ArrayMinError[1, 1] = minError;
            object[,] ArrayTopWarn = new object[2, 2];
            ArrayTopWarn[0, 0] = firstX;
            ArrayTopWarn[0, 1] = maxWarn;
            ArrayTopWarn[1, 0] = lastX;
            ArrayTopWarn[1, 1] = maxWarn;
            object[,] ArrayMinWarn = new object[2, 2];
            ArrayMinWarn[0, 0] = firstX;
            ArrayMinWarn[0, 1] = minWarn;
            ArrayMinWarn[1, 0] = lastX;
            ArrayMinWarn[1, 1] = minWarn;
            object[,] ArrayMidle = new object[2, 2];
            ArrayMidle[0, 0] = firstX;
            ArrayMidle[0, 1] = midVal;
            ArrayMidle[1, 0] = lastX;
            ArrayMidle[1, 1] = midVal;

            Highcharts chart = new Highcharts(nombregrafico);


            chart.SetXAxis(new XAxis
            {

                Type = AxisTypes.Datetime,
                DateTimeLabelFormats = new DateTimeLabel { Month = "%e. %b", Year = "%b" },
                //TickInterval= 3600 * 1000 * 24,

                Title = new XAxisTitle { Text = @"Fechas de Retorno" },



            });

            chart.SetTooltip(new Tooltip
            {
                FollowPointer = true,
                Shared = false,
                UseHTML = true,




            });
            chart.SetYAxis(new YAxis
            {
                Title = new YAxisTitle { Text = TipoDat == "NORM" ? @"Dato normalizado" : @"Ley Laboratorio" },
                Max = TipoDat == "BLANK" ? 0.02 : maxError + (maxError * 0.1),
                Min = TipoDat == "BLANK" ? 0 : minError - (maxError * 0.1),
                TickInterval = TipoDat == "BLANK" ? 0.002 : TipoDat == "NORM" ? 1 : 0.01
            });
            chart.SetPlotOptions(new PlotOptions
            {
                Series = new PlotOptionsSeries { PointInterval = 24 * 3600 * 1000 },
                Line = new PlotOptionsLine
                {

                }
            });

            chart.SetSeries(

                new[] {

                            new Series
                            {
                                Type= TipoDat == "BLANK" ? ChartTypes.Column : ChartTypes.Spline,
                                Data = TipoDat == "BLANK" ?new Data(normalizacionNoRech)  :new Data(normalizacion),

                                Color = TipoDat == "BLANK" ? Color.Orange : TipoDat == "BLANK" ? Color.Orange:Color.Gray,
                                Name="Muestra",
                                PlotOptionsLine = new PlotOptionsLine{  Marker = new PlotOptionsLineMarker{Enabled = true, Radius = 3, LineWidth = 1} },
                                PlotOptionsColumn = TipoDat == "BLANK" ? new  PlotOptionsColumn{ PointWidth =10  }: null
                            },
                           new Series
                            {
                                Type= TipoDat == "BLANK" ? ChartTypes.Column : ChartTypes.Spline,
                                Data =TipoDat == "BLANK" ? new Data(normalizacionRech) : null,
                                Color = Color.Red ,
                                Name="Rechazados",
                                PlotOptionsLine = new PlotOptionsLine{  Marker = new PlotOptionsLineMarker{Enabled = true, Radius = 3, LineWidth = 1, Symbol ="circle"} },
                                PlotOptionsColumn = TipoDat == "BLANK" ? new  PlotOptionsColumn{ PointWidth =10  }: null
                            },
                             new Series
                            {
                                Type= TipoDat == "BLANK" ? ChartTypes.Column : ChartTypes.Spline,
                                Data =TipoDat == "BLANK" ? new Data(normalizacionPend) : null,
                                Color = Color.Yellow ,
                                Name="Pendientes",
                                PlotOptionsLine = new PlotOptionsLine{  Marker = new PlotOptionsLineMarker{Enabled = true, Radius = 3, LineWidth = 1, Symbol ="circle"} },
                                PlotOptionsColumn = TipoDat == "BLANK" ? new  PlotOptionsColumn{ PointWidth =10  }: null
                            },
                            new Series
                            {
                                Data = new Data(ArrayTopError),
                                Color = Color.Red,
                                Name="Error",
                                PlotOptionsLine = new PlotOptionsLine{ Marker = new PlotOptionsLineMarker{Enabled = false}, EnableMouseTracking = false, LineWidth = 1}
                            },
                            new Series
                            {
                                Data = new Data(ArrayMinError),
                                Color = Color.Red,
                                Name="Error",
                                PlotOptionsLine = new PlotOptionsLine{ Marker = new PlotOptionsLineMarker{Enabled = false}, EnableMouseTracking = false, LineWidth = 1,Visible=TipoDat == "BLANK" ? false: true}
                            },

                            new Series
                            {
                                Data = new Data(ArrayTopWarn),
                                Color = Color.Orange,
                                Name="Warning",
                                PlotOptionsLine = new PlotOptionsLine{ Marker = new PlotOptionsLineMarker{Enabled = false}, EnableMouseTracking = false, LineWidth = 1}
                            },
                            new Series
                            {
                                Data = new Data(ArrayMinWarn),
                                Color = Color.Orange,
                                Name="Warning",
                                PlotOptionsLine = new PlotOptionsLine{ Marker = new PlotOptionsLineMarker{Enabled = false}, EnableMouseTracking = false, LineWidth = 1,Visible=TipoDat == "BLANK" ? false: true}
                            },

                            // new Series
                            //{
                            //    Data = new Data(result),
                            //    Color = Color.Orange,
                            //    Name="Mov Avg 20 per",
                            //    PlotOptionsLine = new PlotOptionsLine{ Marker = new PlotOptionsLineMarker{Enabled = false}, EnableMouseTracking = false, LineWidth = 1,Visible=TipoDat == "BLANK" ? false: true}
                            //},
                            new Series
                             {
                                Type = ChartTypes.Line,
                                 Name = "Zero",
                                Color = Color.Green,
                                Data = new Data(ArrayMidle),
                                 PlotOptionsLine = new PlotOptionsLine
                                     {
                            Marker = new PlotOptionsLineMarker { Enabled = false },
                            States = new PlotOptionsLineStates { Hover = new PlotOptionsLineStatesHover { LineWidth = 1 } },
                            EnableMouseTracking = false,
                            Visible=TipoDat == "BLANK" ? false: true
                                 },
                              }
                        }
            );

            chart.InitChart(new Chart
            {
                ZoomType = ZoomTypes.Xy
                ,
                DefaultSeriesType = TipoDat == "BLANK" ? ChartTypes.Spline : ChartTypes.Spline

            });

            chart.SetTitle(new Title
            {
                Text = @"Estándares Desde: " + desde + " Hasta: " + hasta + " STD: " + string.Join(",", datosFiltrados.Select(x => x.STANDARDID).Distinct().ToList()) + " Elementos: " + string.Join(",", datosFiltrados.Select(x => x.ASSAYNAME).Distinct().ToList()),

            })
             .SetSubtitle(new Subtitle { Text = "Suite de Análisis:   " + String.Join(",", string.Join(",", datosFiltrados.Select(x => x.ANALYSISSUITE).Distinct().ToList())) });
            chart.SetPlotOptions(new PlotOptions
            {
                Series = new PlotOptionsSeries
                {
                    Cursor = Cursors.Pointer,
                    TurboThreshold = 10000
                }
            });

            //chart.SetTooltip(new Tooltip { Formatter = "function() { return this.point.name; }" });

            chart.SetExporting(new Exporting
            {
                Enabled = true,

                Filename = "STD_FY"
            });

            return chart;
        }
    }
}
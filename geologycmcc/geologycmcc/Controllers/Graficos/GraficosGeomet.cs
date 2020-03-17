using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System.Drawing;
using geologycmcc.Models.GeometalurgiaModels;

namespace geologycmcc.Controllers.Graficos
{
    public class GraficosGeomet
    {

        public Highcharts RecTotalCutPila(String holeid, String[] fechacarga, List<MineralRecuperacion> data)
        {





                Highcharts chart = new Highcharts("rectotalpilamin");
            chart.InitChart(new Chart { Type = ChartTypes.Column })
            .SetTitle(new Title { Text = "RECUPERACIÓN DE CUT PONDERADO DIARIO" })
            .SetSubtitle(new Subtitle { Text = @"PILA " + holeid.ToUpper() })
            .SetXAxis(new XAxis {
                Title = new XAxisTitle
                {
                    Text = "FECHA DE CARGA",
                    Style = "fontWeight: 'bold', fontSize: '12px'"

                },
                Labels = new XAxisLabels
                {
                    Rotation = -75
                },

                Categories = fechacarga
            })
                              .SetYAxis(new[] { new YAxis
                              {
                                  AllowDecimals = false,
                                  Min = 0,
                                  Title = new YAxisTitle
                                  {
                                      Text = "%, Mineral",
                                      Style = "fontWeight: 'bold', fontSize: '12px'"
                                  }
                              } , new YAxis
                              {
                                  AllowDecimals = false,
                                  Min = 0,
                                  Title = new YAxisTitle
                                  {
                                      Text = "%, Recuperación",
                                      Style = "fontWeight: 'bold', fontSize: '12px'"
                                  },
                                  Labels = new YAxisLabels {
                                      // Format = "{value}%",
                                  },
                                  Opposite = true
                              } })
                              .SetTooltip(new Tooltip { PointFormat = "{series.name}: <b>{point.percentage:.1f}%</b>", Enabled = true })
                              .SetPlotOptions(new PlotOptions
                              {
                                  Column = new PlotOptionsColumn { Stacking = Stackings.Normal },
                                  Series = new PlotOptionsSeries
                                  {
                                      Point = new PlotOptionsSeriesPoint
                                      {
                                          Events = new PlotOptionsSeriesPointEvents
                                          {
                                              MouseOver = "function(e) { $('#' + this.x).addClass('hover'); }",
                                              MouseOut = "function(e) { $('#' + this.x).removeClass('hover');}"
                                          }
                                      },


                                  }
                              })
                              .SetSeries(new[]
                              {
                                new Series
                                {
                                    Name = "OXIDO",
                                    Data = new Data(data.Select(x => x.OXIDO *100 /x.TONDIA).Cast<object>().ToArray()),
                                    Color = ColorTranslator.FromHtml("#008000"),
                                    Stack = "perforacion",
                                    //PlotOptionsColumn = new PlotOptionsColumn {
                                    //     DataLabels = new P {
                                    //         Enabled = true,
                                    //         Color = ColorTranslator.FromHtml("#000000")
                                    //     }
                                    //}
                                },
                                new Series
                                {
                                    Name = "SULFURO",
                                    Data = new Data(data.Select(x =>x.SULFURO *100 /x.TONDIA).Cast<object>().ToArray()),
                                    Color= ColorTranslator.FromHtml("#FF0000"),
                                    Stack = "perforacion"
                                },
                                new Series
                                {
                                    Name = "MSH",
                                    Data = new Data(data.Select(x =>x.Rec_CuT_Fecha_Carga *( x.MSH *100 /x.TONDIA )/100).Cast<object>().ToArray()),
                                   Color = ColorTranslator.FromHtml("#00FF00"),
                                    Stack = "perforacion"

                                },
                                new Series
                                {
                                    Name = "MSHB",
                                    Data = new Data(data.Select(x =>x.Rec_CuT_Fecha_Carga * ( x.MSHB *100 /x.TONDIA )/100).Cast<object>().ToArray()),
                                    Color = ColorTranslator.FromHtml("#A6FF00"),
                                    Stack = "perforacion"
                                },
                                   new Series
                                {
                                    Name = "MSHD",
                                    Data = new Data(data.Select(x =>x.Rec_CuT_Fecha_Carga *( x.MSHM *100 /x.TONDIA )/100).Cast<object>().ToArray()),

                                    Color = ColorTranslator.FromHtml("#FFE400"),
                                    Stack = "perforacion"
                                },
                                 new Series {
                                     PlotOptionsSpline = new PlotOptionsSpline {
                                        Tooltip = new PlotOptionsSplineTooltip {
                                         
                                            PointFormat = "{series.name}: <b>{point.y:.1f}%</b>"
                                        
                                        }
                                     },

                                     Name = "RECUPERACIÓN DE CUT",ZIndex=6,Type = ChartTypes.Spline, Data = new Data(data.Select(x =>x.Rec_CuT_Fecha_Carga).Cast<object>().ToArray()),  Color= Color.Orange, } }

                                  
                                 ).SetExporting(new Exporting
                                  {
                                      Enabled = true,

                                      Filename = "Chart_" + holeid.ToUpper()
                                  });
            
     

            return chart;

        }



    }
}
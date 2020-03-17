using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System.Drawing;

namespace geologycmcc.Controllers.Graficos
{
    public class CrearGrafico
    {
        public Highcharts graficoHidro(String nombre, String titulo, String dataXaxisTitulo, String[] dataXaxis, String dataYaxisTitulo, object[] serie1, object[] serie2, string nameSerie1, string nameSerie2, string sondaje)
        {

            //Creación del Gráfico
            Highcharts columnChart = new Highcharts(nombre);

            columnChart.InitChart(new Chart()
            {
                Type = DotNet.Highcharts.Enums.ChartTypes.Line,
                BackgroundColor = new BackColorOrGradient(System.Drawing.Color.WhiteSmoke),
                Style = "fontWeight: 'bold', fontSize: '17px'",
                BorderColor = System.Drawing.Color.LightBlue,
                BorderRadius = 0,
                BorderWidth = 2,

            });

            columnChart.SetTitle(new Title()
            {
                Text = titulo
            });

            columnChart.SetSubtitle(new Subtitle()
            {
                Text = "Reporte de Fecha: " + DateTime.Now.ToString() + "Sondaje:  " + sondaje
            });

            columnChart.SetXAxis(new XAxis()
            {

                Title = new XAxisTitle() { Text = dataXaxisTitulo, Style = "fontWeight: 'bold', fontSize: '20px'" },
                Categories = dataXaxis,
                Labels = new XAxisLabels()
                {
                    Rotation = -75
                },



            });

            columnChart.SetYAxis(new YAxis()
            {
                Title = new YAxisTitle()
                {
                    Text = dataYaxisTitulo,
                    Style = "fontWeight: 'bold', fontSize: '20px'"
                },


                ShowFirstLabel = true,
                ShowLastLabel = true,
                TickPixelInterval = 20,
                Min = 0


            });

            columnChart.SetLegend(new Legend
            {
                Enabled = true,
                BorderColor = System.Drawing.Color.CornflowerBlue,
                BorderRadius = 6,
                BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFADD8E6"))
            });

            columnChart.SetSeries(new Series[]
            {
                new Series{

                    Name = nameSerie1,
                    Data = new Data(serie1),


                },
                new Series {
                    Name = nameSerie2,
                    Data = new Data(serie2)

                }
            }
            );

            return columnChart;
        }


        public Highcharts graficoPerforacion(String nombre, String titulo, String dataXaxisTitulo, string[] dataXaxis, String dataYaxisTitulo, object[] serie1, object[] serie2, object[] serie3, object[] serie4, object[] serie5, object[] serie6)
        {
            Highcharts chart = new Highcharts(nombre)
                            .InitChart(new Chart { Type = ChartTypes.Column })
                            .SetTitle(new Title { Text = titulo })
                            .SetXAxis(new XAxis
                            {
                                Title = new XAxisTitle
                                {
                                    Text = dataXaxisTitulo,
                                    Style = "fontWeight: 'bold', fontSize: '12px'"
                                },
                                Categories = dataXaxis
                            })
                            .SetYAxis(new YAxis
                            {
                                AllowDecimals = false,
                                Min = 0,
                                Title = new YAxisTitle
                                {
                                    Text = dataYaxisTitulo,
                                    Style = "fontWeight: 'bold', fontSize: '12px'"
                                }
                            })
                            .SetTooltip(new Tooltip { Formatter = "TooltipFormatter" })
                            .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { Stacking = Stackings.Normal } })
                            .SetSeries(new[]
                            {
                                new Series
                                {
                                    Name = "ACT_PERFORACION",
                                    Data = new Data(serie1),
                                    Color = System.Drawing.Color.Brown,
                                    Stack = "perforacion"
                                },
                                new Series
                                {
                                    Name = "DNP",
                                    Data = new Data(serie2),
                                   Color = System.Drawing.Color.Red,
                                    Stack = "perforacion"
                                },
                                new Series
                                {
                                    Name = "DP",
                                    Data = new Data(serie3),
                                   Color = System.Drawing.Color.OrangeRed,
                                    Stack = "perforacion"

                                },
                                new Series
                                {
                                    Name = "RESERVA",
                                    Data = new Data(serie4),
                                    Color = System.Drawing.Color.Orange,
                                    Stack = "perforacion"
                                },
                                   new Series
                                {
                                    Name = "MANTENCION",
                                    Data = new Data(serie5),

                                    Color = System.Drawing.Color.DarkGreen,
                                    Stack = "perforacion"
                                },
                                      new Series
                                {
                                    Name = "PERFORADOS",
                                    Data = new Data(serie6),
                                     Color = System.Drawing.Color.Olive,

                                    Stack = "perforacion"
                                }
                            });


            return chart;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using geologycmcc.Controllers.Crud;
using geologycmcc.Models.HidrogeologiaModels;
using System.Drawing;

namespace geologycmcc.Controllers.Graficos
{
    public class GraficosHidrogeologia
    {


      
        public Highcharts graficoHidro(String nombre, String titulo, String dataXaxisTitulo, String[] dataXaxis, String dataYaxisTitulo, object[] serie1, object[] serie2, string nameSerie1, string nameSerie2, string sondaje)
        {

            //Creación del Gráfico
            Highcharts columnChart = new Highcharts(nombre);

            columnChart.InitChart(new Chart()
            {
                Type = DotNet.Highcharts.Enums.ChartTypes.Line,
                BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#F6f6f6")),
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
                Text = "Reporte de Fecha: " + DateTime.Now.ToString() + "  Sondaje:  " + sondaje
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

    }
}

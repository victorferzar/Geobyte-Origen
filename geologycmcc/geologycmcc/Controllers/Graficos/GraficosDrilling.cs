using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using geologycmcc.Controllers.Crud;
using geologycmcc.Models.DrillingModels;
using System.Drawing;
using System.Globalization;

namespace geologycmcc.Controllers.Graficos
{
    public class GraficosDrilling
    {

        private CRUDDrilling cd;
        public Highcharts GFBackLog()
        {
            cd = new CRUDDrilling();
            IEnumerable<MRFechaMetros> dataCumulated = cd.CumulatedDrilling();
            IEnumerable<MRFechaMetros> dataReleased = cd.ReleasedDataBase();
            IEnumerable<MRFechaMetros> dataPerdidos = cd.PerdidosDataBase();
            IEnumerable<MRFechaMetros> dataNoProcess = cd.NoProcessDataBase();
            IEnumerable<MRFechaMetros> dataBChemical = cd.BackLogChemical();
            IEnumerable<MRFechaMetros> dataRecepcionados = cd.Recepcionados();


            string[] name = dataCumulated.Select(x => x.FECHA.ToString("MMM yy",CultureInfo.CreateSpecificCulture("en-US"))).ToArray();
            double[] mtsCumulated = dataCumulated.Select(x => x.TOTAL).ToArray();
            double[] mtsReleased= dataReleased.Select(x => x.TOTAL).ToArray();
            double[] mtsPerdidos = dataPerdidos.Select(x => x.TOTAL).ToArray();
            double[] mtsNoProcess = dataNoProcess.Select(x => x.TOTAL).ToArray();
            double[] mtsbChemical = dataBChemical.Select(x => x.TOTAL).ToArray();
            double[] mtsRecep = dataRecepcionados.Select(x => x.TOTAL).ToArray();


            object[] mtsCumu = new object[mtsCumulated.Length];
            object[] mtsRel = new object[mtsReleased.Length];
            object[] mtsPer = new object[mtsPerdidos.Length];
            object[] mtsNoP = new object[mtsNoProcess.Length];
            object[] mtsBChe = new object[mtsbChemical.Length];
            object[] mtsR = new object[mtsRecep.Length];
            int j=0;
            for (int i = 0; i < name.Length; i++)
            {
                mtsCumu[i] = mtsCumulated[i];                        
            }
            j = 0;
            for (int i = 0; i < mtsReleased.Length; i++)
            {               
                j = mtsReleased[i] != 0? i : j;
                mtsRel[i] = mtsReleased[j];               
                
            }
            j = 0;
            for (int i = 0; i < mtsPerdidos.Length; i++)
            {               
                j = mtsPerdidos[i] != 0 ? i : j;               
                mtsPer[i] = mtsPerdidos[j] ;                

            }
            j = 0;
            for (int i = 0; i < mtsNoProcess.Length; i++)
            {
                j = mtsNoProcess[i] != 0 ? i : j;
                mtsNoP[i] = mtsNoProcess[j];
                
            }
            j = 0;
            for (int i = 0; i < mtsbChemical.Length; i++)
            {
                j = mtsbChemical[i] != 0 ? i : j;
                mtsBChe[i] = mtsbChemical[j];
            }
            j = 0;
            for (int i = 0; i < mtsRecep.Length; i++)
            {
                j = mtsRecep[i] != 0 ? i : j;
                System.Diagnostics.Debug.Write(("Chemical "+ mtsBChe[i]+ " No procesos " + mtsNoP[i] + " released " + mtsRel[i]) +Environment.NewLine);
                mtsR[i] = mtsRecep[j]-((double)mtsBChe[i]+ (double)mtsNoP[i] + (double)mtsRel[i]);
            }
            Highcharts chart = new Highcharts("GFBackLog")
               .InitChart(new Chart
               {
                   Type = ChartTypes.Column,
                   MarginTop = 80,
                   MarginRight = 40,


               })

               .SetTitle(new Title { Text = "BACKLOG", Style = "fontWeight: 'bold', fontSize: '20px', color:'#0000000'" })
               .SetPlotOptions(new PlotOptions
               {
              
                   Column = new PlotOptionsColumn
                   {

                       Stacking = Stackings.Normal,

                   }
               })
               .SetXAxis(new XAxis { Categories = name })
               .SetYAxis(new YAxis
               {

                   AllowDecimals = false,
                   Min = 0,
                   Title = new YAxisTitle
                   {
                       Text = "Metros",
                       Style = "fontWeight: 'bold', fontSize: '17px', color:'#0000000'"
                   }
               })
               .SetTooltip(new Tooltip
               {
                   HeaderFormat = "<b>{point.key}</b><br>",
                   PointFormat = "<span style=\"color:{series.color}\">\u25CF</span> {series.name}: {point.y} "
               })
               .SetSeries(new[]
               {
                    new Series { Name = "Cumulated Drilling",ZIndex=6,Type = ChartTypes.Spline, Data = new Data(mtsCumu), Color= ColorTranslator.FromHtml("#FF0000")},
                    new Series { Name = "Released Drilling",ZIndex=1,Type = ChartTypes.Column, Data = new Data(mtsRel), Color= ColorTranslator.FromHtml("#FF7F50")},
                    new Series { Name = "Perdidos Drilling",ZIndex=4,Type = ChartTypes.Column, Data = new Data(mtsPer), Color= ColorTranslator.FromHtml("#4169E1")},
                    new Series { Name = "No Procesados",ZIndex=5,Type = ChartTypes.Column, Data = new Data(mtsNoP), Color= ColorTranslator.FromHtml("#32CD32")},
                    new Series { Name = "Backlog in Chemical Analysis",ZIndex=3,Type = ChartTypes.Column, Data = new Data(mtsBChe), Color= ColorTranslator.FromHtml("#C0C0C0")},
                    new Series { Name = "Backlog in Data Capture",ZIndex=2,Type = ChartTypes.Column, Data = new Data(mtsR), Color= ColorTranslator.FromHtml("#708090")},


               });


            return chart;

        }

        public Highcharts GFResumenPerforacionFY() {
            cd = new CRUDDrilling();
            IEnumerable<MResumenPerforacionFY>data =  cd.ResumenPerforacionFY();
            string[] name = data.Select(x => x.PROJECTCODE).ToArray();
            double[] mtsPerforados = data.Select(x => x.DEPTH).ToArray();
            double[] mtsTotal= data.Select(x => x.PROF_PROPUESTA).ToArray();

            object[] mtsPerfo = new object[mtsPerforados.Length];
            object[] mtsTot= new object[mtsTotal.Length];
            for (int i = 0; i < name.Length; i++) {
                mtsPerfo[i] = mtsPerforados[i];
                mtsTot[i] = mtsTotal[i];
            }

            Highcharts chart  = new Highcharts("ResumenPerforacionFY")
               .InitChart(new Chart
               {
                   Type = ChartTypes.Column,
                   MarginTop = 80,
                   MarginRight = 40,
                  
               })
               
               .SetTitle(new Title { Text = "CAMPAÑA PERFORACIÓN", Style = "fontWeight: 'bold', fontSize: '20px', color:'#0000000'" })
               .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { DataLabels = new PlotOptionsColumnDataLabels
               {
                   Enabled = true,

                   Crop = false,
                   Overflow = "'none'",
                   Color = System.Drawing.Color.Black,
                   Formatter = "function() {  return  '<b>'+this.point.y + ' Mts.</b>' ; }"
               }




                 } })
               .SetXAxis(new XAxis {  Categories = name   })
               .SetYAxis(new YAxis
               {
                 
                   AllowDecimals = false,
                   Min = 0,
                   Title = new YAxisTitle {
                       Text = "Metros",
                       Style= "fontWeight: 'bold', fontSize: '17px', color:'#0000000'"
                   }
               })
               .SetTooltip(new Tooltip
               {
                   HeaderFormat = "<b>{point.key}</b><br>",
                   PointFormat = "<span style=\"color:{series.color}\">\u25CF</span> {series.name}: {point.y} "
               })
               .SetSeries(new[]
               {
                    new Series { Name = "Metros Perforados", Stack = "Perforados", Data = new Data(mtsPerfo), Color= ColorTranslator.FromHtml("#234483")
        },
                    new Series { Name = "Metros Propuestos", Stack = "Campaña", Data = new Data(mtsTot), Color = ColorTranslator.FromHtml("#1966b1")}

               });
            

            return chart;

        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System.Drawing;

namespace geologycmcc.Controllers.Graficos
{
    public class Points
    {
        public Color? Color { get; set; }
        public object DataLabels { get; set; }
        public Drilldown Drilldown { get; set; }
        public PlotOptionsSeriesPointEvents Events { get; set; }
        public string Id { get; set; }
        public bool? IsIntermediateSum { get; set; }
        public bool? IsSum { get; set; }
        public Number? LegendIndex { get; set; }
        public PlotOptionsSeriesMarker Marker { get; set; }
        public string Name { get; set; }
        public bool? Selected { get; set; }
        public bool? Sliced { get; set; }
        public DateTime X { get; set; }
        public Number? Y { get; set; }
    }
}
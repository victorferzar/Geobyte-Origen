using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.GeometalurgiaModels
{
    public class MineralPila
    {
        public DateTime FECHAMOVIMIENTO { get; set; }
        public Double TONDIA { get; set; }
        public double? OXIDO { get; set; }
        public double? SULFURO { get; set; }
        public double? MSH { get; set; }
        public double? MSHB { get; set; }
        public double? MSHM { get; set; }

        internal double Sum(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
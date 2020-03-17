using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.GeologiaModels
{
    public class StockControles
    {
        String STD { get; set; }
        String DESCRIPTION { get; set; }
        int STOCKGEOL { get; set; }
        int VALIDADOSGEOL { get; set; }
        int RECHAZADOSGEOL { get; set; }
        int UTILIZADOSGEOL { get; set; }
        int TOTALGEOL { get; set; }
        int STOCKPROD { get; set; }
        int UTILIZADOSPROD { get; set; }
        int TOTALPROD { get; set; }
        int TOTALFINAL { get; set; }

    }
}
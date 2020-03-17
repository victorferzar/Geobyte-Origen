using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.DocumentosModels
{
    public class DocumentListFoto
    {

        public string HOLEID { get; set; }
        public string PROJECTCODE { get; set; }
        public int PRIORITY { get; set; }
        public double GEOLFROM { get; set; }
        public double GEOLTO { get; set; }
        public String NAME { get; set; }
        public  byte[] VALUE { get; set; }

    }
}
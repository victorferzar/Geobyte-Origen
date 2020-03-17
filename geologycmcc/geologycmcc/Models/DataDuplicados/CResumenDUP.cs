using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.DataDuplicados
{
    public class CResumenDUP
    {
        public String Texto1 { get; set; }
        public String Muestra1 { get; set; }
        public String Muestra2 { get; set; }
        public String Diferencia { get; set; }
        public String VReal { get; set; }
        public String Ampd { get; set; }
        public String Ampd90 { get; set; }
        public String Ampd90O { get; set; }
        public String Separacion1 { get; set; }
        public String Texto2 { get; set; }
        public String Valor2 { get; set; }
        public String Separacion2 { get; set; }
        public String Texto3 { get; set; }
        public String Valor3 { get; set; }
        public String Name { get; set; }
        public String Stage { get; set; }


        public CResumenDUP(String texto1, String muestra1, String muestra2, String diferencia, String vReal, String ampd, String ampd90, String ampd90O, String separacion1, String texto2, String valor2, String separacion2, String texto3, String valor3, String name, String stage)
        {
            Texto1 = texto1;
            Muestra1 = muestra1;
            Muestra2 = muestra2;
            Diferencia = diferencia;
            VReal = vReal;
            Ampd = ampd;
            Ampd90 = ampd90;
            Ampd90O = ampd90O;
            Separacion1 = separacion1;
            Texto2 = texto2;
            Valor2 = valor2;
            Separacion2 = separacion2;
            Texto3 = texto3;
            Valor3 = valor3;
            Name = name;
            Stage = stage;



        }

    }
}
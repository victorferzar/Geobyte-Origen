using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;

namespace geologycmcc.Controllers.data
{
    public class ConversorCoordenadas
    {
        public string conversor(double utmEsteX, double utmNorteY)
        {


            double semiejemayor = 6378137;
            double semiejemenor = 6356752.314245;

            double excentricidad = Math.Round(Math.Sqrt((semiejemayor * semiejemayor) - (semiejemenor * semiejemenor)) / semiejemayor,9);
            double excentricidad2a = Math.Round(Math.Sqrt((semiejemayor * semiejemayor) - (semiejemenor * semiejemenor)) / semiejemenor, 9);

            double e2 = Math.Round(excentricidad2a * excentricidad2a,9);

            double radiopolar = Math.Round((semiejemayor * semiejemayor) / semiejemenor,3);

            utmEsteX = utmEsteX - 192.297;
            utmNorteY = utmNorteY - 379.879;


            double log = 0.0;
            double lat = 0.0;
            double huso = 19;
       
            double meridiano = 6 * huso - 183;
            double surecuador = Math.Round(utmNorteY - 10000000,3);
            double fi = Math.Round(surecuador / (6366197.724 * 0.9996),9);
            double ni = Math.Round((radiopolar / Math.Pow(1 + e2 * (Math.Pow(Math.Cos(fi), 2)), ((double)1 / (double)2))) * 0.9996,3);
            double a = Math.Round((utmEsteX - 500000) / ni,9);
            double A1 = Math.Round(Math.Sin(2 * fi),9);
            double A2 = Math.Round(A1 * Math.Pow(Math.Cos(fi), 2),9);
            double J2 = Math.Round(fi + (A1 / 2),8);
            double J4 = Math.Round((3 * J2 + A2) / (double)4,9);
            double J6 = Math.Round((5 * J4 + A2 * Math.Pow(Math.Cos(fi), 2))/3,9);
            double alfa = Math.Round(((double)3 / (double)4) * (double)e2,9);
            double beta = Math.Round((double)5 / (double)3,4) * Math.Pow(alfa, 2);
            double gamma = Math.Round((double)35 / (double)27,4) * Math.Pow(alfa, 3);
            double bfi = 0.9996 * radiopolar * (fi - (alfa * J2) + (beta * J4) - (gamma * J6));
            double b = (surecuador - bfi) / ni;
            double zeta = ((e2 * Math.Pow(a,2))/2)  * Math.Cos(fi);
            double xi = a * (1 - (zeta / (double)3));
            double eta = b * (1 - zeta) + fi;
            double senhxi = (Math.Exp(xi) - Math.Exp(-xi)) / (double)2;
            double deltalambda = Math.Atan(senhxi / Math.Cos(eta));
            double tau = Math.Atan(Math.Cos(deltalambda) * Math.Tan(eta));
            double firadianes = fi + (1 + e2 * Math.Pow(Math.Cos(fi), 2) - ((double)3/(double)2) * e2 * Math.Sin(fi) * Math.Cos(fi) * (tau - fi)) * (tau - fi);
            log = deltalambda / Math.PI * 180 + meridiano;
            lat = (firadianes / Math.PI) * 180;


            return "{ longitude: "+ log + ",latitude:" + lat + "}";
    
        }
    }
}
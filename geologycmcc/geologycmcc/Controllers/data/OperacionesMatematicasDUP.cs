using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;

namespace geologycmcc.Controllers.data
{
    public class OperacionesMatematicasDUP
    {

        public int numeroTotalMuestras(double?[] lista)
        {

            return lista.Length;
        }


        public double? mediadeMuestras(double?[] lista)
        {

            return lista.Average();
        }


        public double? devestEstimado(double? suma, double?[] data)
        {
            double dvt = (suma / data.Length).Value;
            return Math.Sqrt(dvt);
        }

        public double calcIntercepTO(List<double?> data_OR, List<double?> data_CK)
        {

            var application = new Application();
            var worksheetFunction = application.WorksheetFunction;

            var values1 = data_OR.Select(i => (double)i).ToList();
            var values2 = data_CK.Select(i => (double)i).ToList();


            return worksheetFunction.Intercept(values1.ToArray(), values2.ToArray());

        }

        public double calcRSQTO(List<double?> data_OR, List<double?> data_CK)
        {

            var application = new Application();
            var worksheetFunction = application.WorksheetFunction;

            var values1 = data_OR.Select(i => (double)i).ToList();
            var values2 = data_CK.Select(i => (double)i).ToList();


            return worksheetFunction.RSq(values1.ToArray(), values2.ToArray());

        }


        public double calcCorrelTO(List<double?> data_OR, List<double?> data_CK)
        {


            var application = new Application();
            var worksheetFunction = application.WorksheetFunction;

            var values1 = data_OR.Select(i => (double)i).ToList();
            var values2 = data_CK.Select(i => (double)i).ToList();


            return worksheetFunction.Correl(values1.ToArray(), values2.ToArray());

        }


        public double calcSlopeTO(DataPoint[] data)
        {
            double averageX = data.Average(d => d.X);
            double averageY = data.Average(d => d.Y);

            return data.Sum(d => (d.X - averageX) * (d.Y - averageY)) / data.Sum(d => Math.Pow(d.X - averageX, 2));

        }

        public double calcSlopeTO(List<double?> data_OR, List<double?> data_CK)
        {

            DataPoint[] data = new DataPoint[data_OR.Count];
            for (int i = 0; i < data_OR.Count; i++)
            {

                data[i] = new DataPoint(data_OR[i].Value, data_CK[i].Value);

            }
            double averageX = data.Average(d => d.X);
            double averageY = data.Average(d => d.Y);

            return data.Sum(d => (d.X - averageX) * (d.Y - averageY)) / data.Sum(d => Math.Pow(d.X - averageX, 2));

        }


        /* Filas DUP 6 */
        public double? calcTestT(double? pd, int cant, double? dstD)
        {
            return (pd / Math.Sqrt(cant) * dstD);
        }


        /* Filas DUP 7 - 8 */
        public double? calcSesgo(double? promM1, double? promM2)
        {
            double? sesgo = (promM1 - promM2) / promM1 * 100;

            return sesgo;
        }

        public double? calcPercentile(double?[] sequence, double excelPercentile)
        {
            Array.Sort(sequence);
            int N = sequence.Length;
            double n = (N - 1) * excelPercentile + 1;
            if (n == 1d) return sequence[0];
            else if (n == N) return sequence[N - 1];
            else
            {
                int k = (int)n;
                double d = n - k;
                return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
            }
        }



        public double calcTotalResAMPD(double?[] data)
        {
            double? resultAMPD = 0;

            for (int i = 0; i < data.Count(); i++)
            {
                resultAMPD += data[i] * data[i];
            }
            return (double)resultAMPD / data.Length;
        }

        public double? calcErrMedioVREL(double? promVREL)
        {

            return Math.Sqrt((double)promVREL * 100);
        }

        public double? calcErrMedioAMPD(double totalResAMPD)
        {
            return Math.Sqrt(totalResAMPD);
        }

        public string conversor3Decimales(double? valor)
        {
            return (((double)((int)(valor * 10000.0))) / 10000.0).ToString();
        }


    }

    public class DataPoint
    {


        public double X { get; set; }
        public double Y { get; set; }

        public DataPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
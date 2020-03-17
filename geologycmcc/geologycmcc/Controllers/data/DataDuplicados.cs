using System;
using System.Collections.Generic;
using System.Linq;
using geologycmcc.Models;
using geologycmcc.Models.DataDuplicados;
using geologycmcc.Models.GeologiaQAQCModels;
using System.Web;
using Microsoft.Office.Interop.Excel;

namespace geologycmcc.Controllers.data
{
    public class DataDuplicados
    {
        private DM_CC_SONEntities ddb;
        private List<CResumenDUP> listaTablaHTML;
        private List<CDetalleDUP> listaDetalleHTML;
        double? mediaAMPD;
        double? promM1;
        double? promM2;
        double? promDIF;
        //double? promVREL;
        double? desvestAMPD;
        double? desvestM1;
        double? desvestM2;
        double? desvestDIF;
        double totalResAMPD;

        OperacionesMatematicasDUP opdup;


        public DataDuplicados()
        {
            ddb = new DM_CC_SONEntities();
            opdup = new OperacionesMatematicasDUP();

        }

        //Data a cargar en todo el Documento de duplicados
        public IEnumerable<CQUAQCDUP> dataDuplicados()
        {

            var lista = (from l in ddb.DTM_QAQC_DUP 

             select new CQUAQCDUP
              {
                  NREGISTRO = 0,
                  PROJECTCODE = l.PROJECTCODE,
                  STATUS = l.DTM_COLLAR.STATUS,
                  PRIORITY_OR = l.PRIORITY_OR,
                  HOLEID = l.HOLEID,
                  ID_OR = l.ID_OR,
                  ANALYSISSUITE = l.ANALYSISSUITE,
                  SAMPFROM = l.SAMPFROM,
                  SAMPTO = l.SAMPTO,
                  ASSAYVALUE_OR = l.ASSAYVALUE_OR,
                  ASSAYVALUE_CK = l.ASSAYVALUE_CK,
                  DIFERENCIA = l.DIFERENCIA,
                  VAR_REL = l.VAR_REL,
                  NPORCDATOS = 0,
                  AMPD = l.AMPD,
                  PROMEDIO = l.PROMEDIO,
                  MPD = l.MPD,
                  NAMPDPOND = 0,
                  NAMPORD = 0,
                  NZSCORE = 0,
                  SAMPLE_DRILTYPE = l.SAMPLE_DRILLTYPE,
                  CHECKSTAGE = l.CHECKSTAGE,
                  ASSAYNAME = l.ASSAYNAME,
                  RETURNDATE = l.RETURNDATE.ToString(),
                  DESPATCHNO = l.DESPATCHNO,
                  LABJOBNO = l.LABJOBNO_OR,
                  ID_CK = l.ID_CK,
                  LABCODE = l.LABCODE
              }).ToList();


            return lista;

        }
        public double CalculateStdDev(IEnumerable<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                //Compute the Average      
                double avg = values.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }
        public double Median(IEnumerable<double> xs)
        {
            var ys = xs.OrderBy(x => x).ToList();
            double mid = (ys.Count - 1) / 2.0;
            return (ys[(int)(mid)] + ys[(int)(mid + 0.5)]) / 2;
        }


        public IEnumerable<CQUAQCDUP> dataDuplicados(IEnumerable<CQUAQCDUP> lista)
        {


            if (lista != null)
            {

                double?[] ampdOrdenada = lista.OrderBy(x => x.AMPD).Select(x => x.AMPD).ToList().ToArray();

                double? resultAMPD = 0;
                for (int i = 0; i < ampdOrdenada.Count(); i++)
                {
                    resultAMPD += ampdOrdenada[i] * ampdOrdenada[i];
                }
                totalResAMPD = (double)resultAMPD / ampdOrdenada.Length;





                double? sum = lista.Select(x => x.AMPD).Sum(d => (d - mediaAMPD) * (d - mediaAMPD));
                desvestAMPD = opdup.devestEstimado(sum, ampdOrdenada);

                double? sumOR = lista.Select(x => x.ASSAYVALUE_OR).Sum(d => (d - promM1) * (d - promM1));
                desvestM1 = opdup.devestEstimado(sumOR, ampdOrdenada);

                double? sumCK = lista.Select(x => x.ASSAYVALUE_CK).Sum(d => (d - promM2) * (d - promM2));
                desvestM2 = opdup.devestEstimado(sumCK, ampdOrdenada);

                double? sumDIF = lista.Select(x => x.DIFERENCIA).Sum(d => (d - promDIF) * (d - promDIF));
                desvestDIF = opdup.devestEstimado(sumDIF, ampdOrdenada);
                mediaAMPD = opdup.mediadeMuestras(lista.Select(x => x.AMPD).ToArray());

                //SET;  CALCULOS MATEMATICOS
                int idCorrelativo = 0;
                int indice = 0;
                int total = ampdOrdenada.Length;
                foreach (var item in lista)
                {
                    if (item.DIFERENCIA == null)
                    {
                        item.DIFERENCIA = 0;
                    }

                    if (item.ASSAYVALUE_CK == null)
                    {
                        item.ASSAYVALUE_CK = 0;
                    }

                    if (item.ASSAYVALUE_OR == null)
                    {
                        item.ASSAYVALUE_OR = 0;
                    }
                    item.DIFERENCIA = ((double)((int)(item.DIFERENCIA * 10000.0))) / 10000.0; // (double?)(Math.Truncate(decimal.Parse(item.DIFERENCIA.Value.ToString()) * 10000) / 10000);
                    item.VAR_REL = ((double)((int)(item.VAR_REL * 10000.0))) / 10000.0;
                    item.AMPD = ((double)((int)(item.AMPD * 10000.0))) / 10000.0;
                    item.MPD = ((double)((int)(item.MPD * 10000.0))) / 10000.0;
                    item.NREGISTRO = ++idCorrelativo;
                    item.NAMPORD = ((double)((int)(ampdOrdenada[indice] * 10000.0))) / 10000.0;
                    item.NAMPDPOND = ((double)((int)(calculoAMPDPOND(ampdOrdenada, idCorrelativo) * 10000.0))) / 10000.0;
                    item.NZSCORE = ((double)((int)((item.AMPD - mediaAMPD) / desvestAMPD * 10000.0))) / 10000.0;
                    item.NPORCDATOS = ((double)((int)((double)(((item.NREGISTRO * 1.0) / (total * 1.0)) * 100.0) * 10000.0))) / 10000.0;         //(double)(((item.NREGISTRO * 1.0) / (total * 1.0))*100.0);
                    indice++;
                }



            }
            return lista;

        }







        public IEnumerable<CQAQCSTD> dataSTD()
        {

            var lista = ((from l in ddb.DTM_QAQC_BLK_STD
                          join c in ddb.DTM_STANDARDSASSAY on new { p1 = (string)l.STANDARDID, p2 = l.ASSAYNAME } equals new { p1 = c.STANDARDID, p2 = c.NAME }


                          select new CQAQCSTD
                          {
                              INDICE = 0,
                              HOLEID = l.HOLEID,
                              CHECKID = l.CHECKID,
                              STANDARDID = l.STANDARDID,
                              ANALYSISSUITE = l.ANALYSISSUITE,
                              ASSAY_PRIORITY = l.ASSAY_PRIORITY,
                              ASSAYVALUE = l.ASSAYVALUE,
                              LABCODE = l.LABCODE,
                              ASSAYNAME = l.ASSAYNAME,
                              SENDDATE = l.SENDDATE,
                              RETURNDATE = l.RETURNDATE.ToString(),
                              DESPATCHNO = l.DESPATCHNO,
                              LABJOBNO = l.LABJOBNO,
                              MIN = c.ACCEPTABLEMIN,
                              MAX = c.ACCEPTABLEMAX,
                              NORMALIZACION = Math.Round((double)((l.ASSAYVALUE - c.STANDARDVALUE) / c.STANDARDDEVIATION), 8),
                              DEV = c.STANDARDDEVIATION,
                              STANDARDVALUE = c.STANDARDVALUE



                          })).ToList();



            return lista;

        }
        //Funcion null para array
        public bool IsNullOrEmpty(string[] projectList)
        {
            return (projectList == null || projectList.Length == 0);
        }


        // cALCULO MATEMATICO PONDERACION
        public double? calculoAMPDPOND(double?[] arreglo, int hasta)
        {
            double? suma = 0;
            if (hasta == 1)
            {
                suma = arreglo[0];

            }
            else if (hasta == 2)
            {
                suma = arreglo[0] + arreglo[1];
            }
            else {
                for (int i = 1; i < hasta; i++)
                {
                    suma += arreglo[i];
                }
            }
            double calculo = ((suma * 2) / hasta).Value;

            return Math.Sqrt(calculo);
        }



        // TABLA DE DATOS CALCULOS MATEMATICOS
        public List<CResumenDUP> dataDuplicadosTabla(IEnumerable<CQUAQCDUP> dataD)
        {

            listaTablaHTML = new List<CResumenDUP>();

            //TEXTOS FIJOS
            String[] listTexto1 = { "NUMERO", "MINIMO", "MAXIMO", "PROMEDIO", "DESV. EST.", "TEST T", "ERR. REL. MEDIO", "SESGO (%)" };
            String[] listTexto2 = { "MEDIA", "DESVEST" };
            String[] listTexto3 = { "R", "R2", "INTERCEPTO", "PROMEDIO", "COEF. DE X" };

            string[] nombre = dataD.Select(x => x.ASSAYNAME).Distinct().ToArray();
            string[] st = dataD.Select(x => x.CHECKSTAGE).Distinct().ToArray();

            if (nombre.Length <= 0)
            {
                nombre = new string[1];
                nombre[0] = "";
            }


            if (st.Length <= 0)
            {
                st = new string[1];
                st[0] = "";
            }


            var application = new Application();
            var worksheetFunction = application.WorksheetFunction;

            //FILA 1


            var valuesOR = dataD.Select(x => (double)x.ASSAYVALUE_OR).ToList();
            var valuesCK = dataD.Select(x => (double)x.ASSAYVALUE_CK).ToList();
            var valuesAMPD = dataD.Select(x => (double)x.AMPD).ToList();
            var valuesDIF = dataD.Select(x => (double)x.DIFERENCIA).ToList();
            var valuesVREL = dataD.Select(x => (double)x.VAR_REL).ToList();


            string uno = worksheetFunction.Count(valuesOR.ToArray()).ToString();
            String dos = worksheetFunction.Count(valuesOR.ToArray()).ToString();
            String tres = worksheetFunction.Count(valuesOR.ToArray()).ToString();
            String cuatro = worksheetFunction.Count(valuesOR.ToArray()).ToString();
            String cinco = conversor3Decimales(worksheetFunction.Average(valuesAMPD.ToArray())).ToString();
            String seis = (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.Correl(valuesOR.ToArray(), valuesCK.ToArray())).ToString();
           
                listaTablaHTML.Add(new CResumenDUP(listTexto1[0], worksheetFunction.Count(valuesOR.ToArray()).ToString(), worksheetFunction.Count(valuesOR.ToArray()).ToString(), worksheetFunction.Count(valuesOR.ToArray()).ToString(), worksheetFunction.Count(valuesOR.ToArray()).ToString(), "", "", "", "", listTexto2[0].ToString(), conversor3Decimales(worksheetFunction.Average(valuesAMPD.ToArray())).ToString(), "", listTexto3[0].ToString(), (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.Correl(valuesOR.ToArray(), valuesCK.ToArray())).ToString(), nombre[0].ToString(), st[0].ToString()));

                //FILA 2
                listaTablaHTML.Add(new CResumenDUP(listTexto1[1], worksheetFunction.Min(valuesOR.ToArray()).ToString(), worksheetFunction.Min(valuesCK.ToArray()).ToString(), worksheetFunction.Min(valuesDIF.ToArray()).ToString(), worksheetFunction.Min(valuesVREL.ToArray()).ToString(), "", "", "", "", listTexto2[1], (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.StDev(valuesAMPD.ToArray())), "", listTexto3[1], (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.RSq(valuesOR.ToArray(), valuesCK.ToArray())), nombre[0], st[0]));

                //FILA 3
                listaTablaHTML.Add(new CResumenDUP(listTexto1[2], worksheetFunction.Max(valuesOR.ToArray()).ToString(), worksheetFunction.Max(valuesCK.ToArray()).ToString(), worksheetFunction.Max(valuesDIF.ToArray()).ToString(), worksheetFunction.Max(valuesVREL.ToArray()).ToString(), "", "", "", "", listTexto3[2], (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.Intercept(valuesOR.ToArray(), valuesCK.ToArray())), "", "", "", nombre[0], st[0]));

                //FILA 4
                listaTablaHTML.Add(new CResumenDUP(listTexto1[3], conversor3Decimales(worksheetFunction.Average(valuesOR.ToArray())), conversor3Decimales(worksheetFunction.Average(valuesCK.ToArray())), conversor3Decimales(worksheetFunction.Average(valuesDIF.ToArray())), conversor3Decimales(worksheetFunction.Average(valuesVREL.ToArray())), "", "", "", "", listTexto3[3], (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.Slope(valuesOR.ToArray(), valuesCK.ToArray())), "", "", "", nombre[0], st[0]));

                //FILA 5
                listaTablaHTML.Add(new CResumenDUP(listTexto1[4], (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.StDev(valuesOR.ToArray())), (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.StDev(valuesCK.ToArray())), (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.StDev(valuesDIF.ToArray())), "", "", "", "", "", "", "", "", "", "", nombre[0], st[0]));
            try
            {
                //FILA 6
                listaTablaHTML.Add(new CResumenDUP(listTexto1[5], "", "", "", (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : "Error", "", "", "", "", "", "", "", "", "", nombre[0], st[0]));
            }
            catch (Exception EX) { }
            //FILA 7  TRABAJARA CON ESTOS 2 ARREGLOS
            var valuesAMPDORD = dataD.OrderBy(x => x.NAMPORD).Select(x => (double)x.NAMPORD).ToList();
                var valuesAMPDPOND = dataD.OrderBy(x => x.NAMPDPOND).Select(x => (double)x.NAMPDPOND).ToList();


                double valorPercentile = 0.90;

                //FILA 7
                listaTablaHTML.Add(new CResumenDUP(listTexto1[6], "", "", "", conversor3Decimales(worksheetFunction.SqrtPi(worksheetFunction.Average(valuesVREL.ToArray())) * 100), conversor3Decimales(worksheetFunction.SqrtPi(worksheetFunction.SumProduct(valuesAMPD.ToArray(), valuesAMPD.ToArray()) / worksheetFunction.Count(valuesOR.ToArray()))), (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.Percentile(valuesAMPDPOND.ToArray(), valorPercentile)).ToString(), (valuesOR.Count <= 1 || valuesCK.Count <= 1) ? "Error" : conversor3Decimales(worksheetFunction.Percentile(valuesAMPDORD.ToArray(), valorPercentile)).ToString(), "", "", "", "", "", "", nombre[0], st[0]));

                //FILA 8
                listaTablaHTML.Add(new CResumenDUP(listTexto1[7], conversor3Decimales(calcSesgo(worksheetFunction.Average(valuesOR.ToArray()), worksheetFunction.Average(valuesCK.ToArray()))), "", "", "", "", "", "", "", "", "", "", "", "", nombre[0], st[0]));
          
            return listaTablaHTML;
        }

        public List<CDetalleDUP> dataDetalleTabla(IEnumerable<CQUAQCDUP> dataD)
        {
            listaDetalleHTML = new List<CDetalleDUP>();


            foreach (var x in dataD)
            {
                listaDetalleHTML.Add(new CDetalleDUP(x.NREGISTRO.ToString(), x.HOLEID, x.ID_OR, x.ID_CK, x.CHECKSTAGE, x.SAMPFROM, x.SAMPTO, x.ASSAYNAME, x.ASSAYVALUE_OR, x.ASSAYVALUE_CK, x.DIFERENCIA, x.NPORCDATOS, x.VAR_REL, x.AMPD, x.PROMEDIO, x.MPD, x.NAMPDPOND, x.NAMPORD, x.NZSCORE));

            }




            return listaDetalleHTML;
        }


        /* Cortar a 3 decimales valores double? */
        public string conversor3Decimales(double? valor)
        {
            return (((double)((int)(valor * 10000.0))) / 10000.0).ToString();
        }

        public double? calcSesgo(double? promM1, double? promM2)
        {
            double? sesgo = (promM1 - promM2) / promM1 * 100;

            return sesgo;
        }
    }
}
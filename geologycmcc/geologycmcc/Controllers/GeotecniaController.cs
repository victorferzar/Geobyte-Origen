using geologycmcc.Controllers.Crud;
using geologycmcc.Controllers.DataBase;
using geologycmcc.Models.GeotecniaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;



namespace geologycmcc.Controllers
{
 

    [Authorize(Roles = @"Americas\chachl9,Americas\sancjc,Americas\verarc9,Americas\bustce,Americas\gonzcc,Americas\mulej,Americas\malejf,AMERICAS\estacd,
AMERICAS\mereg,
AMERICAS\valljb,
AMERICAS\escofa,
AMERICAS\quinva,
AMERICAS\alvapa2,
AMERICAS\bricpc,
AMERICAS\arcema,
AMERICAS\silvd,
AMERICAS\hidadf,
AMERICAS\sepuy,
AMERICAS\poolra,
AMERICAS\cespjl,
AMERICAS\escaj,
AMERICAS\becec,
AMERICAS\aedol,
AMERICAS\ferrra,
AMERICAS\becec,
AMERICAS\matih,
AMERICAS\BUSTCE,
AMERICAS\yanen9")]
    public class GeotecniaController : Controller
    {
        // GET: Geotecnia
        public ActionResult Index()
        {
            return View();
        }
        // Supporting function that converts an image to base64.
     
        public ActionResult RIGEOT(String id) {
            Conexion cn;

            try
            {
                cn = new Conexion();
       
         
                SqlCommand com = new SqlCommand("SELECT TOP 100 PERCENT * FROM (SELECT * FROM (SELECT TOP 100 PERCENT * FROM ( SELECT [HOLELOCATION].[HOLEID], [HOLELOCATION].[PROJECTCODE], [HOLELOCATION].[HOLETYPE], [HOLEDETAILS].[RECARGO], [HOLECOMMENT].[RECCONTROL], [HOLECOMMENT].[RECEJECUCION], [HOLECOMMENT].[RECEMISOR], [HOLEDETAILS].[RECFECHA], [HOLEDETAILS].[RECFECHACIERRE], [HOLECOMMENT].[RECLUGAR], [HOLEDETAILS].[RECTURNO], [HOLEDETAILS].[RGN], [HOLEBIGCOMMENT].[RECANTECEDENTES], [HOLEBIGCOMMENT].[RECASUNTO], [HOLEBIGCOMMENT].[RECPATCHFOTO], [HOLEBIGCOMMENT].[RECPATCHFOTOCIERRE], [HOLEBIGCOMMENT].[RECEVALUACION], [HOLEBIGCOMMENT].[RECRECOMENDACION], [HOLELOCATION].[RL], [HOLELOCATION].[PROSPECT], [HOLELOCATION].[STARTDATE], [HOLELOCATION].[ENDDATE], [HOLEDETAILS].[RECFECHAPROGCIERRE], [HOLEDETAILS].[RECZONA], [HOLECOMMENT].[RECEJECUCIONCARGO], [HOLEBIGCOMMENT].[RECEVALUACION2], [HOLEBIGCOMMENT].[RECEVALUACION3], [HOLEBIGCOMMENT].[RECPATCHFOTO2], [HOLEBIGCOMMENT].[RECPATCHFOTO3], [HOLEDETAILS].[RECTIPO], [HOLEBIGCOMMENT].[RECPATCHACCIONES1] FROM [HOLELOCATION] INNER JOIN (SELECT * FROM [HOLELOCATION]) AS [CollarWSF] ON [CollarWSF].[HOLEID] = [HOLELOCATION].[HOLEID] AND [CollarWSF].[PROJECTCODE] = [HOLELOCATION].[PROJECTCODE] LEFT JOIN (SELECT [HOLEDETAILS].[HOLEID], [HOLEDETAILS].[PROJECTCODE], min(CASE when [HOLEDETAILS].[NAME] = 'RECARGO' then [HOLEDETAILS].[VALUE] ELSE NULL END) as [RECARGO], min(CASE when [HOLEDETAILS].[NAME] = 'RECFECHA' then [HOLEDETAILS].[VALUE] ELSE NULL END) as [RECFECHA], min(CASE when [HOLEDETAILS].[NAME] = 'RECFECHACIERRE' then [HOLEDETAILS].[VALUE] ELSE NULL END) as [RECFECHACIERRE], min(CASE when [HOLEDETAILS].[NAME] = 'RECTURNO' then [HOLEDETAILS].[VALUE] ELSE NULL END) as [RECTURNO], min(CASE when [HOLEDETAILS].[NAME] = 'RGN' then [HOLEDETAILS].[VALUE] ELSE NULL END) as [RGN], min(CASE when [HOLEDETAILS].[NAME] = 'RECFECHAPROGCIERRE' then [HOLEDETAILS].[VALUE] ELSE NULL END) as [RECFECHAPROGCIERRE], min(CASE when [HOLEDETAILS].[NAME] = 'RECZONA' then [HOLEDETAILS].[VALUE] ELSE NULL END) as [RECZONA], min(CASE when [HOLEDETAILS].[NAME] = 'RECTIPO' then[HOLEDETAILS].[VALUE] ELSE NULL END) as [RECTIPO] FROM[HOLEDETAILS] WHERE[HOLEDETAILS].[NAME] IN('RECARGO', 'RECFECHA', 'RECFECHACIERRE', 'RECTURNO', 'RGN', 'RECFECHAPROGCIERRE', 'RECZONA', 'RECTIPO') GROUP BY[HOLEDETAILS].[PROJECTCODE], [HOLEDETAILS].[HOLEID])[HOLEDETAILS] ON[HOLELOCATION].[PROJECTCODE] = [HOLEDETAILS].[PROJECTCODE]  AND[HOLELOCATION].[HOLEID] = [HOLEDETAILS].[HOLEID]" +
        "LEFT JOIN(SELECT[HOLECOMMENT].[HOLEID], [HOLECOMMENT].[PROJECTCODE], min(CASE when [HOLECOMMENT].[NAME] = 'RECCONTROL' then[HOLECOMMENT].[VALUE] ELSE NULL END) as [RECCONTROL], min(CASE when [HOLECOMMENT].[NAME] = 'RECEJECUCION' then[HOLECOMMENT].[VALUE] ELSE NULL END) as [RECEJECUCION], min(CASE when [HOLECOMMENT].[NAME] = 'RECEMISOR' then[HOLECOMMENT].[VALUE] ELSE NULL END) as [RECEMISOR], min(CASE when [HOLECOMMENT].[NAME] = 'RECLUGAR' then[HOLECOMMENT].[VALUE] ELSE NULL END) as [RECLUGAR], min(CASE when [HOLECOMMENT].[NAME] = 'RECEJECUCIONCARGO' then[HOLECOMMENT].[VALUE] ELSE NULL END) as [RECEJECUCIONCARGO]" +
        "FROM[HOLECOMMENT] WHERE[HOLECOMMENT].[NAME] IN('RECCONTROL', 'RECEJECUCION', 'RECEMISOR', 'RECLUGAR', 'RECEJECUCIONCARGO') GROUP BY[HOLECOMMENT].[PROJECTCODE], [HOLECOMMENT].[HOLEID])[HOLECOMMENT]" +
        "ON[HOLELOCATION].[PROJECTCODE] = [HOLECOMMENT].[PROJECTCODE]" +
        "AND[HOLELOCATION].[HOLEID] = [HOLECOMMENT].[HOLEID]" +
        "LEFT JOIN(SELECT[HOLEBIGCOMMENT].[HOLEID], [HOLEBIGCOMMENT].[PROJECTCODE], min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECANTECEDENTES' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECANTECEDENTES], min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECASUNTO' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECASUNTO], min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECPATCHFOTO' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECPATCHFOTO], min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECPATCHFOTOCIERRE' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECPATCHFOTOCIERRE], min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECEVALUACION' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECEVALUACION]," +
 "min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECRECOMENDACION' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECRECOMENDACION], min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECEVALUACION2' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECEVALUACION2], min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECEVALUACION3' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECEVALUACION3], min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECPATCHFOTO2' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECPATCHFOTO2], min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECPATCHFOTO3' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECPATCHFOTO3], min(CASE when [HOLEBIGCOMMENT].[NAME] = 'RECPATCHACCIONES1' then[HOLEBIGCOMMENT].[VALUE] ELSE NULL END) as [RECPATCHACCIONES1]" +
        "FROM[HOLEBIGCOMMENT] GROUP BY[HOLEBIGCOMMENT].[PROJECTCODE], [HOLEBIGCOMMENT].[HOLEID])[HOLEBIGCOMMENT]" +
        "ON[HOLELOCATION].[PROJECTCODE] = [HOLEBIGCOMMENT].[PROJECTCODE]" +
        "AND[HOLELOCATION].[HOLEID] = [HOLEBIGCOMMENT].[HOLEID]) [ACQTMP]  ) AS[TMPVIEW4]) [TMPSQLSHEETVIEW]" +


"WHERE(([TMPSQLSHEETVIEW].[RGN] = '" + id + "'))", cn.openDB2());

                SqlDataReader dr = com.ExecuteReader();

                if (dr.Read()) {
                    ViewBag.RGN = dr.GetString(11);
                    ViewBag.RECCONTROL = dr.GetString(4);
                    ViewBag.RECARGO = dr.IsDBNull(3) ? null : dr.GetString(3);
                    ViewBag.STARTDATE = dr.GetString(20);
                    ViewBag.RECLUGAR = dr.GetString(9);
                    ViewBag.RECZONA = dr.GetString(23);
                    ViewBag.RL = dr.GetDouble(18);
                    ViewBag.RECTURNO = dr.GetString(10);
                    ViewBag.RECASUNTO = dr.GetString(13);                 
                    ViewBag.RECPATCHFOTO = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(System.IO.File.ReadAllBytes(dr.GetString(14))));
                    ViewBag.RECEVALUACION = dr.GetString(16);
                    ViewBag.RECPATCHFOTO2 = dr.IsDBNull(27) ? null : string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(System.IO.File.ReadAllBytes(dr.GetString(27)))); 
                    ViewBag.RECPATCHFOTO3 = dr.IsDBNull(28) ? null : string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(System.IO.File.ReadAllBytes(dr.GetString(28)))); 
                    ViewBag.RECEVALUACION2 = dr.IsDBNull(25) ? null : dr.GetString(25);
                    ViewBag.RECEVALUACION3 = dr.IsDBNull(26) ? null : dr.GetString(26);
                    ViewBag.RECRECOMENDACION = dr.IsDBNull(17) ? null : dr.GetString(17);
                    ViewBag.RECPATCHACCIONES1 = dr.IsDBNull(30) ? null : string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(System.IO.File.ReadAllBytes(dr.GetString(30)))) ;
           
                    ViewBag.RECEJECUCION = dr.IsDBNull(5) ? null : dr.GetString(5);
                    ViewBag.RECEJECUCIONCARGO = dr.IsDBNull(24) ? null : dr.GetString(24);
                    ViewBag.RECFECHAPROGCIERRE = dr.IsDBNull(22) ? null : dr.GetString(22);
                    ViewBag.RECPATCHFOTOCIERRE = dr.IsDBNull(15) ? null : string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(System.IO.File.ReadAllBytes(dr.GetString(15)))) ;
                }
                String dato ="";
                if (ViewBag.RECCONTROL == "CARLOS CORNEJO CORTÉS")
                {
                    dato = "INGENIERO GEOTÉCNICO SÉNIOR";
                }
                else if (ViewBag.RECCONTROL == "JUAN PABLO MULET" || ViewBag.RECCONTROL == "CONSTANZA GONZÁLEZ CANNOBBIO")
                {
                    dato = "INGENIERO GEOTÉCNICO";

                }
                else {
                    dato = "OPERADOR GEOTÉCNICO";
                }
                ViewBag.RECCARGOCONTROL = dato;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            // CRUDGeotecnia crudhg = new CRUDGeotecnia();
            //RigeotReporte dato = crudhg.reporteGeotecnia(id);

            // ViewBag.RIGEOTNUM = dato.HOLEID;
            
            return View();
        }
        public ActionResult PrintRIGEOT(String id)
        {
            var report = new Rotativa.ActionAsPdf("RIGEOT", id);
            return report;
        }

    }
}
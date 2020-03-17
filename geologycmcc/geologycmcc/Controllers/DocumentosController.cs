using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using geologycmcc.Controllers.Crud;
using System.Data.SqlClient;
using RDotNet;
using System.IO;

namespace geologycmcc.Controllers
{
    /*[Authorize(Roles = @"Americas\chachl9,Americas\verarc9,Americas\danicda9,Americas\sancjc,Americas\bustce,Americas\bustv9,Americas\cistee,Americas\escofa,AMERICAS\estacd,
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
AMERICAS\matih,
AMERICAS\becec,
AMERICAS\smoji9,
AMERICAS\silvfj9,
AMERICAS\guzmja
")]*/
    public class DocumentosController : Controller
    {
        private CRUDDocumentos crud;
        static int FISCALYEARGLOBAL = 0;
        private static String vid = "0";
        private static String vholeid = "0";
        // ################################## DOCUMENTOS PDF


        public ActionResult Index()
        {
            crud = new CRUDDocumentos();

            var listFY = crud.listaSondaje().OrderBy(z => z.FISCAL_YEAR).Select(x => x.FISCAL_YEAR).Distinct().ToList();
            ViewBag.Fiscal_Year = listFY.Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();

            return View();
        }

        public ActionResult ListadoDocumentos(String FISCAL_YEAR, String PROJECTCODE, String HOLEID)
        {
            crud = new CRUDDocumentos();

            ViewBag.MenuHeadSondaje = crud.listaFileListCategory(HOLEID).ToList();
      
            ViewBag.MenuItemSondaje = crud.listaFileList(HOLEID).ToList();


            return View();
        }
        public ActionResult AdminDocumentoSondajes()
        {
            crud = new CRUDDocumentos();

            var listFY = crud.listaSondaje().OrderBy(z => z.FISCAL_YEAR).Select(x => x.FISCAL_YEAR).Distinct().ToList();
            ViewBag.Fiscal_Year = listFY.Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();

            return View();
        }


        public ActionResult AdminDocumentoSondajesAdd(String HOLEID, String PROJECTCODE)
        {
            crud = new CRUDDocumentos();

            ViewBag.holeidv = HOLEID;
            ViewBag.projectv = PROJECTCODE;
            ViewBag.subcatergoria = crud.listaSubCategority(HOLEID).Select(x => new SelectListItem { Value = x.DESCRIPTION, Text = x.DESCRIPTION }).ToList();

            ViewBag.MenuHeadSondaje = crud.listaFileListCategory(HOLEID).ToList();

            ViewBag.MenuItemSondaje = crud.listaFileList(HOLEID).ToList();

            return View();
        }

        public ActionResult ReAdminDocumentoSondajesAdd(String HOLEID) {
            crud = new CRUDDocumentos();
            ViewBag.MenuHeadSondaje = crud.listaFileListCategory(HOLEID).ToList();

            ViewBag.MenuItemSondaje = crud.listaFileList(HOLEID).ToList();
            return View();
        }

        [HttpPost]
        public String CreateCRUDPDF(String HOLEID, String PROJECT, String ID, HttpPostedFileBase ARCHIVO)
        {

            if (ARCHIVO != null && ARCHIVO.ContentLength > 0)
            {

                using (var reader = new System.IO.BinaryReader(ARCHIVO.InputStream))
                {
                    Byte[] FileDet = reader.ReadBytes(ARCHIVO.ContentLength);

                    //int vid = int.Parse(ID);
                    SqlConnection cn = new SqlConnection("Data Source=(local);Initial Catalog=DocumentacionPrueba;Integrated Security=True");
                    cn.Open();
                    SqlCommand com = new SqlCommand("INSERT INTO FILEDETAILS(HOLEID,PROJECTCODE, PRIORITY,NAME,VALUE)VALUES ('" + HOLEID + "','" + PROJECT + "','1','" + ID + "', @Name)", cn);
                    com.Parameters.Add(new SqlParameter("Name", FileDet));
                    int res = com.ExecuteNonQuery();
                    ViewBag.Mensaje = "ok";
                    return ViewBag.Mensaje;
                }



            }
            else
            {

                ViewBag.Mensaje = "Invalid file format.";
                return ViewBag.Mensaje;
            }

        }

        [HttpGet]
        public ActionResult eliminarPDF(string id, string name)
        {


            vholeid = id;
            ViewBag.dataHoleid = vholeid;

            SqlConnection cn = new SqlConnection("Data Source=(local);Initial Catalog=DocumentacionPrueba;Integrated Security=True");
            cn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM [DocumentacionPrueba].[dbo].[FILEDETAILS] WHERE  HOLEID = '" + id + "' and NAME = '" + name + "'", cn);

            int res = cmd.ExecuteNonQuery();

            return View();
        }

        public ActionResult VisualizadorPDF(string idsub, string holeid)
        {

            vid = idsub;
            vholeid = holeid;
            ViewBag.dataHoleid = vholeid;

            return View();
        }
        // ################################## DOCUMENTOS FOTOGRAFIA
        public ActionResult Fotografia()
        {

            crud = new CRUDDocumentos();

            var listFY = crud.listaSondaje().OrderBy(z => z.FISCAL_YEAR).Select(x => x.FISCAL_YEAR).Distinct().ToList();
            ViewBag.Fiscal_Year = listFY.Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();

            return View();
        }

        public ActionResult FotografiaDetalle(String FISCAL_YEAR, String PROJECTCODE, String HOLEID)
        {

            crud = new CRUDDocumentos();

            ViewBag.MenuItemSondaje = crud.listaFileListFOTO(HOLEID).ToArray().ToList();

            return View();
        }
        public ActionResult VisualizadorIMG(string idsub, string holeid)
        {

            vid = idsub;
            vholeid = holeid;
            ViewBag.dataHoleid = vholeid;
            ViewBag.dataFrom = idsub;
   

            return View();
        }

        public ActionResult AdminFotografia()
        {

            return View();
        }

        // ################################## DOCUMENTOS PDF Y FOTOGRAFIA
        public ActionResult verPDF(string dataHoleid, string dataFrom)
        {


            SqlConnection con = new SqlConnection("Data Source=(local);Initial Catalog=DocumentacionPrueba;Integrated Security=True");

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            float n;
            bool isNumeric = float.TryParse(vid, out n);
          
            cmd.CommandText = isNumeric == true ? @"SELECT VALUE FROM [IMAGEDETAILS] WHERE [GEOLFROM] =" + dataFrom + " AND HOLEID='" + dataHoleid + "'" : @"SELECT VALUE FROM FILEDETAILS WHERE NAME ='" + vid + "' AND HOLEID='" + vholeid + "'";
            var app = isNumeric == true ? "image/jpg" : "application/pdf";
            ViewBag.AppType = app;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return File((byte[])dr["VALUE"], app);
            }
            else {
                return null;
            }
           
        }
      

        public ActionResult Petrografia() {
            crud = new CRUDDocumentos();

            ViewBag.SampleID = crud.listaPetrografia().Select(x => new SelectListItem { Value = x.SAMPLEID.ToString(), Text = x.SAMPLEID.ToString() }).Distinct().ToList();
            return View();
        }

       static String filePathPetrografia;
        public ActionResult PetrografiaPDF(string HOLEID_ME, string SAMPLEID, string tipoarchivo)
        {
         
            if (tipoarchivo == "CARTILLA")
            {
                filePathPetrografia = "~/Content/files/petrografia/" + HOLEID_ME + ".pdf";
                
            }
            else if (tipoarchivo == "MAPEO")
            {
                filePathPetrografia = "~/Content/files/mapeo/" + HOLEID_ME + ".pdf";

            }
            else if (tipoarchivo == "COLLAR")
            {
                filePathPetrografia = "~/Content/files/collar/" + HOLEID_ME + ".pdf";

            }
            else
            {
                filePathPetrografia = "~/Content/files/microscopica/" + HOLEID_ME + ".pdf";
             
            }

            return View();
        }

        public ActionResult PetrografiaVerPDF()
        {

            Boolean path = System.IO.File.Exists(Server.MapPath(filePathPetrografia));

       
            if (path) {
                return File(filePathPetrografia, "application/pdf");

            }
            else { 
                return File("~/Content/files/no.pdf", "application/pdf");
            }
         
        }


        //############################################### DOCUMENTOS PDF - FOTOGRAFIA - PETROGRAFIA

        public JsonResult GetProjecto(string id)
        {

            crud = new CRUDDocumentos();
            FISCALYEARGLOBAL = int.Parse(id);
            List<SelectListItem> proyectosDDL = new List<SelectListItem>();


            proyectosDDL = crud.listaSondaje().Where(x => x.FISCAL_YEAR == FISCALYEARGLOBAL).Select(x => x.PROJECTCODE.ToString()).Distinct().ToList().Select(x => new SelectListItem { Value = x, Text = x }).ToList();

            return Json(proyectosDDL, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSondaje(string projectid)
        {
            crud = new CRUDDocumentos();
         
            List<SelectListItem> sondajesDDL = new List<SelectListItem>();
  
            sondajesDDL = crud.listaSondaje().Where(x => x.FISCAL_YEAR == FISCALYEARGLOBAL && x.PROJECTCODE.Equals(projectid)).Select(x => x.HOLEID.ToString()).Distinct().ToList().Select(x => new SelectListItem { Value = x, Text = x }).ToList();

            return Json(sondajesDDL, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSondajeImg(string projectid)
        {
            crud = new CRUDDocumentos();
            List<SelectListItem> sondajesDDL = new List<SelectListItem>();
            var data = crud.listaSondaje();
            sondajesDDL = crud.listaSondaje().Where(x => x.FISCAL_YEAR== FISCALYEARGLOBAL && x.PROJECTCODE.Equals(projectid) && x.DH_DRILLTYPE_LIST.Contains('D')).Select(x => x.HOLEID.ToString()).Distinct().ToList().Select(x => new SelectListItem { Value = x, Text = x }).ToList();

            return Json(sondajesDDL, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSondPetro(string id)
        {

            crud = new CRUDDocumentos();

            List<SelectListItem> sondpetroDDL = new List<SelectListItem>();


            sondpetroDDL = crud.listaPetrografia().Where(x => x.SAMPLEID.Equals(id)).Select(x => x.HOLEID_ME.ToString()).Distinct().ToList().Select(x => new SelectListItem { Value = x, Text = x }).ToList();

            return Json(sondpetroDDL, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getDepartment()
        {
            crud = new CRUDDocumentos();
            return Json( crud.listaSubCategority(vid).Select(x => new { ID = x.DESCRIPTION, DESCRIPTION = x.DESCRIPTION}).ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}
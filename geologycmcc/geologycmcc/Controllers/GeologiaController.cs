using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using geologycmcc.Controllers.Crud;

namespace geologycmcc.Controllers
{
    public class GeologiaController : Controller
    {
        private CRUDGeologia crud;
        // GET: Geologia
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ControlSTD() {
            crud = new CRUDGeologia();

            var data = crud.listaControlStockSOND();
            return View();
        }
    }
}
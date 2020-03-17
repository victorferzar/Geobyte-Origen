using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace geologycmcc.Controllers
{
    public class QaQcController : Controller
    {
        [Authorize(Roles = @"Americas\chachl9,Americas\verac9,Americas\sancjc,Americas\bustce,AMERICAS\estacd,
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
AMERICAS\matih")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
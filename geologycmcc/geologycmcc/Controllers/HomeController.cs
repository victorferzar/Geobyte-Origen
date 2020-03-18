using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using geologycmcc.Controllers.SeguridadRoles;
using System.Data.SqlClient;
using System.Globalization;
using geologycmcc.Models.GeologiaQAQCModels;

namespace geologycmcc.Controllers
{

    [Authorize(Roles = @"Americas\chachl9,Americas\verarc9,Americas\danicda9,Americas\sancjc9,Americas\sancjc,Americas\bustce,Americas\becec,Americas\cespjl,Americas\escaj,Americas\araype2,Americas\bustv9,Americas\cistee,Americas\cifupg,Americas\gonzcc,Americas\mulej,Americas\malejf,AMERICAS\mereg,
AMERICAS\estacd,
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
AMERICAS\guzmja,
AMERICAS\moracb,
AMERICAS\BUSTCE,
AMERICAS\yanen9,
AUSTEM_BEAST1\Austen
")]
    public class HomeController : Controller
    {
        private RolData rd;
        DM_CC_SONEntities ddb;
        public string connectionString = "data source=(local);initial catalog=DM_CC_SON;integrated security=True;MultipleActiveResultSets=True;";
        public HomeController() {
            rd = new RolData();

        }


        public ActionResult Index()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            ViewBag.id = id.Name;
            ViewBag.name = User.Identity.Name;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    
        public ActionResult Denegado()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult ElemntListStd(string desde, string hasta, string std, string priority, string suite, string lab)
        {

            string queryString = string.IsNullOrEmpty(priority) ? "SELECT distinct ASSAYNAME FROM DTM_QAQC_BLK_STD  where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and STANDARDID  in ({0}) and ANALYSISSUITE  in ({1}) and LABCODE  in ({2})" : "SELECT distinct ASSAYNAME FROM DTM_QAQC_BLK_STD  where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and STANDARDID  in ({0}) and ANALYSISSUITE  in ({1}) and LABCODE  in ({2}) and PRIORITY_OR = 1 ";
            List<String> CheckList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                std = std.Replace("[", "").Replace("]", "").Replace("\"", "");
                suite = suite.Replace("[", "").Replace("]", "").Replace("\"", "");
                lab = lab.Replace("[", "").Replace("]", "").Replace("\"", "");
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                List<string> StList = std.Split(',').Select(x => x).ToList();
                List<string> SList = suite.Split(',').Select(x => x).ToList();
                List<string> Lablist = lab.Split(',').Select(x => x).ToList();
                var nameParameter = new List<string>();
                var nameParameter1 = new List<string>();
                var nameParameter2 = new List<string>();

                var index = 0; // Reset the index
                foreach (var name in StList)
                {
                    var paramName = "@nameParam" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in SList)
                {
                    var paramName = "@nameParam1" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter1.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in Lablist)
                {
                    var paramName = "@nameParam2" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter2.Add(paramName);
                    index++;
                }

                command.CommandText = String.Format(queryString, string.Join(",", nameParameter), string.Join(",", nameParameter1), string.Join(",", nameParameter2));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        CheckList.Add(reader["ASSAYNAME"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }


            }
            return Json(CheckList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult elementosList(string suite)
        {


            //System.Diagnostics.Debug.WriteLine(suite.ToList());
            using (ddb = new DM_CC_SONEntities())
            {
                IOrderedEnumerable<DTM_SUITEDEFINITION> elementySuite;
                elementySuite = ddb.DTM_SUITEDEFINITION.ToList().OrderBy(x => x.ANALYSISSUITE);
                List<String> elementList = new List<string>();
                foreach (var item in elementySuite.Where(x => suite.Contains(x.ANALYSISSUITE)).Select(x => x.NAME).Distinct().ToList())
                {
                    elementList.Add(item.ToString());
                }

                return Json(elementList, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult StdList(string desde, string hasta, string tipo, string lab, string suite)
        {
            string queryString = "SELECT distinct STANDARDID FROM DTM_QAQC_BLK_STD where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and STANDARDID LIKE @tipo  and LABCODE in ({0})  and ANALYSISSUITE in ({1})";



            List<String> StandardList = new List<string>();
            lab = lab.Replace("[", "").Replace("]", "").Replace("\"", "");
            suite = suite.Replace("[", "").Replace("]", "").Replace("\"", "");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@tipo", "%" + tipo + "%");
                List<string> labList = lab.Split(',').Select(x => x).ToList();
                var nameParameter = new List<string>();
                List<string> SuiteList = suite.Split(',').Select(x => x).ToList();
                var nameParameter1 = new List<string>();
                var index = 0; // Reset the index
                foreach (var name in labList)
                {
                    var paramName = "@nameParam" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter.Add(paramName);
                    index++;
                }
                foreach (var name in SuiteList)
                {
                    var paramName = "@nameParam1" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter1.Add(paramName);
                    index++;
                }

                command.CommandText = String.Format(queryString, string.Join(",", nameParameter), string.Join(",", nameParameter1));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {

                        StandardList.Add(reader["STANDARDID"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }


            }
            return Json(StandardList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult LabList(string desde, string hasta, string tipo)
        {
            string queryString = "SELECT distinct LABCODE FROM DTM_QAQC_BLK_STD where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and STANDARDID LIKE @tipo";



            List<String> LabList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                command.Parameters.AddWithValue("@tipo", "%" + tipo + "%");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {

                        LabList.Add(reader["LABCODE"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }


            }
            return Json(LabList, JsonRequestBehavior.AllowGet);

        }


        public JsonResult StdSuite(string desde, string hasta, string lab, string tipo)
        {
            string queryString = "SELECT distinct ANALYSISSUITE FROM DTM_QAQC_BLK_STD where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) AND STANDARDID LIKE @tipo and LABCODE in ({0})";



            List<String> StdSuite = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                lab = lab.Replace("[", "").Replace("]", "").Replace("\"", "");
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@tipo", "%" + tipo + "%");

                List<string> labList = lab.Split(',').Select(x => x).ToList();
                var nameParameter = new List<string>();
                var index = 0; // Reset the index
                foreach (var name in labList)
                {
                    var paramName = "@nameParam" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter.Add(paramName);
                    index++;
                }

                command.CommandText = String.Format(queryString, string.Join(",", nameParameter));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {

                        StdSuite.Add(reader["ANALYSISSUITE"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }


            }
            return Json(StdSuite, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ProjectList(string desde, string hasta)
        {
            string queryString = "SELECT distinct PROJECTCODE FROM DTM_QAQC_DUP where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date)";



            List<String> ProjectList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        ProjectList.Add(reader["PROJECTCODE"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }


            }
            return Json(ProjectList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DrillTypeList(string desde, string hasta, string project, string holestatus)
        {
            string queryString = "SELECT distinct SAMPLE_DRILLTYPE FROM DTM_QAQC_DUP D INNER JOIN DTM_COLLAR C ON D.HOLEID=C.HOLEID where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and C.PROJECTCODE  in ({0}) and STATUS  in ({1}) ";
            List<String> DrillTypeList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                project = project.Replace("[", "").Replace("]", "").Replace("\"", "");
                holestatus = holestatus.Replace("[", "").Replace("]", "").Replace("\"", "");
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                List<string> PList = project.Split(',').Select(x => x).ToList();
                List<string> HSList = holestatus.Split(',').Select(x => x).ToList();
                var nameParameter = new List<string>();
                var nameParameter1 = new List<string>();
                var index = 0; // Reset the index
                foreach (var name in PList)
                {
                    var paramName = "@nameParam" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in HSList)
                {
                    var paramName = "@nameParam1" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter1.Add(paramName);
                    index++;
                }

                command.CommandText = String.Format(queryString, string.Join(",", nameParameter), string.Join(",", nameParameter1));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {

                        DrillTypeList.Add(reader["SAMPLE_DRILLTYPE"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }


            }
            return Json(DrillTypeList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult HoleStatusList(string desde, string hasta, string project)
        {
            string queryString = "SELECT distinct STATUS FROM DTM_COLLAR C INNER JOIN  DTM_QAQC_DUP D  ON D.HOLEID=C.HOLEID where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) AND STATUS IN ('Terminado','Extraible','Modelable','Remapeo','Recodificacion','Parcial','Reproceso') AND C.PROJECTCODE  in ({0})";
            List<String> HoleStatusList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                project = project.Replace("[", "").Replace("]", "").Replace("\"", "");
                SqlCommand command = new SqlCommand(queryString, connection);
                List<string> PList = project.Split(',').Select(x => x).ToList();
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                var nameParameter = new List<string>();
                var index = 0; // Reset the index
                foreach (var name in PList)
                {
                    var paramName = "@nameParam" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter.Add(paramName);
                    index++;
                }

                command.CommandText = String.Format(queryString, string.Join(",", nameParameter));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {

                        HoleStatusList.Add(reader["STATUS"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                  //  reader.Close();
                }


            }
            return Json(HoleStatusList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CheckList(string desde, string hasta, string project, string check, string holestatus)
        {
            string queryString = "SELECT distinct CHECKSTAGE FROM DTM_QAQC_DUP  D INNER JOIN DTM_COLLAR C ON D.HOLEID=C.HOLEID where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and C.PROJECTCODE  in ({0})  and SAMPLE_DRILLTYPE  in ({1}) and STATUS  in ({2})";
            List<String> CheckList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                project = project.Replace("[", "").Replace("]", "").Replace("\"", "");
                check = check.Replace("[", "").Replace("]", "").Replace("\"", "");
                holestatus = holestatus.Replace("[", "").Replace("]", "").Replace("\"", "");
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                List<string> PList = project.Split(',').Select(x => x).ToList();
                List<string> CList = check.Split(',').Select(x => x).ToList();
                List<string> HList = holestatus.Split(',').Select(x => x).ToList();
                var nameParameter = new List<string>();
                var nameParameter1 = new List<string>();
                var nameParameter2 = new List<string>();
                var index = 0; // Reset the index
                foreach (var name in PList)
                {
                    var paramName = "@nameParam" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in CList)
                {
                    var paramName = "@nameParam1" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter1.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in HList)
                {
                    var paramName = "@nameParam2" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter2.Add(paramName);
                    index++;
                }

                command.CommandText = String.Format(queryString, string.Join(",", nameParameter), string.Join(",", nameParameter1), string.Join(",", nameParameter2));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        CheckList.Add(reader["CHECKSTAGE"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }


            }
            return Json(CheckList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SuiteList(string desde, string hasta, string project, string drill, string check, string holestatus, string priority, string labdup)
        {

            string queryString = string.IsNullOrEmpty(priority) ? "SELECT distinct ANALYSISSUITE FROM DTM_QAQC_DUP D INNER JOIN DTM_COLLAR C ON D.HOLEID=C.HOLEID where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and C.PROJECTCODE  in ({0})  and SAMPLE_DRILLTYPE  in ({1}) and CHECKSTAGE  in ({2}) and STATUS  in ({3}) and LABCODE  in ({4})" : "SELECT distinct ANALYSISSUITE FROM DTM_QAQC_DUP D INNER JOIN DTM_COLLAR C ON D.HOLEID=C.HOLEID where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and C.PROJECTCODE  in ({0})  and SAMPLE_DRILLTYPE  in ({1}) and CHECKSTAGE  in ({2}) and STATUS  in ({3}) and LABCODE  in ({4}) and PRIORITY_OR = 1 ";
            List<String> CheckList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                project = project.Replace("[", "").Replace("]", "").Replace("\"", "");
                drill = drill.Replace("[", "").Replace("]", "").Replace("\"", "");
                check = check.Replace("[", "").Replace("]", "").Replace("\"", "");
                holestatus = holestatus.Replace("[", "").Replace("]", "").Replace("\"", "");
                labdup = labdup.Replace("[", "").Replace("]", "").Replace("\"", "");
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                List<string> PList = project.Split(',').Select(x => x).ToList();
                List<string> DList = drill.Split(',').Select(x => x).ToList();
                List<string> CList = check.Split(',').Select(x => x).ToList();
                List<string> HList = holestatus.Split(',').Select(x => x).ToList();
                List<string> LList = labdup.Split(',').Select(x => x).ToList();
                var nameParameter = new List<string>();
                var nameParameter1 = new List<string>();
                var nameParameter2 = new List<string>();
                var nameParameter3 = new List<string>();
                var nameParameter4 = new List<string>();
                var index = 0; // Reset the index
                foreach (var name in PList)
                {
                    var paramName = "@nameParam" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in DList)
                {
                    var paramName = "@nameParam1" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter1.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in CList)
                {
                    var paramName = "@nameParam2" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter2.Add(paramName);
                    index++;
                }
                foreach (var name in HList)
                {
                    var paramName = "@nameParam3" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter3.Add(paramName);
                    index++;
                }
                foreach (var name in LList)
                {
                    var paramName = "@nameParam4" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter4.Add(paramName);
                    index++;
                }
                command.CommandText = String.Format(queryString, string.Join(",", nameParameter), string.Join(",", nameParameter1), string.Join(",", nameParameter2), string.Join(",", nameParameter3), string.Join(",", nameParameter4));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        CheckList.Add(reader["ANALYSISSUITE"].ToString());
                        System.Diagnostics.Debug.WriteLine(reader["ANALYSISSUITE"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }


            }
            return Json(CheckList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult LabDup(string desde, string hasta, string project, string drill, string check, string holestatus, string priority)
        {

            string queryString = string.IsNullOrEmpty(priority) ? "SELECT distinct LABCODE FROM DTM_QAQC_DUP D INNER JOIN DTM_COLLAR C ON D.HOLEID=C.HOLEID where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and C.PROJECTCODE  in ({0})  and SAMPLE_DRILLTYPE  in ({1}) and CHECKSTAGE  in ({2}) and STATUS  in ({3})" : "SELECT distinct ANALYSISSUITE FROM DTM_QAQC_DUP D INNER JOIN DTM_COLLAR C ON D.HOLEID=C.HOLEID where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and C.PROJECTCODE  in ({0})  and SAMPLE_DRILLTYPE  in ({1}) and CHECKSTAGE  in ({2}) and STATUS  in ({3}) and PRIORITY_OR = 1 ";
            List<String> CheckList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                project = project.Replace("[", "").Replace("]", "").Replace("\"", "");
                drill = drill.Replace("[", "").Replace("]", "").Replace("\"", "");
                check = check.Replace("[", "").Replace("]", "").Replace("\"", "");
                holestatus = holestatus.Replace("[", "").Replace("]", "").Replace("\"", "");
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                List<string> PList = project.Split(',').Select(x => x).ToList();
                List<string> DList = drill.Split(',').Select(x => x).ToList();
                List<string> CList = check.Split(',').Select(x => x).ToList();
                List<string> HList = holestatus.Split(',').Select(x => x).ToList();
                var nameParameter = new List<string>();
                var nameParameter1 = new List<string>();
                var nameParameter2 = new List<string>();
                var nameParameter3 = new List<string>();
                var index = 0; // Reset the index
                foreach (var name in PList)
                {
                    var paramName = "@nameParam" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in DList)
                {
                    var paramName = "@nameParam1" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter1.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in CList)
                {
                    var paramName = "@nameParam2" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter2.Add(paramName);
                    index++;
                }
                foreach (var name in HList)
                {
                    var paramName = "@nameParam3" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter3.Add(paramName);
                    index++;
                }
                command.CommandText = String.Format(queryString, string.Join(",", nameParameter), string.Join(",", nameParameter1), string.Join(",", nameParameter2), string.Join(",", nameParameter3));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        CheckList.Add(reader["LABCODE"].ToString());
                        System.Diagnostics.Debug.WriteLine(reader["LABCODE"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }


            }
            return Json(CheckList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ElemntListDup(string desde, string hasta, string project, string drill, string check, string holestatus, string priority, string suite)
        {

            string queryString = string.IsNullOrEmpty(priority) ? "SELECT distinct ASSAYNAME FROM DTM_QAQC_DUP D INNER JOIN DTM_COLLAR C ON D.HOLEID=C.HOLEID where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and C.PROJECTCODE  in ({0})  and SAMPLE_DRILLTYPE  in ({1}) and CHECKSTAGE  in ({2}) and STATUS  in ({3}) and ANALYSISSUITE  in ({4})" : "SELECT distinct ASSAYNAME FROM DTM_QAQC_DUP D INNER JOIN DTM_COLLAR C ON D.HOLEID=C.HOLEID where RETURNDATE >=cast(@inicio as date) and RETURNDATE <cast(@fin as date) and C.PROJECTCODE  in ({0})  and SAMPLE_DRILLTYPE  in ({1}) and CHECKSTAGE  in ({2}) and STATUS  in ({3}) and ANALYSISSUITE  in ({4}) and PRIORITY_OR = 1 ";
            List<String> CheckList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                project = project.Replace("[", "").Replace("]", "").Replace("\"", "");
                drill = drill.Replace("[", "").Replace("]", "").Replace("\"", "");
                check = check.Replace("[", "").Replace("]", "").Replace("\"", "");
                holestatus = holestatus.Replace("[", "").Replace("]", "").Replace("\"", "");
                suite = suite.Replace("[", "").Replace("]", "").Replace("\"", "");
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@inicio", DateTime.ParseExact(desde, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@fin", DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                List<string> PList = project.Split(',').Select(x => x).ToList();
                List<string> DList = drill.Split(',').Select(x => x).ToList();
                List<string> CList = check.Split(',').Select(x => x).ToList();
                List<string> HList = holestatus.Split(',').Select(x => x).ToList();
                List<string> SList = suite.Split(',').Select(x => x).ToList();
                var nameParameter = new List<string>();
                var nameParameter1 = new List<string>();
                var nameParameter2 = new List<string>();
                var nameParameter3 = new List<string>();
                var nameParameter4 = new List<string>();
                var index = 0; // Reset the index
                foreach (var name in PList)
                {
                    var paramName = "@nameParam" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in DList)
                {
                    var paramName = "@nameParam1" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter1.Add(paramName);
                    index++;
                }
                index = 0; // Reset the index
                foreach (var name in CList)
                {
                    var paramName = "@nameParam2" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter2.Add(paramName);
                    index++;
                }
                index = 0;
                foreach (var name in HList)
                {
                    var paramName = "@nameParam3" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter3.Add(paramName);
                    index++;
                }
                index = 0;
                foreach (var name in SList)
                {
                    var paramName = "@nameParam4" + index;
                    command.Parameters.AddWithValue(paramName, name);
                    nameParameter4.Add(paramName);
                    index++;
                }
                command.CommandText = String.Format(queryString, string.Join(",", nameParameter), string.Join(",", nameParameter1), string.Join(",", nameParameter2), string.Join(",", nameParameter3), string.Join(",", nameParameter4));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        CheckList.Add(reader["ASSAYNAME"].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }


            }
            return Json(CheckList, JsonRequestBehavior.AllowGet);

        }



    }
}
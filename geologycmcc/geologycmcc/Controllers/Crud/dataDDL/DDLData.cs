using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Models.GeologiaQAQCModels;
using System.Web.Mvc;

namespace geologycmcc.Controllers.Crud.dataDDL
{
    public class DDLData
    {

        private DM_CC_SONEntities ddb;
        private List<SelectListItem> listNameStage;
        private List<SelectListItem> listNameAssay;
        private List<SelectListItem> listProyect;
        private List<SelectListItem> listSuites;
        private List<SelectListItem> listFullProject;
        private List<SelectListItem> listType;
        private List<SelectListItem> listStd;
        private List<SelectListItem> listStatus;
        /************************************************Graphics/Dup **************************************************/

        public DDLData()
        {
            ddb = new DM_CC_SONEntities();
        }

        //Form List 1
        public List<SelectListItem> DDLFullProyect()
        {

            listFullProject = new List<SelectListItem>();

            var dupFullProyectCode = ddb.DTM_QAQC_DUP.Select(x => x.PROJECTCODE).Distinct().ToList();

            foreach (var i in dupFullProyectCode)
                listFullProject.Add(new SelectListItem { Text = i, Value = i.ToString() });


            return listFullProject;

        }

        public List<SelectListItem> DDLDrillTypeList()
        {
            listType = new List<SelectListItem>();

            var dupDType = ddb.DTM_QAQC_DUP.Where(x => x.SAMPLE_DRILLTYPE.Length > 0).Select(x => x.SAMPLE_DRILLTYPE).Distinct().ToList();

            foreach (var i in dupDType)

                listType.Add(new SelectListItem { Text = i, Value = i.ToString() });


            return listType;

        }

        public List<SelectListItem> DDLDespatchSuites(string tipo)
        {

            listSuites = new List<SelectListItem>();
            var dupFySuites = tipo == "dup" ? ddb.DTM_QAQC_DUP.Select(x => x.ANALYSISSUITE).Distinct().ToList() : ddb.DTM_QAQC_BLK_STD.Select(x => x.ANALYSISSUITE).Distinct().ToList();

            foreach (var i in dupFySuites)
                listSuites.Add(new SelectListItem { Text = i, Value = i.ToString() });

            return listSuites;

        }

        public List<SelectListItem> DDLNameStage()
        {

            listNameStage = new List<SelectListItem>();

            var items = (ddb.DTM_QAQC_DUP.Select(x => x.CHECKSTAGE).Distinct()).ToList();

            items.Remove(null);

            foreach (var i in items)
                listNameStage.Add(new SelectListItem { Text = i, Value = i });

            return listNameStage;

        }

        public List<SelectListItem> DDLNameAssay(String id)
        {

            listNameAssay = new List<SelectListItem>();



            var results = ddb.DTM_SUITEDEFINITION.Select(x => x.ANALYSISSUITE).Distinct().ToList();

            foreach (var i in results)
                listNameAssay.Add(new SelectListItem { Text = i, Value = i.ToString() });


            return listNameAssay;

        }



        public List<SelectListItem> DDLListStatus()
        {

            listStatus = new List<SelectListItem>();

            listStatus.Add(new SelectListItem { Text = "Todos", Value = "", Selected = true });
            listStatus.Add(new SelectListItem { Text = "Aprob", Value = "1" });
            listStatus.Add(new SelectListItem { Text = "Pend", Value = "2" });
            listStatus.Add(new SelectListItem { Text = "Rech", Value = "3" });
            return listStatus;

        }
        public List<SelectListItem> DDLListSTD(string tipo)
        {

            listStd = new List<SelectListItem>();

            var items = (ddb.DTM_QAQC_BLK_STD.Where(x => x.STANDARDID.Substring(0, 1) == tipo).Select(x => x.STANDARDID).Distinct()).ToList();

            items.Remove(null);

            foreach (var i in items)
                listStd.Add(new SelectListItem { Text = i, Value = i });

            return listStd;

        }


        public List<SelectListItem> DDLProyectos()
        {

            listProyect = new List<SelectListItem>();

            var datosCollar = (ddb.DTM_COLLAR.Where(x => x.HOLEID.StartsWith("RC-18"))).ToList();

            foreach (var i in datosCollar)
                listProyect.Add(new SelectListItem { Text = i.HOLEID, Value = i.HOLEID });

            return listProyect;

        }

    }
}
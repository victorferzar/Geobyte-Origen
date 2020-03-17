using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace geologycmcc.Controllers.DataBase
{
    public class Conexion
    {
        private SqlConnection cn;
        public SqlConnection openDB()
        {
            this.cn = new SqlConnection("data source = cmc01663; initial catalog = ACQ_BHPB_CC_SON_LOCAL; integrated security = True; MultipleActiveResultSets = True;");
            this.cn.Open();
            return this.cn;
        }

        public SqlConnection openDB2()
        {
            this.cn = new SqlConnection("data source = iqqccm-vacq01; initial catalog = ACQ_BHPB_CC_BAN; integrated security = True; MultipleActiveResultSets = True;");
            this.cn.Open();
            return this.cn;
        }



        public string GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Data Source"] = "iqqccm-vacq01";
            builder["integrated Security"] = true;
            builder["Initial Catalog"] = "ACQ_BHPB_CC_SON";
           
            return builder.ConnectionString;
        }

        public string GetConnectionDM_SON_String()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Data Source"] = "cmc01663";
            builder["integrated Security"] = true;
            builder["Initial Catalog"] = "DM_CC_SON";
            return builder.ConnectionString;
        }

        public string GetConnectionDocumentacionPrueba()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Data Source"] = "cmc01663";
            builder["integrated Security"] = true;
            builder["Initial Catalog"] = "DocumentacionPrueba";
            return builder.ConnectionString;
        }

        public void closeDB()
        {
            this.cn.Dispose();
            this.cn.Close();

        }

        public string GetConnectionGeotecnia()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Data Source"] = "IQQCCM-VACQ01";
            builder["integrated Security"] = true;
            builder["Initial Catalog"] = "ACQ_BHPB_CC_BAN";

            return builder.ConnectionString;
        }


        public string GetConnectionBDSON()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Data Source"] = "IQQCCM-VACQ01";
            builder["integrated Security"] = true;
            builder["Initial Catalog"] = "ACQ_BHPB_CC_SON";

            return builder.ConnectionString;
        }


        public string GetConnectionBDPROD()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Data Source"] = "IQQCCM-VACQ01";
            builder["integrated Security"] = true;
            builder["Initial Catalog"] = "ACQ_BHPB_CC_PROD";

            return builder.ConnectionString;
        }


        public string GetConnectionBDGM()
        {
            SqlConnection cn = new SqlConnection("data source = iqqccm-vacq01; initial catalog = ACQ_BHPB_CC_GM; integrated security = True; MultipleActiveResultSets = True;");

            return cn.ConnectionString;
        }
    }
}
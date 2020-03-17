using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Models.DocumentosModels;
using System.Data.Linq;

namespace geologycmcc.Controllers.DataBase
{
    public class TransaccionesDocumentos
    {

        private Conexion cnLocalBD;

        public IEnumerable<DocumentsDrillHole> dataListaSondaje(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionString());

            IEnumerable<DocumentsDrillHole> resumen = context.ExecuteQuery<DocumentsDrillHole>(data).ToList();
            context.Dispose();
            return resumen;
        }


        public IEnumerable<DocumentFileListSubCategory> datalistaSubCategority(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionDocumentacionPrueba());

            IEnumerable<DocumentFileListSubCategory> resumen = context.ExecuteQuery<DocumentFileListSubCategory>(data).ToList();
            context.Dispose();
            return resumen;
        }


        public IEnumerable<DocumentFileList> dataFileList(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionDocumentacionPrueba());

            IEnumerable<DocumentFileList> resumen = context.ExecuteQuery<DocumentFileList>(data).ToList();
            context.Dispose();
            return resumen;
        }
        public IEnumerable<DocumentListFoto> dataFileListFoto(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionDocumentacionPrueba());

            IEnumerable<DocumentListFoto> resumen = context.ExecuteQuery<DocumentListFoto>(data).ToList();
            context.Dispose();
            return resumen;
        }
       
        public IEnumerable<DocumentFileListCategory> dataFileListCategory(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionDocumentacionPrueba());

            IEnumerable<DocumentFileListCategory> resumen = context.ExecuteQuery<DocumentFileListCategory>(data).ToList();
            context.Dispose();
            return resumen;
        }

        public IEnumerable<DocumentSondPetrografia> dataListPetrografia(String data)
        {
            cnLocalBD = new Conexion();

            DataContext context = new DataContext(cnLocalBD.GetConnectionString());

            IEnumerable<DocumentSondPetrografia> resumen = context.ExecuteQuery<DocumentSondPetrografia>(data).ToList();
            context.Dispose();
            return resumen;
        }
       

    }
}
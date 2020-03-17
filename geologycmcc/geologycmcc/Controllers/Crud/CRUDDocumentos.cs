using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Controllers.DataBase;
using geologycmcc.Models.DocumentosModels;

namespace geologycmcc.Controllers.Crud
{
    public class CRUDDocumentos
    {
        TransaccionesDocumentos tr;


        //View INDEX
        public IEnumerable<DocumentsDrillHole> listaSondaje()
        {
            tr = new TransaccionesDocumentos();
            IEnumerable<DocumentsDrillHole> resultado = tr.dataListaSondaje("SELECT PROJECTCODE, HOLEID, FISCAL_YEAR, STATUS, DH_DRILLTYPE_LIST FROM COLLAR WHERE(FISCAL_YEAR IS NOT NULL) AND(STATUS IN('Modelable', 'Extraible', 'Remapeo', 'Recodificacion', 'Terminado')) AND(DH_DRILLTYPE_LIST IS NOT NULL)");

            return resultado;
        }

        public IEnumerable<DocumentFileListSubCategory> listaSubCategority(String HOLEID)
        {
            tr = new TransaccionesDocumentos();
            IEnumerable<DocumentFileListSubCategory> resultado = tr.datalistaSubCategority("SELECT NAME, DESCRIPTION,PRIMARYCODE FROM [DocumentacionPrueba].[dbo].[FILECODESECONDARY] WHERE [NAME] NOT IN (SELECT [NAME] FROM [DocumentacionPrueba].[dbo].[FILEDETAILS] WHERE HOLEID='" + HOLEID + "')");

            return resultado;
        }

        public IEnumerable<DocumentFileList> listaFileList(String HOLEID)
        {
            tr = new TransaccionesDocumentos();
            
            IEnumerable<DocumentFileList> resultado = tr.dataFileList("SELECT DFH.HOLEID, DFS.NAME, DFC.PRIMARYCODE FROM[DocumentacionPrueba].[dbo].[FILEDETAILS]" +
            "DFH INNER JOIN[DocumentacionPrueba].[dbo].[FILECODESECONDARY]" +
            "DFS ON DFS.NAME = DFH.NAME and DFH.HOLEID = '" + HOLEID + "' INNER JOIN[DocumentacionPrueba].[dbo].[FILECODEPRIMARY]" +
            "DFC ON DFC.PRIMARYCODE = DFS.PRIMARYCODE");

            return resultado;
        }

        public IEnumerable<DocumentFileListCategory> listaFileListCategory(String HOLEID)
        {
            tr = new TransaccionesDocumentos();
            IEnumerable<DocumentFileListCategory> resultado = tr.dataFileListCategory("SELECT DISTINCT(DFC.PRIMARYCODE), DFC.DESCRIPTION FROM[DocumentacionPrueba].[dbo].[FILEDETAILS] DFH INNER JOIN[DocumentacionPrueba].[dbo].[FILECODESECONDARY] DFS ON DFS.NAME = DFH.NAME and DFH.HOLEID = '" + HOLEID + "' INNER JOIN[DocumentacionPrueba].[dbo].[FILECODEPRIMARY] DFC ON DFC.PRIMARYCODE = DFS.PRIMARYCODE");

            return resultado;
        }
        public IEnumerable<DocumentListFoto> listaFileListFOTO(String HOLEID)
        {
            tr = new TransaccionesDocumentos();
            IEnumerable<DocumentListFoto> resultado = tr.dataFileListFoto("SELECT HOLEID,GEOLFROM,GEOLTO FROM IMAGEDETAILS where HOLEID = '" + HOLEID+"'");

            return resultado;
        }

        public IEnumerable<DocumentSondPetrografia> listaPetrografia()
        {
            tr = new TransaccionesDocumentos();
            IEnumerable<DocumentSondPetrografia> resultado = tr.dataListPetrografia("SELECT SAMPLEID, HOLEID_ME FROM(SELECT SAMPLEID, NAME, VALUE FROM SAMPLEDETAILS WHERE NAME IN( 'DESCMACROME','HOLEID_ME'))S pivot(max(VALUE) FOR NAME IN([DESCMACROME], [HOLEID_ME]) ) AS PVT WHERE PVT.DESCMACROME=1");

            return resultado;
        }
        

    }
}
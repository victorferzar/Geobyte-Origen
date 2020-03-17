using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace geologycmcc.Helpers
{
    public class JsonNetResult
    {
        public new object Data { get; set; }
        public JsonRequestBehavior JsonRequestBehavior { get; internal set; }

        public JsonNetResult()
        {

        }

        //public override void ExecuteResult(ControllerContext context)
        //{
        //    HttpResponseBase response = context.HttpContext.Response;
        //    response.ContentType = "application/json";

        //    if (ContentEncoding != null)
        //    {
        //        response.ContentEncoding = ContentEncoding;
        //    }

        //    if (Data != null)
        //    {
        //        JsonTextWriter writer = new JsonTextWriter(response.Output);
        //        writer.Formatting = Formatting.Indented;
        //        JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings());
        //        serializer.Serialize(writer, Data);
        //        writer.Flush();
        //    }
        //}
    }
}
using OnlineStore.Lib_Primavera.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineStore.Controllers
{
    public class BillingPDFController : ApiController
    {

        public PrintDoc Get(string id, [FromUri]string dest_path, [FromUri]bool duplicate = false)
        {
            Lib_Primavera.Model.PrintDoc pdf;
            try { pdf = Lib_Primavera.PriIntegration.PrintDocument(id, duplicate, dest_path); }
            catch (Exception ex)
            {


                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.BadRequest, Lib_Primavera.Util.ErrorException(ex).GetObject()));
            }

            if (pdf == null)
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            else return pdf;

        }
    }
}

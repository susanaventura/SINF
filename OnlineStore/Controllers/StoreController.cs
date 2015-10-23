using OnlineStore.Lib_Primavera.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineStore.Controllers
{
    public class StoreController : ApiController
    {
        // GET api/store/5    
        public Store Get(string id)
        {
            Lib_Primavera.Model.Store product = Lib_Primavera.PriIntegration.
            if (product == null)
            {
                return null;
                /* throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));*/
            }
            else { return product; }
        }
    }
}

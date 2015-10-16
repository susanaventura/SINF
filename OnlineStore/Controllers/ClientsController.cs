using OnlineStore.Lib_Primavera.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace OnlineStore.Controllers
{
    public class ClientsController : ApiController
    {
        public Client Get(string id)
        {
            Lib_Primavera.Model.Client cliente = Lib_Primavera.PriIntegration.GetClient(id);
            if (cliente == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return cliente;
            }
        }


    }
}
using OnlineStore.Lib_Primavera.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Net.Http;


namespace OnlineStore.Controllers
{
    public class ClientsController : ApiController
    {

        // GET api/cliente/5    
        public Client Get(string id)
        {
            Lib_Primavera.Model.Client client = Lib_Primavera.PriIntegration.GetClient(id);
            if (client == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return client;
            }
        }



        public HttpResponseMessage Post(Lib_Primavera.Model.Client client)
        {
            Lib_Primavera.Model.ErrorResponse erro = new Lib_Primavera.Model.ErrorResponse();
            erro = Lib_Primavera.PriIntegration.CreateClient(client);

            if (erro.Error == 0)
            {
                var response = Request.CreateResponse(
                   HttpStatusCode.Created, client);
                string uri = Url.Link("DefaultApi", new { CodCliente = client.codClient });
                response.Headers.Location = new Uri(uri);
                return response;
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }




    }
}
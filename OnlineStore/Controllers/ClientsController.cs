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


            Lib_Primavera.Model.ErrorResponse error = new Lib_Primavera.Model.ErrorResponse();
            Lib_Primavera.Model.Client client = Lib_Primavera.PriIntegration.GetClient(id);
            return client;
        }


        
        public HttpResponseMessage Post(Lib_Primavera.Model.Client client)
        {
            Lib_Primavera.Model.ErrorResponse erro = new Lib_Primavera.Model.ErrorResponse();
            erro = Lib_Primavera.PriIntegration.CreateClient(client);

            if (erro.Error == 0)
            {
                var response = Request.CreateResponse(
                   HttpStatusCode.Created, client);
                string uri = Url.Link("DefaultApi", new { CodCliente = client.CodClient });
                response.Headers.Location = new Uri(uri);
                return response;
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        public HttpResponseMessage Put(Lib_Primavera.Model.Client client)
        {

            Lib_Primavera.Model.ErrorResponse error = new Lib_Primavera.Model.ErrorResponse();

            try
            {
                error = Lib_Primavera.PriIntegration.UpdCliente(client);
                if (error.Error == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, error.Description);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, error.Description);
                }
            }

            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.Description);
            }
        }




        public HttpResponseMessage Delete(string id)
        {


            Lib_Primavera.Model.ErrorResponse error = new Lib_Primavera.Model.ErrorResponse();

            try
            {

                error = Lib_Primavera.PriIntegration.DelCliente(id);

                if (error.Error == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, error.Description);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, error.Description);
                }

            }

            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.Description);

            }

        }



    }
}
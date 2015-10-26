﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineStore.Controllers
{
    public class OrdersController : ApiController
    {
        // GET /orders(?codClient=1)
        public IEnumerable<Lib_Primavera.Model.Order> Get([FromUri]string codClient = "", [FromUri]string aux = "")
        {
            return Lib_Primavera.PriIntegration.ListOrders();
        }

        public Lib_Primavera.Model.Order Get(string codOrder)
        {
            return null;
            /*Lib_Primavera.Model.Order order = Lib_Primavera.PriIntegration.Encomenda_Get(id);
            if (docvenda == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return docvenda;
            }*/
        }


        // POST /orders
        public HttpResponseMessage Post(Lib_Primavera.Model.Order order) {
            Lib_Primavera.Model.ErrorResponse error = Lib_Primavera.PriIntegration.NewOrder(order);
            if (error.Error == 0) {
                var response = Request.CreateResponse(HttpStatusCode.Created, order.CodOrder);
                string uri = Url.Link("DefaultApi", new { DocId = order.CodOrder });
                response.Headers.Location = new Uri(uri);
                return response;
            } else {
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.Description);
            }
        }

    }
}

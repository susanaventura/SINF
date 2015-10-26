using System;
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
        public IEnumerable<Lib_Primavera.Model.Order> Get([FromUri]string codClient = "") { 
            // TODO
            return null;
        }

        public Lib_Primavera.Model.Order Get(string codOrder)
        {
            // TODO
            return null;
        }


        // POST /orders
        public HttpResponseMessage Post(Lib_Primavera.Model.Order client) { 
            // TODO
            return null;
        }

    }
}

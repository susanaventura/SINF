using OnlineStore.Lib_Primavera.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineStore.Controllers
{
    public class StoresController : ApiController
    {
        // GET api/store/5    
        public Store Get(string id)
        {
            Lib_Primavera.Model.Store store = Lib_Primavera.PriIntegration.getStore(id);
            return store;
        }

        //GET api/store
        public IEnumerable<Lib_Primavera.Model.Store> Get()
        {
            return Lib_Primavera.PriIntegration.listStores();
        }
    }
}

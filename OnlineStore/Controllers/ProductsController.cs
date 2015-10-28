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
    public class ProductsController : ApiController
    {
        // GET api/products/5    
        public Product Get(string id)
        {
            Lib_Primavera.Model.Product product = Lib_Primavera.PriIntegration.GetProduct(id);
            if (product == null)
            {
                return null;
                /* throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));*/
            }
            else { return product; }
        }

        // GET api/products
        public const int productPageSize = 50;

        public IEnumerable<Lib_Primavera.Model.Product> Get([FromUri]string page = "", [FromUri]string codStore = "", [FromUri]string codCategory = "",
                [FromUri]string filterNew = "", [FromUri]string filterRecent = "", [FromUri]string filterOnSale = "", [FromUri]string filterPoints = "")
        {
            int indexStart;
            try { indexStart = (int.Parse(page)-1) * productPageSize; } catch (Exception) { indexStart = 0; }

            return Lib_Primavera.PriIntegration.ListProducts(indexStart, productPageSize);
        }

        

    }
}

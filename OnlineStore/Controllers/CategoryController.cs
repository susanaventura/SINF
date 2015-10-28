using OnlineStore.Lib_Primavera.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OnlineStore.Controllers
{
    public class CategoryController : ApiController
    {
        public IEnumerable<Lib_Primavera.Model.Category> Get([FromUri]string page = "", [FromUri]string codStore = "", [FromUri]string codCategory = "",
                [FromUri]string filterNew = "", [FromUri]string filterRecent = "", [FromUri]string filterOnSale = "", [FromUri]string filterPoints = "")
        {
            return Lib_Primavera.PriIntegration.Categories_List();
        }
        
    }
}
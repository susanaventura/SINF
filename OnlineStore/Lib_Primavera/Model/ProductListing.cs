using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class ProductListing
    {

        public List<Model.Product> products { get; set; }
        public int pageSize { get; set; }
        public int numResults { get; set; }
    }
}
﻿using OnlineStore.Lib_Primavera.Model;
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

        public ProductListing Get([FromUri]string page = "1", [FromUri]string pageLength = "30", [FromUri]string codStore = "", [FromUri]string codCategory = "",
                [FromUri]string filterOnSale = "", [FromUri]string filterPoints = "", [FromUri]string searchString = "", [FromUri]string sort = "")
        {
            int indexStart, pageSize; Product.QueryParams.SortType sortType;
            try { pageSize = Math.Min(Math.Max(0, int.Parse(pageLength)), 100); } catch (Exception) { pageSize = 30; }
            try { indexStart = (int.Parse(page) - 1) * pageSize; } catch (Exception) { indexStart = 0; }
            switch (sort.ToUpper())
            {
                case "LASTSOLD": sortType = Product.QueryParams.SortType.LAST_SOLD; break;
                case "PRICELOWEST": sortType = Product.QueryParams.SortType.PRICE_LOWEST; break;
                case "PRICEHIGHEST": sortType = Product.QueryParams.SortType.PRICE_HIGHEST; break;
                case "DATENEWEST": sortType = Product.QueryParams.SortType.DATE_NEWEST; break;
                case "DATEOLDEST": sortType = Product.QueryParams.SortType.DATE_OLDEST; break;
                default: sortType = Product.QueryParams.SortType.NONE; break;
            }

            return Lib_Primavera.PriIntegration.ListProducts(indexStart, pageSize, codCategory, codStore, !filterOnSale.Equals(""), !filterPoints.Equals(""), searchString, sortType);
        }

        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class Order
    {
        public string CodOrder { get; set; }
        public string CodClient { get; set; }
        public DateTime Date { get; set; }
        public List<Model.OrderLine> Items { get; set; }
        public double Total { get; set; }
    }
}
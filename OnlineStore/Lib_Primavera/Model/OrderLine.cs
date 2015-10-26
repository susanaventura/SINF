using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class OrderLine
    {
        public string CodProduct { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public String Main_image { get; set; }
        public double ValorIEC { get; set; }
        public double Discount { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrediscount { get; set; }
        public double Total { get; set; }
        public double TotalIEC { get; set; }
    }
}
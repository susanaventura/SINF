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
        public string DeliveryAddress { get; set; }
        public string BillingAddress { get; set; }
        public string Currency { get; set; }
        public List<Model.OrderLine> Items { get; set; }
        public double SubTotal { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalShippingCosts { get; set; }
        public double TotalIEC { get; set; }
        public double Total { get; set; }
        public double TotalIva { get; set; }
    }
}
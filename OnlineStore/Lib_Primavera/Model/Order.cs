using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class Order
    {
        public List<OrderLine> order_lines { get; set; }
        public float sub_total { get; set; }
        public float total_discount { get; set; }
        public float shipping_cost { get; set; }
        public float final_price { get; set; }
    }
}
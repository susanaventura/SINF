using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class OrderLine
    {
        // TODO Adicionar Produto

        public int quantity { get; set; }
        public float price { get; set; }
        public float final_price { get; set; }
    }
}
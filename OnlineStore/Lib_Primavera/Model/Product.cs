using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class Product
    {
        public String codProduct { get; set; }
        public String description { get; set; }
        public String main_image { get; set; }
        public String[] images { get; set; }
        public String price { get; set; } 
        public String unit { get; set; }
        public String availability { get; set; }
        public int points { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class Client
    {
        public String id { get; set; }
        public String name { get; set; }
        public String email { get; set; }
        public String delivery_address { get; set; }
        public String billing_address { get; set; }
        public String taxpayer_num { get; set; }
        public String billing_address { get; set; }
        public String currency { get; set; }
    }
}
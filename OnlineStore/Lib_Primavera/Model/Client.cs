using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class Client
    {
        public String CodClient
        {
            get;
            set;

        }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Delivery_address { get; set; } //morada
        public String Billing_address { get; set; } //morada2 
        public String Taxpayer_num { get; set; }
        public String Currency { get; set; }
       // public int availablePoints { get; set; }
    }
}
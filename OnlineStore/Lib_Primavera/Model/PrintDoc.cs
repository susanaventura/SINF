using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Lib_Primavera.Model
{
    public class PrintDoc 
    {
       public string Module{get; set;}
        public string DocType {get; set;}
        public string Serie {get; set;}
        public long NumDoc {get; set;}
        public string Branch {get; set;}
        public bool Duplicate {get; set;}
        public int Billing_entity {get; set;}
        public string Dest_Path { get; set;  }


    }
}

using Interop.StdBE800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera.Model
{
    public class Store
    {
        public String id { get; set; }
        public String name { get; set; }
        public String address { get; set; }

        public Store() { }
        public Store(StdBELista store)
        {
            this.id = store.Valor("Armazem");
            this.name = store.Valor("Descricao");
            this.address = store.Valor("Morada");
        }
    }
}
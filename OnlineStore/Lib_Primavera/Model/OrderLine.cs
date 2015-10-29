using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.StdBE800;

namespace OnlineStore.Lib_Primavera.Model
{
    public class OrderLine
    {
        public string CodProduct { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double ValorIEC { get; set; }
        public double Discount { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrediscount { get; set; }
        public double Total { get; set; }
        public double TotalIEC { get; set; }

        public OrderLine() { }
        public OrderLine(StdBELista objListLin)
        {
            this.CodProduct = objListLin.Valor("Artigo");
            this.Description = objListLin.Valor("Descricao");
            this.Quantity = objListLin.Valor("Quantidade");
            this.Unit = objListLin.Valor("Unidade");
            this.Discount = objListLin.Valor("TotalDA");
            this.UnitPrice = objListLin.Valor("PrecUnit");
            this.TotalPrediscount = this.Quantity * this.UnitPrice;
            this.ValorIEC = objListLin.Valor("ValorIEC");
            this.TotalIEC = objListLin.Valor("TotalIEC");
            this.Total = objListLin.Valor("PrecoLiquido");
        }
    }    
}
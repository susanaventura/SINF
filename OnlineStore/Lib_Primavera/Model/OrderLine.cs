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
        public float DiscountUnit { get; set; }
        public double ValorIEC { get; set; }
        public double TotalDiscount { get; set; }
        public float DiscountPerc { get; set; }
        public double UnitPrice { get; set; }
        public double TotalLiq { get; set; }
        public double Total { get; set; }
        public double TotalIEC { get; set; }
        public double TaxaIva { get; set; }
        public double TotalIva { get; set; }
        

        public OrderLine() { }
        public OrderLine(StdBELista objListLin)
        {
            this.CodProduct = objListLin.Valor("Artigo");
            this.Description = objListLin.Valor("Descricao");
            this.Quantity = objListLin.Valor("Quantidade");
            this.DiscountPerc = objListLin.Valor("Desconto1");
            this.TotalDiscount = objListLin.Valor("TotalDA");
            this.UnitPrice = objListLin.Valor("PrecUnit");
            this.TotalLiq = objListLin.Valor("PrecoLiquido");
            this.ValorIEC = objListLin.Valor("ValorIEC");
            this.TotalIEC = objListLin.Valor("TotalIEC");
            this.TaxaIva = objListLin.Valor("TaxaIva");
            this.DiscountUnit = (float) (DiscountPerc / 100 * UnitPrice);
            this.TotalIva = objListLin.Valor("TotalIva");
            this.Total = TotalIva + TotalLiq + TotalIEC;
        }
    }    
}
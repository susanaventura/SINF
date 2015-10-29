using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.StdBE800;

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
        public string Status { get; set; }


        public Order() {}
        public Order(StdBELista objListCab, bool getItems = false)
        {
            this.CodOrder = objListCab.Valor("id");
            this.CodClient = objListCab.Valor("Entidade");
            this.Date = objListCab.Valor("Data");
            this.SubTotal = objListCab.Valor("TotalMerc");
            this.TotalDiscount = objListCab.Valor("TotalDesc");
            this.TotalShippingCosts = objListCab.Valor("TotalOutros");
            this.BillingAddress = objListCab.Valor("MoradaFac");
            this.DeliveryAddress = objListCab.Valor("MoradaEntrega");
            this.Currency = objListCab.Valor("Moeda");
            this.TotalIva = objListCab.Valor("TotalIva");
            this.TotalIEC = objListCab.Valor("TotalIEC");
            this.Total = this.SubTotal + this.TotalIva + this.TotalShippingCosts + this.TotalIEC - this.TotalDiscount;
            this.Status = Order.CalcStatus(objListCab);

            
            // Get Items List
            if (getItems)
            {
                this.Items = new List<Model.OrderLine>();
                StdBELista objListLin = PriEngine.Engine.Consulta("SELECT Artigo, Descricao, Quantidade, Unidade, PrecUnit, TotalDA, TotalILiquido, PrecoLiquido, TotalIEC, ValorIEC FROM LinhasDoc WHERE IdCabecDoc='" + this.CodOrder + "' ORDER BY NumLinha");

                while (!objListLin.NoFim())
                {
                    this.Items.Add(new Model.OrderLine(objListLin));
                    objListLin.Seguinte();
                }
            }
        }

        public static string GetQuery(string codOrder = "", string codClient = "", bool fromOnlineStore=false)
        {
            string query = "SELECT ";
            query += 
                "CabecDoc.id, CabecDoc.Entidade, CabecDoc.Data, CabecDoc.Moeda, CabecDoc.TotalMerc, CabecDoc.TotalDesc, CabecDoc.TotalIEC, "+
                "CabecDoc.TotalIva, CabecDoc.TotalOutros, CabecDoc.MoradaEntrega, CabecDoc.MoradaFac, "+
                "CabecDocStatus.Estado ";
            
            query += "FROM CabecDoc ";
            query += "JOIN CabecDocStatus ON CabecDoc.id = CabecDocStatus.IdCabecDoc ";

            query += "WHERE CabecDoc.TipoDoc='ECL'";
            if (codOrder != "") query += "AND CabecDoc.id='" + codOrder + "'";
            if (codClient != "") query += " AND CabecDoc.Entidade='" + codClient + "'";
            if (fromOnlineStore) query += " AND CabecDoc.Observacoes='" + Util.OBS_ONLINE_STORE + "'";

            return query;
        }

        public static string CalcStatus(StdBELista order)
        {
            string r;
            switch ((String)order.Valor("Estado"))
            {
                case "T": r = "Shipped"; break;
                case "G": r = "Waiting for Payment"; break;
                case "P": r = "Approved"; break;
                default: r = "Invalid"; break;
            }
            return r;
        }
    }
}
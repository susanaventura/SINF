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
        public string ShippingMethod {get; set;}
        public string PaymentMethod { get; set; }
        public List<Model.OrderLine> Items { get; set; }
        public double SubTotal { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalShippingCosts { get; set; }
        public double TotalIEC { get; set; }
        public double Total { get; set; }
        public double TotalIva { get; set; }
        public string Status { get; set; }
        public int NumDoc { get; set; }
        public string Serie { get; set; }


        public Order() { }
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
            this.NumDoc = objListCab.Valor("NumDoc");
            this.Serie = objListCab.Valor("Serie");
            this.ShippingMethod = objListCab.Valor("ModoExp");
            this.PaymentMethod = objListCab.Valor("ModoPag");
            


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

        public static string GetQuery(string codOrder = "", string codClient = "", bool fromOnlineStore = false)
        {
            string query = "SELECT ";
            query +=
                "CabecDoc.id, CabecDoc.Entidade, CabecDoc.Data, CabecDoc.Moeda, CabecDoc.TotalMerc, CabecDoc.TotalDesc, CabecDoc.TotalIEC, " +
                "CabecDoc.TotalIva, CabecDoc.TotalOutros, CabecDoc.MoradaEntrega, CabecDoc.NumDoc, CabecDoc.Serie, CabecDoc.MoradaFac, CabecDoc.ModoExp, CabecDoc.ModoPag, " +
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
            string id = order.Valor("Id");
            string serieNum = order.Valor("Serie") + order.Valor("NumDoc");
            switch ((String)order.Valor("Estado"))
            {
                case "T":
                    if (isCanceled(id)) r = "Canceled";
                    else if (isClosed(id)) r = "Delivered";
                    else if (isTransformed(serieNum, "FA"))
                        r = "Paid. Processing";
                    else if (isTransformed(serieNum, "CR"))
                        r = "Shipped";
                    else r = "Incoherence dectected";
                    break;

                case "G":
                    if (!isTransformed(serieNum, "FA") && !isTransformed(serieNum, "CR")) r = "Waiting for Payment";
                    else
                        r = "Incoherence detected";
                    break;

                case "P": r = "Approved"; break;

                default: r = "Invalid"; break;
            }

            return r;
        }

        /* Verifies if the order was transformed in other document
         *
         */
        private static bool isTransformed(string serieNum, string docType)
        {

            StdBELista objListLin = PriEngine.Engine.Consulta("SELECT id FROM CabecDoc WHERE CabecDoc.Requisicao='" + serieNum + "' and CabecDoc.TipoDoc='" + docType + "'");

            if (objListLin.NumLinhas()==0) return false;

            return true;

        }

        private static bool isClosed(string codOrder)
        {
            StdBELista objList = PriEngine.Engine.Consulta("SELECT Fechado FROM CabecDocStatus WHERE IdCabecDoc='" + codOrder + "'");
           return objList.Valor("Fechado");
            
        }

        private static bool isCanceled(string codOrder)
        {
            StdBELista objList = PriEngine.Engine.Consulta("SELECT Anulado FROM CabecDocStatus WHERE IdCabecDoc='" + codOrder + "'");
            return objList.Valor("Anulado");
            
        }

    }
}
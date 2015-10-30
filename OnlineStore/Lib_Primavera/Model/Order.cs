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
        public String ShippementDate { get; set; }
        public String ShippementHour { get; set; }


        public Order() { }
        public Order(StdBELista objListCab, bool getItems = false)
        {
            this.CodOrder = objListCab.Valor("id");
            this.CodClient = objListCab.Valor("Entidade");
            //this.Date = objListCab.Valor("Data");
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
           
            switch ((String)order.Valor("Estado"))
            {
                case "T":
                    if (isCanceled(id)) r = "Canceled";

                    else
                    {
                        Order billing = getBilling(order);
                        if (billing == null) r = "incoherence detected";
                        else
                        {
                            if (orderIsPaid(billing))
                            {
                                DateTime date = Convert.ToDateTime(billing.ShippementDate);
                                //check shipping date
                                if (DateTime.Compare(date, DateTime.Now) > 0)
                                    r = "Paid. Processing";
                                else
                                {
                                    DateTime time = Convert.ToDateTime(billing.ShippementHour);
                                    if (DateTime.Compare(time, DateTime.Now) > 0)
                                        r = "Paid. Processing";
                                    else r = "Shipped";
                                }


                            }
                            else r = "Wating for payment";
                        }
                    }

                    break;

                case "G":
                   r = "Wating for payment"; break;

                case "P": r = "Wating for payment"; break;

                default: r = "Invalid"; break;
            }

            return r;
        }



        private static Order getBilling(StdBELista order)
        {
            StdBELista billing = PriEngine.Engine.Consulta("SELECT Serie, NumDoc, HoraCarga, DataCarga FROM CabecDoc WHERE TipoDoc='FA' and RefDocOrig='ECL "+order.Valor("NumDoc")+"/"+order.Valor("Serie")+"'");
            if (billing.Vazia()) return null;

            Order b = new Order();
            b.Serie = billing.Valor("Serie"); b.NumDoc = billing.Valor("NumDoc");
            b.ShippementHour = billing.Valor("HoraCarga");
            b.ShippementDate = billing.Valor("DataCarga");
            return b;
        }

        private static bool isClosed(string codOrder)
        {
            StdBELista objList = PriEngine.Engine.Consulta("SELECT Fechado FROM CabecDocStatus WHERE IdCabecDoc='" + codOrder + "'");
           return objList.Valor("Fechado");
            
        }

        private static bool orderIsPaid(Order billing)
        {
            StdBELista objListLin = PriEngine.Engine.Consulta("SELECT NumDoc FROM CabLiq WHERE TipoDoc='RE' and DocLiq='FA " + billing.Serie + "/" + billing.NumDoc+"'");
            return !objListLin.Vazia();

        }



        private static bool isCanceled(string codOrder)
        {
            StdBELista objList = PriEngine.Engine.Consulta("SELECT Anulado FROM CabecDocStatus WHERE IdCabecDoc='" + codOrder + "'");
            return objList.Valor("Anulado");
            
        }

    }
}
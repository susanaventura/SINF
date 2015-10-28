using Interop.GcpBE800;
using Interop.StdBE800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera
{
    public class PriIntegration
    {

        //START CLIENT
        #region Client

        public static Lib_Primavera.Model.Client GetClient(string codClient)
        {
            Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();

            GcpBECliente objCli = new GcpBECliente();
            Model.Client myCli = new Model.Client();

            if (Util.checkCredentials())
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codClient))
                {
                    objCli = PriEngine.Engine.Comercial.Clientes.Edita(codClient);
                    myCli.CodClient = objCli.get_Cliente();
                    myCli.Name = objCli.get_Nome();
                    myCli.Currency = objCli.get_Moeda();
                    myCli.Email = objCli.get_B2BEnderecoMail();
                    myCli.Taxpayer_num = objCli.get_NumContribuinte();
                    myCli.Address = objCli.get_Morada();

                    return myCli;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static Lib_Primavera.Model.ErrorResponse CreateClient(Model.Client cli)
        {

            try
            {
                if (!Util.checkCredentials()) return Util.ErrorWrongCredentials();
                return Util.setClientValues(new GcpBECliente(), cli, true);
            }

            catch (Exception ex)
            {
                return Util.ErrorException(ex);
            }

        }



        public static Lib_Primavera.Model.ErrorResponse UpdCliente(Lib_Primavera.Model.Client client)
        {

            GcpBECliente objCli = new GcpBECliente();

            try
            {

                if (!Util.checkCredentials()) return Util.ErrorWrongCredentials();

                if (!PriEngine.Engine.Comercial.Clientes.Existe(client.CodClient)) return Util.ErrorClientNotFound();


                objCli = PriEngine.Engine.Comercial.Clientes.Edita(client.CodClient);

                objCli.set_EmModoEdicao(true);

                return Util.setClientValues(objCli, client, true);


            }

            catch (Exception ex)
            {
                return Util.ErrorException(ex);
            }

        }


        public static Lib_Primavera.Model.ErrorResponse DelCliente(string codCliente)
        {

            try
            {
                if (!Util.checkCredentials()) return Util.ErrorWrongCredentials();


                if (!PriEngine.Engine.Comercial.Clientes.Existe(codCliente)) return Util.ErrorClientNotFound();

                PriEngine.Engine.Comercial.Clientes.Remove(codCliente);

                Lib_Primavera.Model.ErrorResponse error = new Model.ErrorResponse();
                error.Error = 0;
                error.Description = "Client deleted";
                return error;

            }

            catch (Exception ex)
            {
                return Util.ErrorException(ex);
            }

        }






        #endregion Client; //END CLIENT


        //START PRODUCT
        #region Product


        public static Lib_Primavera.Model.Product GetProduct(string codProduct)
        {


            if (Util.checkCredentials())
            {
                if (PriEngine.Engine.Comercial.Artigos.Existe(codProduct))
                {
                    
                    StdBELista objArtigo = PriEngine.Engine.Consulta(
                   
                    Model.Product.GetQuery(0, 1, codProduct)
                    );
                    return new Model.Product(objArtigo);
                }
                else return null;
            }
            else return null;
        }

        public static List<Model.Product> ListProducts(int offset = 0, int limit = 1, string codCategory = "", string codStore = "", bool filterOnSale = false, bool filterPoints = false)
        {

            StdBELista objList;
            List<Model.Product> listArts = new List<Model.Product>();

            if (Util.checkCredentials())
            {
                objList = PriEngine.Engine.Consulta(Model.Product.GetQuery(offset, limit, "", codCategory, codStore, filterOnSale, filterPoints));

                for (; !objList.NoFim(); objList.Seguinte())
                    listArts.Add(new Model.Product(objList));

                return listArts;
            }
            else return null;
        }

        #endregion Product; //END PRODUCT

        #region Categories

        public static List<Model.Category> Categories_List()
        {
            StdBELista objList;
            List<Model.Category> categories = new List<Model.Category>();

            if (PriEngine.InitializeCompany(OnlineStore.Properties.Settings.Default.Company.Trim(), OnlineStore.Properties.Settings.Default.User.Trim(), OnlineStore.Properties.Settings.Default.Password.Trim()))
            {
                objList = PriEngine.Engine.Consulta("SELECT Descricao FROM familias");
                while (!objList.NoFim())
                {
                    Model.Category newCat = new Model.Category();

                    newCat.name = objList.Valor("Descricao");

                    categories.Add(newCat);

                    objList.Seguinte();

                }
                return categories;
            }
            else
                return null;

        }


        #endregion Categories
    
        //START ORDER
        #region Order;

        public static Lib_Primavera.Model.Order GetOrder(string codOrder)
        {
            Model.Order order = new Model.Order();
            List<Model.OrderLine> orderLine_list = new List<Model.OrderLine>();

            StdBELista objListCab;
            StdBELista objListLin;

            if (!Util.checkCredentials()) return null;

            objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Data, Moeda, TotalMerc, TotalDesc, TotalIEC, TotalIva, TotalOutros, MoradaEntrega, MoradaFac From CabecDoc where TipoDoc='ECL' AND id='" + codOrder + "'");


            order = new Model.Order();
            order.CodOrder = objListCab.Valor("id");
            order.CodClient = objListCab.Valor("Entidade");
            order.Date = objListCab.Valor("Data");
            order.SubTotal = objListCab.Valor("TotalMerc");
            order.TotalDiscount = objListCab.Valor("TotalDesc");
            order.TotalShippingCosts = objListCab.Valor("TotalOutros");
            order.BillingAddress = objListCab.Valor("MoradaFac");
            order.DeliveryAddress = objListCab.Valor("MoradaEntrega");
            order.Currency = objListCab.Valor("Moeda");
            order.TotalIva = objListCab.Valor("TotalIva");
            order.TotalIEC = objListCab.Valor("TotalIEC");
            order.Total = order.SubTotal + order.TotalIva + order.TotalShippingCosts + order.TotalIEC - order.TotalDiscount;


            objListLin = PriEngine.Engine.Consulta("SELECT Artigo, Descricao, Quantidade, Unidade, PrecUnit, TotalDA, TotalILiquido, PrecoLiquido, TotalIEC, ValorIEC from LinhasDoc where IdCabecDoc='" + order.CodOrder + "' order By NumLinha");

            orderLine_list = new List<Model.OrderLine>();

            while (!objListLin.NoFim())
            {

                Model.OrderLine orderLine = new Model.OrderLine();
                orderLine.CodProduct = objListLin.Valor("Artigo");
                orderLine.Description = objListLin.Valor("Descricao");
                orderLine.Quantity = objListLin.Valor("Quantidade");
                orderLine.Unit = objListLin.Valor("Unidade");
                orderLine.Discount = objListLin.Valor("TotalDA");
                orderLine.UnitPrice = objListLin.Valor("PrecUnit");
                orderLine.TotalPrediscount = orderLine.Quantity * orderLine.UnitPrice;
                orderLine.ValorIEC = objListLin.Valor("ValorIEC");
                orderLine.TotalIEC = objListLin.Valor("TotalIEC");
                orderLine.Total = objListLin.Valor("PrecoLiquido");


                orderLine_list.Add(orderLine);
                objListLin.Seguinte();
            }

            order.Items = orderLine_list;

            return order;
        }



        public static List<Model.Order> ListOrders(string codClient, bool fromOnlineStore)
        {
            if (!Util.checkCredentials()) return null;


            List<Model.Order> listOrders = new List<Model.Order>();

            StdBELista objListCab = new StdBELista();

            String query = "SELECT id, Entidade, Data, Moeda, TotalMerc, TotalDesc, TotalIEC, TotalIva, TotalOutros, MoradaEntrega, MoradaFac From CabecDoc where TipoDoc='ECL'";

            if(!codClient.Equals("")) query+= " AND Entidade='" + codClient + "'";
            if (fromOnlineStore) query += " AND Observacoes='" + Util.OBS_ONLINE_STORE + "'";

            objListCab = PriEngine.Engine.Consulta(query);

            while (!objListCab.NoFim())
            {
                Model.Order order = GetOrder(objListCab.Valor("id"));
                listOrders.Add(order);
                objListCab.Seguinte();
            }

            return listOrders;
        }



        public static Model.ErrorResponse NewOrder(Model.Order order)
        {
            GcpBEDocumentoVenda myEnc = new GcpBEDocumentoVenda();

            GcpBELinhaDocumentoVenda myLin = new GcpBELinhaDocumentoVenda();

            GcpBELinhasDocumentoVenda myLinhas = new GcpBELinhasDocumentoVenda();

            PreencheRelacaoVendas rl = new PreencheRelacaoVendas();
            List<Model.OrderLine> lstlindv = new List<Model.OrderLine>();


            if (Util.checkCredentials())
            {
                try
                {
                    // Atribui valores ao cabecalho do doc
                    myEnc.set_Entidade(order.CodClient);
                    myEnc.set_Serie("A");
                    myEnc.set_Tipodoc("ECL");
                    myEnc.set_TipoEntidade("C");
                    myEnc.set_DataDoc(order.Date);
                    myEnc.set_Morada(order.DeliveryAddress);
                    myEnc.set_MoradaFac(order.BillingAddress);
                    myEnc.set_Observacoes(Util.OBS_ONLINE_STORE);

                    // Linhas do documento para a lista de linhas
                    lstlindv = order.Items;
                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);
                    foreach (Model.OrderLine lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodProduct, lin.Quantity, "", "", lin.UnitPrice, lin.Discount);
                    }
                }
                catch (Exception ex)
                {
                    return Util.ErrorException(ex);
                }

                // PriEngine.Engine.Comercial.Compras.TransformaDocumento(
                try
                {
                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Vendas.Actualiza(myEnc, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                }
                catch (Exception ex)
                {
                    PriEngine.Engine.DesfazTransaccao();
                    return Util.ErrorException(ex);
                }

                return Util.Success();
            }
            else return Util.ErrorWrongCredentials();
        }

        #endregion Order
    
    }
}
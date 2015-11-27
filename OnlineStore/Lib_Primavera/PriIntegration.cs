using Interop.GcpBE800;
using Interop.StdBE800;
using OnlineStore.Lib_Primavera.Model;
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


            if (Util.checkCredentials())
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codClient))
                {

                    StdBELista objClient = PriEngine.Engine.Consulta(Model.Client.GetQuery(codClient));

                    return new Model.Client(objClient);
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

                    Product.QueryParams param = new Product.QueryParams();
                    param.CodProduct = codProduct;
                    StdBELista objArtigo = PriEngine.Engine.Consulta( Model.Product.GetQuery(param) );
                    return new Model.Product(objArtigo);
                }
                else return null;
            }
            else return null;
        }

        public static ProductListing ListProducts(int offset = 0, int limit = 1, string codCategory = "", string codStore = "", bool filterOnSale = false, bool filterPoints = false, bool sortDate=false, string searchString = "", bool sortLastSold = false)
        {

            StdBELista objList;
            ProductListing productListing = new ProductListing();
            productListing.products = new List<Model.Product>();

            if (Util.checkCredentials())
            {
                Product.QueryParams param = new Product.QueryParams();
                param.Offset = offset;
                param.Limit = limit;
                param.CodCategory = codCategory;
                param.CodStore = codStore;
                param.FilterOnSale = filterOnSale;
                param.FilterPoints = filterPoints;
                param.SortDate = sortDate;
                param.SearchString = searchString;
                param.SortLastSold = sortLastSold;

                // Product List
                objList = PriEngine.Engine.Consulta(Model.Product.GetQuery(param));

                for (; !objList.NoFim(); objList.Seguinte())
                    productListing.products.Add(new Model.Product(objList));

                // Hit count
                param.Count = true;
                objList = PriEngine.Engine.Consulta(Model.Product.GetQuery(param));
                productListing.numResults = objList.Valor("Count");

                // Page Size
                productListing.pageSize = limit;

                return productListing;
            }
            else return null;
        }

        #endregion Product; //END PRODUCT

        //START CATEGORIES
        #region Categories

        public static List<Model.Category> Categories_List()
        {
            StdBELista objList;
            List<Model.Category> categories = new List<Model.Category>();

            if (PriEngine.InitializeCompany(OnlineStore.Properties.Settings.Default.Company.Trim(), OnlineStore.Properties.Settings.Default.User.Trim(), OnlineStore.Properties.Settings.Default.Password.Trim()))
            {
                objList = PriEngine.Engine.Consulta("SELECT Familia, Descricao FROM familias");
                while (!objList.NoFim())
                {
                    Model.Category newCat = new Model.Category();

                    newCat.name = objList.Valor("Descricao");
                    newCat.code = objList.Valor("Familia");

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
            if (!Util.checkCredentials()) return null;

            StdBELista objListCab = PriEngine.Engine.Consulta(Order.GetQuery(codOrder));
            if (objListCab.NoFim()) return null;
            
            return new Model.Order(objListCab, true);
        }

        

        public static List<Model.Order> ListOrders(string codClient, bool fromOnlineStore)
        {
            if (!Util.checkCredentials()) return null;

            List<Model.Order> listOrders = new List<Model.Order>();
            StdBELista objListCab = PriEngine.Engine.Consulta(Order.GetQuery("", codClient, fromOnlineStore));

            while (!objListCab.NoFim())
            {
                listOrders.Add(new Order(objListCab, true));
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
                    
                    myEnc.set_Serie("A");
                    myEnc.set_Tipodoc("ECL");
                    myEnc.set_Entidade(order.CodClient);
                    myEnc.set_TipoEntidade("C");
                    myEnc.set_DataDoc(order.Date);
                    myEnc.set_Morada(order.DeliveryAddress);
                    myEnc.set_MoradaFac(order.BillingAddress);
                    myEnc.set_ModoExp("01"); //transportadora
                    myEnc.set_ModoPag("MB");
                    myEnc.set_CondPag("1");
                    myEnc.set_Observacoes(Util.OBS_ONLINE_STORE);

                    // Linhas do documento para a lista de linhas
                    lstlindv = order.Items;
                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);
                    foreach (Model.OrderLine lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodProduct, lin.Quantity, "", "", lin.UnitPrice, lin.TotalDiscount);
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




        #region BillingPDF

        public static Model.PrintDoc PrintDocument(string codOrder, bool duplicate, string dest_path)
        {

            if (!Util.checkCredentials()) return null;

            StdBELista objOrderCab = PriEngine.Engine.Consulta("SELECT ModuloOrigem, TipoDoc, Serie, NumDoc, Filial, TipoEntidadeFac From CabecDoc where TipoDoc='FA' AND id='" + codOrder + "'");

            PrintDoc doc = new PrintDoc();
            doc.Module = objOrderCab.Valor("ModuloOrigem");
            doc.DocType = objOrderCab.Valor("TipoDoc");
            doc.Serie = objOrderCab.Valor("Serie");
            doc.NumDoc = objOrderCab.Valor("NumDoc");
            doc.Branch = objOrderCab.Valor("Filial");
            doc.Duplicate = duplicate;
            doc.Dest_Path = dest_path;
            doc.Billing_entity = objOrderCab.Valor("TipoEntidadeFac");

            Interop.StdPlatBS800.StdBSGridImpressao print = new Interop.StdPlatBS800.StdBSGridImpressao();
            print.Imprimir(codOrder);

            return doc;

            //Interop.StdPlatBE800.StdBSGridImpressao a;

            /*if(PriEngine.Engine.DSO.Comercial.Vendas.ImprimeDocumento(doc.Module, doc.DocType, doc.Serie, doc.NumDoc, doc.Branch, duplicate, dest_path, doc.Billing_entity))*/
               // return doc;
            //else return null;
        }



        
        #endregion 


        //START STORE
        #region Store

        public static Lib_Primavera.Model.Store getStore(string codStore)
        {
            GcpBEArmazem objStore = new GcpBEArmazem();
            Model.Store store = new Model.Store();

            if (Util.checkCredentials())
            {
                if (PriEngine.Engine.Comercial.Armazens.Existe(codStore))
                {
                    objStore = PriEngine.Engine.Comercial.Armazens.Edita(codStore);

                    store.id = objStore.get_Armazem();
                    store.name = objStore.get_Descricao();
                    store.address = objStore.get_Morada();

                    return store;
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

        public static List<Model.Store> listStores()
        {
            StdBELista storeList;
            List<Model.Store> finalList = new List<Model.Store>();

            if (Util.checkCredentials())
            {
                storeList = PriEngine.Engine.Consulta("SELECT Armazens.Armazem, Armazens.Descricao, Armazens.Morada FROM Armazens");

                for (; !storeList.NoFim(); storeList.Seguinte())
                {
                    finalList.Add(new Model.Store(storeList));
                }

                return finalList;
            }
            else
            {
                return null;
            }
        }

        #endregion Store;
    
    }
}
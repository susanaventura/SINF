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
                    String sql = Model.Product.GetQuery(0, 1, false, codProduct);
                    System.Diagnostics.Debug.WriteLine(sql);
                    System.Diagnostics.Debug.WriteLine("");
                    System.Diagnostics.Debug.WriteLine("");
                    System.Diagnostics.Debug.WriteLine("");
                    System.Diagnostics.Debug.WriteLine("");
                    StdBELista objArtigo = PriEngine.Engine.Consulta(
                        /*    "SELECT Artigo.Artigo, Artigo.Descricao, Artigo.UnidadeBase, ArtigoMoeda.PVP1, ArtigoMoeda.Moeda, Anexos.Id From Artigo " +
                        "JOIN ArtigoMoeda ON Artigo.Artigo = ArtigoMoeda.Artigo " +
                        "LEFT JOIN Anexos ON Artigo.Artigo = Anexos.Chave AND Anexos.Tabela=4 AND Anexos.Tipo='IPR'"+
                        "WHERE Artigo.Artigo='"+codProduct+"'" */
                    sql
                    );
                    return new Model.Product(objArtigo, true);
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
                objList = PriEngine.Engine.Consulta(Model.Product.GetQuery(offset, limit, true, "", codCategory, codStore));

                for (; !objList.NoFim(); objList.Seguinte())
                    listArts.Add(new Model.Product(objList, false));

                return listArts;
            }
            else return null;
        }

        #endregion Product; //END PRODUCT

        //START ORDER
        #region Order

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
                    //myEnc.set_DataDoc(dv.Data);
                    myEnc.set_Entidade(order.CodClient);
                    myEnc.set_Serie("A");
                    myEnc.set_Tipodoc("ECL");
                    myEnc.set_TipoEntidade("C");
                    myEnc.set_DataDoc(order.Date);
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
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
                if(!Util.checkCredentials()) return Util.ErrorWrongCredentials();


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

            GcpBEArtigo objArtigo = new GcpBEArtigo();
            GcpBEB2BAnexo objAnexo = new GcpBEB2BAnexo();
            Model.Product myProd = new Model.Product();

            if (Util.checkCredentials())
            {
                if (PriEngine.Engine.Comercial.Artigos.Existe(codProduct))
                {
                    objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codProduct);
                    myProd.codProduct = objArtigo.get_Artigo();
                    myProd.description = objArtigo.get_Descricao();
                    myProd.main_image = "todo";
                    myProd.images = new String[] {"todo1", "todo2", "todo3"};
                    myProd.price = "todo";
                    myProd.unit = objArtigo.get_UnidadeBase();
                    //myProd.points = 0;

                    return myProd;
                }
                else return null;
            }
            else return null;
        }

        public static List<Model.Product> ListProducts(int offset = 0, int limit = 1)
        {

            StdBELista objList;
            Model.Product myProd;
            List<Model.Product> listArts = new List<Model.Product>();

            if (Util.checkCredentials())
            {
                objList = PriEngine.Engine.Consulta("SELECT Artigo.Artigo, Artigo.Descricao, ArtigoMoeda.PVP1, Anexos.Id From Artigo "+
                    "JOIN ArtigoMoeda ON Artigo.Artigo = ArtigoMoeda.Artigo " +
                    "LEFT JOIN Anexos ON Artigo.Artigo = Anexos.Chave AND Anexos.Tabela=4 AND Anexos.Tipo='IPR'"
                    );

                // iterate to the correct index
                for (int i = 0; i < offset && !objList.NoFim(); i++) objList.Seguinte();

                // fetch a page of elements
                for (int i = 0; i < limit && !objList.NoFim(); i++)
                {
                    myProd = new Model.Product();
                    myProd.codProduct = objList.Valor("Artigo");
                    myProd.description = objList.Valor("Descricao");
                    myProd.main_image = objList.Valor("Id");
                    myProd.images = new String[] { myProd.main_image };
                    myProd.price = ((double)objList.Valor("pvp1")).ToString();
                    //myProd.points = 0;

                    listArts.Add(myProd);
                    objList.Seguinte();
                }

                return listArts;
            }
            else return null;
        }

        #endregion Product; //END PRODUCT
    }
}